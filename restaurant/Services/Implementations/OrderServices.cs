using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Data.Enums;
using restaurant.Dtos;
using restaurant.Helper;
using restaurant.Model;
using restaurant.Repositories.Implementations;
using restaurant.Repositories.Interfaces;
using restaurant.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace restaurant.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;


        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<Order>> CreateAsync(Guid userId, OrderCreateDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            var Details = dto.OrderDetails.ToList();
            try
            {
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    TotalPrice = 0m,
                    CustomerNotes = dto.CustomerNotes,
                    DeliveryAddress = dto.DeliveryAddress,
                    Details = new List<OrderDetail>()
                };

                foreach (var item in dto.OrderDetails)
                {
                    var inventory = await _unitOfWork.Inventory.GetByMenuItemIdAsync(item.MenuItemId);
                    if (inventory == null || inventory.MenuItem?.IsDeleted == true)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return ServiceResult<Order>.Failure("الصنف غير موجود او تم حذفه ", 400);
                    }
                    if (inventory.Quantity < item.Quantity && item.Quantity > 0)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return ServiceResult<Order>.Failure("الكمية غير كافية", 400);
                    }

                    inventory.Quantity -= item.Quantity;

                    var price = inventory.MenuItem!.Price * item.Quantity;
                    var ItemPrice = inventory.MenuItem.Price;
                    var OrderDetail = new OrderDetail
                    {
                        MenuItemId = item.MenuItemId,
                        Quantity = item.Quantity,
                        Price = ItemPrice
                    };
                    order.Details.Add(OrderDetail);

                    order.TotalPrice += price;
                }

                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return ServiceResult<Order>.Success(order, "order has created ", 200);
            }
            catch (Exception ex)
            {
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    Console.WriteLine(ex.ToString());
                    return ServiceResult<Order>.Failure("هناك خطا في انشاء الاوردر ", 500);
                }
            }
        }

        public async Task<ServiceResult<IEnumerable<Order>>> GetAllAsync(OrderSpecParams dto)
        {
            try
            {

                var query = _unitOfWork.Orders.GetQueryable();

                var pagedOrders = await query
                    .OrderByDescending(o => o.OrderDate)
                    .Skip((dto.PageNumber - 1) * dto.PageSize)
                    .Take(dto.PageSize)
                    .ToListAsync();

                return ServiceResult<IEnumerable<Order>>.Success(pagedOrders, "تم رجوع الداتا المطلوبه", 200);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ServiceResult<IEnumerable<Order>>.Failure("حدث خطا غير متوقع ", 500);
            }
        }


        public async Task<ServiceResult<Order?>> GetByIdAsync(int id)
        {

            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                return ServiceResult<Order?>.Failure("عفواً، الطلب غير موجود", 404);
            }
            return ServiceResult<Order?>.Success(order, "تم جلب بيانات الطلب بنجاح", 200);
        }


        public async Task<ServiceResult<IEnumerable<Order>>> GetByMenuItemAsync(int menuItemId)
        {
            var _menuItemId = await _unitOfWork.Orders.GetByMenuItemAsync(menuItemId);
            if (_menuItemId == null)
            {
                return ServiceResult<IEnumerable<Order>>.Failure("عفوا الطلب غير موجود", 404);
            }
            return ServiceResult<IEnumerable<Order>>.Success(_menuItemId, "", 200);

        }


        public async Task<ServiceResult<Order?>> UpdateAsync(int id, OrderUpdateDto dto)
        {

            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)

                return ServiceResult<Order?>.Failure("عفو هذا الاوردر غير موجود", 404);
            // 2. حماية البيزنس: منع التعديل لو الأوردر انتهى فعلياً
            if (order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Cancelled)
            {
                return ServiceResult<Order?>.Failure("لا يمكن تعديل طلب تم توصيله أو إلغاؤه", 400);
            }
            if (dto.Status.HasValue)
            {
                order.Status = dto.Status.Value;
            }

            if (!String.IsNullOrWhiteSpace(dto.CustomerNotes))
            {
                order.CustomerNotes = dto.CustomerNotes;
            }

            if (!String.IsNullOrWhiteSpace(dto.DeliveryAddress))
            {
                order.DeliveryAddress = dto.DeliveryAddress;
            }

            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.CompleteAsync();
            return ServiceResult<Order?>.Success(order, "تم تعديل الاوردر بنجاح", 200);
        }


        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                //get order by id 
                var Order = await _unitOfWork.Orders.GetByIdAsync(id);
                if (Order == null)
                {
                    return ServiceResult<bool>.Failure("عفو هذا الاوردر غير موجود", 404);
                }
                if (Order.Status == OrderStatus.Delivered || Order.Status == OrderStatus.Cancelled)
                {
                    return ServiceResult<bool>.Failure("لا يمكن تعديل طلب تم توصيله أو إلغاؤه", 400);
                }
                foreach (var item in Order.Details)
                {
                    var inventory = await _unitOfWork.Inventory.GetByMenuItemIdAsync(item.MenuItemId);
                    if (inventory != null)
                    {
                        inventory.Quantity += item.Quantity; // رجع الكمية للمخزن
                    }
                }
                Order.IsDeleted = true;
                await _unitOfWork.CommitTransactionAsync();
                // await _unitOfWork.CompleteAsync();
                return ServiceResult<bool>.Success(true, "تم المسح بنجاح", 200);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await _unitOfWork.RollbackTransactionAsync();
                return ServiceResult<bool>.Failure("حدث خطا غير متوقع", 500);

            }
        }
    }
}


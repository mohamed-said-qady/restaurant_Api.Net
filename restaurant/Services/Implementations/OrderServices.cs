using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Dtos;
using restaurant.Model;
using restaurant.Repositories.Implementations;
using restaurant.Repositories.Interfaces;
using restaurant.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using restaurant.Data.Enums;

namespace restaurant.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;


        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> CreateAsync(Guid userId, OrderCreateDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            var Details = dto.OrderDetails.ToList();
            try
            {
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending ,
                    TotalPrice = 0m,
                    Details = new List<OrderDetail>()
                };

                foreach (var item in dto.OrderDetails)
                {
                    var inventory = await _unitOfWork.Inventory.GetByMenuItemIdAsync(item.MenuItemId);

                    if (inventory == null)
                        throw new Exception("الصنف غير موجود");

                    if (inventory.Quantity < item.Quantity && item.Quantity > 0)
                        throw new Exception("الكمية غير كافية");

                    inventory.Quantity -= item.Quantity;

                    var price = inventory.MenuItem!.Price * item.Quantity;
                    var OrderDetail = new OrderDetail
                    {
                        MenuItemId = item.MenuItemId,
                        Quantity = item.Quantity
                    };
                    order.Details.Add(OrderDetail);

                    order.TotalPrice += price;
                }

                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.CommitTransactionAsync();

                return order;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync(OrderSpecParams dto)
        {
            try
            {

                var query = _unitOfWork.Orders.GetQueryable();

                var pagedOrders = await query
                    .OrderByDescending(o => o.OrderDate)
                    .Skip((dto.PageNumber - 1) * dto.PageSize)
                    .Take(dto.PageSize)
                    .ToListAsync();

                return pagedOrders;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<Order?> GetByIdAsync(int id)
            => await _unitOfWork.Orders.GetByIdAsync(id);

     
        public async Task<IEnumerable<Order>> GetByMenuItemAsync(int menuItemId)
        {
             var _menuItemId = await _unitOfWork.Orders.GetByMenuItemAsync(menuItemId);
             return _menuItemId;
        }
   

        public async Task<Order> UpdateAsync(int id, OrderUpdateDto dto)
        {
           
                var order = await _unitOfWork.Orders.GetByIdAsync(id);
                if (order == null) return null;

                if (dto.Status.HasValue)
                {
                  order.Status = dto.Status.Value;
                }

                if (!String.IsNullOrEmpty( dto.CustomerNotes))
                 {
                order.CustomerNotes = dto.CustomerNotes;
                 }

                if (!String.IsNullOrWhiteSpace(dto.DeliveryAddress)) {
                order.DeliveryAddress = dto.DeliveryAddress;
                 }

                await _unitOfWork.Orders.UpdateAsync(order);
                await _unitOfWork.CompleteAsync();
                 return order;
            }


        public async Task<bool> DeleteAsync(int id)
        {
                //get order by id 
                var Order = await _unitOfWork.Orders.GetByIdAsync(id);
                if (Order == null)
                {
                    return false;
                }
                await _unitOfWork.Orders.Delete(Order);
               
                await _unitOfWork.CompleteAsync();
                return true;
            }
           
        }
    }


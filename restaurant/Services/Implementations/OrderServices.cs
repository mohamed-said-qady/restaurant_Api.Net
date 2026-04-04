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

namespace restaurant.Services
{
    public class OrderService : IOrderService
    {
        private readonly UnitOfWork _unitOfWork;


        public OrderService(UnitOfWork unitOfWork )
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
                    Status = "Pending",
                    TotalPrice = 0m,
                    Details = new List<OrderDetail>()
                };

                foreach (var item in dto.OrderDetails)
                {
                    var inventory = await _unitOfWork.Inventory.GetByMenuItemIdAsync(item.MenuItemId);

                    if (inventory == null)
                        throw new Exception("الصنف غير موجود");

                    if (inventory.Quantity < item.Quantity && item.Quantity>0)
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

                var pagedOrders = query
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
            => await _orderRepo.GetByIdAsync(id);

        public async Task<IEnumerable<Order>> GetByMenuItemAsync(int menuItemId)
            => await _orderRepo.GetByMenuItemAsync(menuItemId);

        public async Task<Order?> UpdateAsync(int id, OrderUpdateDto dto)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return null;

            order.Status = dto.Status;

            await _orderRepo.UpdateAsync(order);
            await _orderRepo.SaveAsync();

            return order;
        }
        public async Task<bool> DeleteAsync(int id)
        {
         await _unitOfWork.BeginTransactionAsync();
            try
            {
                //get order by id 
                var Order =  await _unitOfWork.Orders.GetByIdAsync(id);
                if (Order == null) {
                   
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;

                }
                await _unitOfWork.Orders.Delete(Order);
                await _unitOfWork.CommitTransactionAsync();
                
                return true;
            }
            catch{
            await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}

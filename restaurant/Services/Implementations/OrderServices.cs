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
        private readonly IOrderRepository _orderRepo;
        private readonly IInventoryRepository _inventoryRepo;
        private readonly AppDbContext _context;

        public OrderService(
            IOrderRepository orderRepo,
            IInventoryRepository inventoryRepo,
            AppDbContext context)
        {
            _orderRepo = orderRepo;
            _inventoryRepo = inventoryRepo;
            _context = context;
        }

        public async Task<Order> CreateAsync(Guid userId, OrderCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

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
                    var inventory = await _inventoryRepo.GetByMenuItemIdAsync(item.MenuItemId);

                    if (inventory == null)
                        throw new Exception("الصنف غير موجود");

                    if (inventory.Quantity < item.Quantity)
                        throw new Exception("الكمية غير كافية");

                    inventory.Quantity -= item.Quantity;

                    var price = inventory.MenuItem!.Price * item.Quantity;

                    order.Details.Add(new OrderDetail
                    {
                        MenuItemId = item.MenuItemId,
                        Quantity = item.Quantity
                    });

                    order.TotalPrice += price;
                }

                await _orderRepo.AddAsync(order);
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
            => await _orderRepo.GetAllAsync();

        public async Task<Order?> GetByIdAsync(int id)
            => await _orderRepo.GetByIdAsync(id);

        public async Task<IEnumerable<Order>> GetByMenuItemAsync(int menuItemId)
            => await _orderRepo.GetByMenuItemAsync(menuItemId);

        public async Task<Order?> UpdateAsync(int id, OrderUpdateDto dto)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return null;

            // عدّل اللي مسموح يتعدل بس
            order.Status = dto.Status;

            await _orderRepo.UpdateAsync(order);
            await _orderRepo.SaveAsync();

            return order;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return false;

            await _orderRepo.Delete(order);
            await _orderRepo.SaveAsync();

            return true;
        }
    }
}

using restaurant.Dtos;
using restaurant.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurant.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateAsync(Guid userId, OrderCreateDto dto);
        Task<Order?> UpdateAsync(int id, OrderUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetByMenuItemAsync(int menuItemId);
    }
}

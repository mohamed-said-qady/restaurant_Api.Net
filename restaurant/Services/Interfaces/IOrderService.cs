using restaurant.Dtos;
using restaurant.Helper;
using restaurant.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurant.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResult<Order>> CreateAsync(Guid userId, OrderCreateDto dto);
        Task<ServiceResult<Order?>> UpdateAsync(int id, OrderUpdateDto dto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<IEnumerable<Order>>> GetAllAsync(OrderSpecParams dto);
        Task<ServiceResult<Order?>> GetByIdAsync(int id);

        Task<ServiceResult<IEnumerable<Order>>> GetByMenuItemAsync(int menuItemId);
    }
}

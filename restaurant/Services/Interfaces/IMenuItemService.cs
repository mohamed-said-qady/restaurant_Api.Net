using restaurant.Model;
using restaurant.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using restaurant.Helper;

namespace restaurant.Services.Interfaces
{
    public interface IMenuItemService
    {
        Task<ServiceResult<IEnumerable<MenuItem>>>GetAllAsync(MenuItemSpecParams dto);
        Task<ServiceResult<MenuItem?>> GetByIdAsync(int id);
        Task<ServiceResult<MenuItem>> CreateAsync(MenuItemCreateDto dto);
        Task<ServiceResult<MenuItem?>> UpdateAsync(int id, MenuItemUpdateDto dto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}

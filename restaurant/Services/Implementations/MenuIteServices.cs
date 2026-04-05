using Microsoft.EntityFrameworkCore;
using restaurant.Dtos;
using restaurant.Model;
using restaurant.Repositories.Implementations;
using restaurant.Repositories.Interfaces;
using restaurant.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace restaurant.Services.Implementations
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork _unitOfWork ;

        public MenuItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync(MenuItemSpecParams dto)
        {
            try
            {
                var query = _unitOfWork.MenuItems.GetQueryable();
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    query = query.Where(m => m.Name.Contains(dto.Name));
                }
                    //var query = _unitOfWork.MenuItems.GetQueryable();
                    var menuItem = await query
                        .OrderByDescending(m => m.Id)
                        .Skip((dto.PageNumber - 1) * dto.PageSize)
                        .Take(dto.PageSize)
                        .ToListAsync();
                    return menuItem;
                
            }
            catch (System.Exception ex)
            {
                // Log the exception (ex) here as needed
                throw new System.Exception("An error occurred while retrieving menu items.", ex);
            }
        }
            

        public async Task<MenuItem?> GetByIdAsync(int id)
            => await _unitOfWork.MenuItems.GetByIdAsync(id);

        public async Task<MenuItem> CreateAsync(MenuItemCreateDto dto)
        {
            var item = new MenuItem
            {
                Name = dto.Name,
                Price = dto.Price,
            };

            await _unitOfWork.MenuItems.AddAsync(item);
            await _unitOfWork.MenuItems.SaveAsync();

            return item;
        }

        public async Task<MenuItem?> UpdateAsync(int id, MenuItemUpdateDto dto)
        {
            var item = await _unitOfWork.MenuItems.GetByIdAsync(id);
            if (item == null) return null;

            item.Name = dto.Name;
            item.Price = dto.Price;

            await _unitOfWork.MenuItems.UpdateAsync(item);
            await _unitOfWork.MenuItems.SaveAsync();

            return item;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _unitOfWork.MenuItems.GetByIdAsync(id);
            if (item == null) return false;

            await _unitOfWork.MenuItems.Delete(item);
            await _unitOfWork.MenuItems.SaveAsync();

            return true;
        }
    }

}

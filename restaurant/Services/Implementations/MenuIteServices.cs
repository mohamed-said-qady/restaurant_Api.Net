using restaurant.Dtos;
using restaurant.Model;
using restaurant.Repositories.Implementations;
using restaurant.Repositories.Interfaces;
using restaurant.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurant.Services.Implementations
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepo;

        public MenuItemService(IMenuItemRepository menuItemRepo)
        {
            _menuItemRepo = menuItemRepo;
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
            => await _menuItemRepo.GetAllAsync();

        public async Task<MenuItem?> GetByIdAsync(int id)
            => await _menuItemRepo.GetByIdAsync(id);

        public async Task<MenuItem> CreateAsync(MenuItemCreateDto dto)
        {
            var item = new MenuItem
            {
                Name = dto.Name,
                Price = dto.Price,
            };

            await _menuItemRepo.AddAsync(item);
            await _menuItemRepo.SaveAsync();

            return item;
        }

        public async Task<MenuItem?> UpdateAsync(int id, MenuItemUpdateDto dto)
        {
            var item = await _menuItemRepo.GetByIdAsync(id);
            if (item == null) return null;

            item.Name = dto.Name;
            item.Price = dto.Price;

            await _menuItemRepo.UpdateAsync(item);
            await _menuItemRepo.SaveAsync();

            return item;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _menuItemRepo.GetByIdAsync(id);
            if (item == null) return false;

            await _menuItemRepo.Delete(item);
            await _menuItemRepo.SaveAsync();

            return true;
        }
    }

}

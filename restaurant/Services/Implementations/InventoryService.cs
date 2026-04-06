using Microsoft.IdentityModel.Tokens;
using restaurant.Dtos;
using restaurant.Model;
using restaurant.Repositories.Implementations;
using restaurant.Repositories.Interfaces;
using restaurant.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurant.Services.Implementations
{
    public class InventoryService : IInventoryService
    {
        private readonly UnitOfWork _unitOFWork;

        public InventoryService(UnitOfWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
        }

        public async Task<IEnumerable<InventoryItem>> GetAllAsync(invetorySpecParams dto)
        {
            var result =  _unitOFWork.Inventory.GetQueryable();
            if (!string.IsNullOrEmpty( dto.MenuItemId.ToString))
            {
                result = result.Where(i => i.MenuItemId == dto.MenuItem.Id);
            }
            var Inventory = result
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToList();



            return Inventory;


        } 

        public async Task<InventoryItem?> GetByMenuItemIdAsync(int menuItemId)
            => await _inventoryRepo.GetByMenuItemIdAsync(menuItemId);

        public async Task<InventoryItem> CreateAsync(InventoryCreateDto dto)
        {
            var inventory = new InventoryItem
            {
                MenuItemId = dto.MenuItemId,
                Quantity = dto.Quantity
            };

            await _inventoryRepo.AddAsync(inventory);
            return inventory;
        }

        public async Task<bool> UpdateQuantityAsync(int menuItemId, int quantity)
        {
            var inventory = await _inventoryRepo.GetByMenuItemIdAsync(menuItemId);
            if (inventory == null) return false;

            inventory.Quantity = quantity;
            await _inventoryRepo.SaveAsync();

            return true;
        }


    }
}

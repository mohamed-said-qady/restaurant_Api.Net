using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using restaurant.Dtos;
using restaurant.Model;
using restaurant.Repositories.Implementations;
using restaurant.Repositories.Interfaces;
using restaurant.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace restaurant.Services.Implementations
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOFWork;

        public InventoryService(IUnitOfWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
        }

        public async Task<IEnumerable<InventoryItem>> GetAllAsync(invetorySpecParams dto)
        {
            var result =  _unitOFWork.Inventory.GetQueryable();
            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                result = result.Where(i => i.MenuItem != null &&  i.MenuItem.Name.Contains(dto.Name));       
            }
            var Inventory = await result
                .Include(i => i.MenuItem)
                .OrderByDescending(i =>i.Id)
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync();

          

            return Inventory;


        }

        public async Task<InventoryItem?> GetByMenuItemIdAsync(int menuItemId) {

            try { 
           var Inventory = await _unitOFWork.Inventory.GetByMenuItemIdAsync(menuItemId);
            return Inventory;
             }
             catch (OperationCanceledException)
            {
            return null;
            }
            catch (Exception)
            {
                throw new Exception("السيرفر مضغوط حالياً، يرجى المحاولة بعد قليل.");
            }
        }
             

        public async Task<InventoryItem> CreateAsync(InventoryCreateDto dto)
        {
            var existingInventory = await _unitOFWork.Inventory.GetByMenuItemIdAsync(dto.MenuItemId);
            if (existingInventory != null)
            {
                throw new Exception("المخزون لهذا الصنف موجود بالفعل.");
            }
            var inventory = new InventoryItem
            {
                MenuItemId = dto.MenuItemId,
                Quantity = dto.Quantity
            };

            await _unitOFWork.Inventory.AddAsync(inventory);
                await _unitOFWork.CompleteAsync();
            return inventory;
        }

        public async Task<bool> UpdateQuantityAsync(int menuItemId, int quantity)
        {
            if (quantity < 0) return false;
            var inventory = await _unitOFWork.Inventory.GetByMenuItemIdAsync(menuItemId);
            
            if (inventory == null) return false;

            inventory.Quantity = quantity;
            await _unitOFWork.Inventory.UpdateAsync(inventory);
            await _unitOFWork.CompleteAsync();

            return true;
        }


    }
}

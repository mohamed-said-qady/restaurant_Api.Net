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
using restaurant.Helper;
namespace restaurant.Services.Implementations
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOFWork;

        public InventoryService(IUnitOfWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
        }

        public async Task<ServiceResult<IEnumerable<InventoryItem>>> GetAllAsync(invetorySpecParams dto)
        {
            var result = _unitOFWork.Inventory.GetQueryable();
            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                result = result.Where(i => i.MenuItem != null && i.MenuItem.Name.Contains(dto.Name));
            }
            var Inventory = await result
                .Include(i => i.MenuItem)
                .OrderByDescending(i => i.Id)
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync();



            return ServiceResult<IEnumerable<InventoryItem>>.Success(Inventory, "تم جلب المخزون بنجاح", 200);


        }

        public async Task<ServiceResult<InventoryItem?>> GetByMenuItemIdAsync(int menuItemId)
        {

            var Inventory = await _unitOFWork.Inventory.GetByMenuItemIdAsync(menuItemId);
            if (Inventory == null)
            {
                return ServiceResult<InventoryItem?>.Failure("المخزون غير موجود", 404);
            }
            return ServiceResult<InventoryItem?>.Success(Inventory, "تم جلب المخزون بنجاح", 200);
        }


        public async Task<ServiceResult<InventoryItem>> CreateAsync(InventoryCreateDto dto)
        {
            var existingInventory = await _unitOFWork.Inventory.GetByMenuItemIdAsync(dto.MenuItemId);
            if (existingInventory != null)
            {
                return ServiceResult<InventoryItem>.Failure("هذا المخزون موجود بالفعل", 400);

            }
            var inventory = new InventoryItem
            {
                MenuItemId = dto.MenuItemId,
                Quantity = dto.Quantity
            };

            await _unitOFWork.Inventory.AddAsync(inventory);
            await _unitOFWork.CompleteAsync();
            return ServiceResult<InventoryItem>.Success(inventory, "تم انشاء المخزون بنجاح", 201);
        }

        public async Task<ServiceResult<bool>> UpdateQuantityAsync(int menuItemId, int quantity)
        {
            if (quantity <= 0)
                return ServiceResult<bool>.Failure("يرجي ادخال علي الاقل كميه واحده", 400);
            var inventory = await _unitOFWork.Inventory.GetByMenuItemIdAsync(menuItemId);

            if (inventory == null)
                return ServiceResult<bool>.Failure("عفوا هذا المخزون غير موجود", 400);

            inventory.Quantity = quantity;

            await _unitOFWork.Inventory.UpdateAsync(inventory);
            await _unitOFWork.CompleteAsync();

            return ServiceResult<bool>.Success(true, "تم تعديل الكميه بنجاح ", 201);
        }


    }
}

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
        {
            try
            {
                var item = await _unitOfWork.MenuItems.GetByIdAsync(id);

                return item;
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

        public async Task<MenuItem> CreateAsync(MenuItemCreateDto dto)
        {
            var exist = _unitOfWork.MenuItems.GetQueryable();
            if (exist.Any(MI => MI.Name == dto.Name))
            {
                throw new Exception("هذا الصنف موجود بالفعل يرجي اختبيار اسم مختلف ");
            }
            
            var item = new MenuItem
            {
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description
            };

            await _unitOfWork.MenuItems.AddAsync(item);
            await _unitOfWork.CompleteAsync();

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
            try
            {
                var item = await _unitOfWork.MenuItems.GetByIdAsync(id);
                if (item == null) return false;

                await _unitOfWork.MenuItems.Delete(item);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (DbUpdateException )
            {
                // هنا همسك غلطة الـ Restrict اللي معموله في الـ ModelBuilder
              
                throw new Exception("لا يمكن مسح هذا الصنف لأنه مرتبط بطلبات سابقة في النظام. يمكنك تعديل حالته ليكون 'غير متاح' بدلاً من المسح.");
            }
        }
    }

}

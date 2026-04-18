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
using restaurant.Helper;
using System.Collections;
using Microsoft.IdentityModel.Tokens;


namespace restaurant.Services.Implementations
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<IEnumerable<MenuItem>>> GetAllAsync(MenuItemSpecParams dto)
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
            if (!menuItem.Any())
            {
                return ServiceResult<IEnumerable<MenuItem>>.Failure("لايوجد اصناف", 404);
            }
            return ServiceResult<IEnumerable<MenuItem>>.Success(menuItem, "تم استرجاع الاصناف بنجاح", 200);
        }


        public async Task<ServiceResult<MenuItem?>> GetByIdAsync(int id)
        {

            var item = await _unitOfWork.MenuItems.GetByIdAsync(id);
            if (item == null)
            {
                return ServiceResult<MenuItem?>.Failure("هذا الصنف غير موجود", 404);
            }
            return ServiceResult<MenuItem?>.Success(item, "تم استرجاع الصنف بنجاح", 200);


        }

        public async Task<ServiceResult<MenuItem>> CreateAsync(MenuItemCreateDto dto)
        {
            var exist = _unitOfWork.MenuItems.GetQueryable();
            if (exist.Any(MI => MI.Name == dto.Name))
            {
                return ServiceResult<MenuItem>.Failure("لا يمكن ضافة عنصر لانه موجود سابقا", 400);
            }

            var item = new MenuItem
            {
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description
            };

            await _unitOfWork.MenuItems.AddAsync(item);
            await _unitOfWork.CompleteAsync();

            return ServiceResult<MenuItem>.Success(item, "تمت اضافة العنصر بنجاح", 201);

        }

        public async Task<ServiceResult<MenuItem?>> UpdateAsync(int id, MenuItemUpdateDto dto)
        {
            var item = await _unitOfWork.MenuItems.GetByIdAsync(id);
            if (item == null)
                return ServiceResult<MenuItem?>.Failure("هذا الصنف غير موجود", 404);
            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                item.Name = dto.Name;

            }
            if (dto.Price.HasValue && dto.Price.Value > 0)
            {
                item.Price = dto.Price.Value;
            }
            else if (dto.Price < 0)
            {
                return ServiceResult<MenuItem?>.Failure("لا يمكن أن يكون السعر بالسالب", 400);
            }

            await _unitOfWork.MenuItems.UpdateAsync(item);
            await _unitOfWork.CompleteAsync();

            return ServiceResult<MenuItem?>.Success(item, "تم تعديل الصنف بنجاح", 200);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {

            var item = await _unitOfWork.MenuItems.GetByIdAsync(id);

            if (item == null || item.IsDeleted == true)
            {
                return ServiceResult<bool>.Failure("هذا الصنفغير موجود او تم حذفه", 400);
            }
            var hasMovements = await _unitOfWork.Orders.HasAnyOrdersWithMenuItem(id);
            if (hasMovements)
            {
                return ServiceResult<bool>.Failure("هذا الصنف لايمكن حذفه لانه تمت عليه حركه", 400);

            }
            await _unitOfWork.MenuItems.Delete(item);
            await _unitOfWork.CompleteAsync();
            return ServiceResult<bool>.Success(true, "تم حذف الصنف بنجاح", 200);
        }


    }

}

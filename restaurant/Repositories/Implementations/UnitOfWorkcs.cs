using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using restaurant.Data;
using restaurant.Model;
using restaurant.Repositories.Interfaces;
using restaurant.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace restaurant.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IJwtService _jwtService;
        //private readonly IRolePermissionService _rolePermissionService;
        public IOrderRepository Orders { get; private set; }
        public IMenuItemRepository MenuItems { get; private set; }
        public IInventoryRepository Inventory { get; private set; }
        public IRolePermissionRepository RolePermissions { get; private set; }

        public UnitOfWork(
            AppDbContext context,
            IOrderRepository orders,
            IMenuItemRepository menuItems,
            IInventoryRepository inventory,
            IRolePermissionRepository rolePermissions)
        {
            _context = context;
            Orders = orders;
            MenuItems = menuItems;
            Inventory = inventory;
            RolePermissions = rolePermissions;
        }

        // 1. بدأ العملية
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        // 2. حفظ التغييرات وتأكيد العملية
        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync(); // لازم نحفظ الداتا الأول
                if (_transaction != null)
                {
                    await _transaction.CommitAsync(); // نأكد العملية في الداتا بيز
                }
            }
            catch
            {
                await RollbackTransactionAsync(); // لو الحفظ فشل، ارجع في كلامك فوراً
                throw;
            }
            finally
            {
                if (_transaction != null)
                {   
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        // 3. إلغاء كل اللي حصل لو فيه مشكلة
        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();//لغينا العمليه من RAM بس الخط مفتوح 
                await _transaction.DisposeAsync();//اقفل السماعة خلاص، مش عايز حاجة تاني
                _transaction = null;//شيل رقم البنك من على شاشة التليفون عشان لو حبيت أتصل بحد تاني
            }
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
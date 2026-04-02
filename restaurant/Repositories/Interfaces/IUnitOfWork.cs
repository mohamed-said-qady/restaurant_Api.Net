using Microsoft.EntityFrameworkCore.Storage;
using restaurant.Repositories.Implementations;
using restaurant.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace restaurant.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // الـ Repositories اللي أنت سجلتها يا محمد
        IOrderRepository Orders { get; }
        IMenuItemRepository MenuItems { get; }
        IInventoryRepository Inventory { get; }
        IRolePermissionRepository RolePermissions { get; }

        // العمليات الأساسية
        Task<int> CompleteAsync(); // دي بديلة لـ SaveChanges
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(); 
        Task RollbackTransactionAsync();
    }
}
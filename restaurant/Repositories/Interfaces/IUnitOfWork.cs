using Microsoft.EntityFrameworkCore.Storage;
using restaurant.Repositories.Implementations;
using restaurant.Repositories.Interfaces;
using System;
using System.Threading.Tasks;


namespace restaurant.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderRepository Orders { get; }
        IMenuItemRepository MenuItems { get; }
        IInventoryRepository Inventory { get; }
        IRolePermissionRepository RolePermissions { get; }

        Task<int> CompleteAsync();

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        // 6. الأستيكة: لو حصل أي Error (زي إن الكمية مش كافية أو السيرفر وقع)
        Task RollbackTransactionAsync();
    }
}
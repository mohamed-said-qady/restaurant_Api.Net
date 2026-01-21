using System.Threading.Tasks;
using System.Collections.Generic;
using restaurant.Model;
using restaurant.Data;
using Microsoft.EntityFrameworkCore;
using restaurant.Repositories.Interfaces;

namespace restaurant.Repositories.Implementations
{
    public class MenuItemRepository : GenericRepository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}

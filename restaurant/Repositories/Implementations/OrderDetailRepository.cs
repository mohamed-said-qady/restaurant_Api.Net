using System.Threading.Tasks;
using System.Collections.Generic;
using restaurant.Model;
using restaurant.Data;
using Microsoft.EntityFrameworkCore;
using restaurant.Repositories.Interfaces;

namespace restaurant.Repositories.Implementations
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {

        public OrderDetailRepository(AppDbContext context) : base(context)
        {


        }
    }
}

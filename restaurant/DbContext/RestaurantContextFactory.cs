//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;

//namespace restaurant.DbContext
//{
//    public class RestaurantContextFactory : IDesignTimeDbContextFactory<RestaurantContext>
//    {
//        public RestaurantContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<RestaurantContext>();

//            // connection string هنا مباشر عشان الميجريشن
//            optionsBuilder.UseSqlServer("Server=.;Database=RestaurantDb;Trusted_Connection=True;TrustServerCertificate=True");

//            return new RestaurantContext(optionsBuilder.Options);
//        }
//    }
//}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using restaurant.Data; // تأكد إن ده الـ Namespace اللي فيه الـ AppDbContext

namespace restaurant.Data // أو المكان اللي تحبه
{
    public class RestaurantContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // الـ Connection String بتاعتك من الـ appsettings.json
            optionsBuilder.UseSqlServer("Server=.;Database=RestaurantDb;Trusted_Connection=True;TrustServerCertificate=True");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
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

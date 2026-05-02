using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using restaurant.Data; // اسم الـ Namespace بتاع الـ DbContext عندك
namespace RestaurantSystem.IntegrationTests.Helpers;
public class TestingWebAppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{ 
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            // 1. مسح شامل لكل ما يخص الـ DbContextOptions
            var contextOptions = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (contextOptions != null) services.Remove(contextOptions);

            // 2. مسح الـ DbContext نفسه لو متسجل بطريقة تانية
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(AppDbContext));
            if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

            // 3. إضافة الـ InMemory Database
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("IntegrationTestDb");
                // السطر ده مهم عشان يمنع الـ Identity من إنها تفتح Internal Service Provider تاني
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            // الكام سطر دول هما "المساحة" اللي هتمسح أي أثر للـ SQL Server
            var descriptors = services.Where(d => d.ServiceType.Name.Contains("DbContextOptions")).ToList();
            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            // أعد تسجيل الـ DbContext بالـ InMemory بوضوح
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("IntegrationTestDb");
            });
        });
    }
}
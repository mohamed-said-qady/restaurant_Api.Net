using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using restaurant.Data;
using restaurant.Model;
using restaurant.Middleware;
using restaurant.Repositories;
using restaurant.Repositories.Implementations;
using restaurant.Repositories.Interfaces;
using restaurant.Services;
using restaurant.Services.Implementations;
using restaurant.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// 1️⃣ Add DbContext
// -------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// -------------------------
// 2️⃣ Add Identity
// -------------------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// -------------------------
// 3️⃣ Add JWT Authentication
// -------------------------
var key = Encoding.UTF8.GetBytes("SUPER_SECRET_KEY");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// -------------------------
// 4️⃣ Add Repositories
// -------------------------
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

// -------------------------
// 5️⃣ Add Services
// -------------------------
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();


// -------------------------
// 6️⃣ Add Controllers
// -------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -------------------------
// 7️⃣ Build app
// -------------------------
var app = builder.Build();

// -------------------------
// 8️⃣ Middleware
// -------------------------
app.UseGlobalExceptionMiddleware();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();  // 👈 لازم قبل Authorization
app.UseAuthorization();

app.MapControllers();

// -------------------------
// 9️⃣ Run
// -------------------------
app.Run();
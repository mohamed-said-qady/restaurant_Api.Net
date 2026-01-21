using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using restaurant.Model;
using System;
using System.Reflection.Emit;

namespace restaurant.Data
{
    public class AppDbContext
        : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany()
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany()
                .HasForeignKey(rp => rp.PermissionId);
            // =========================
            // Order ↔ ApplicationUser (One To Many)
            // =========================
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany() // لو حابب تضيف ICollection<Order> في ApplicationUser خليها WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // Order ↔ OrderDetail (One To Many)
            // =========================
            modelBuilder.Entity<OrderDetail>()
                .HasOne<Order>()
                .WithMany(o => o.Details)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // MenuItem ↔ OrderDetail (One To Many)
            // =========================
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.MenuItem)
                .WithMany()
                .HasForeignKey(od => od.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // MenuItem ↔ InventoryItem (One To One)
            // =========================
            modelBuilder.Entity<InventoryItem>()
                .HasOne(i => i.MenuItem)
                .WithOne()
                .HasForeignKey<InventoryItem>(i => i.MenuItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }



    }
}

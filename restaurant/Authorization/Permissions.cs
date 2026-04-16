using System;

namespace restaurant.Authorization
{
    public static class Permissions
    {
        public const string OrderCreate = "Order.Create";
        public const string OrderUpdate = "Order.Update";
        public const string OrderView = "Order.View";
        public const string OrderViewAll = "Order.ViewAll";
        public const string OrderViewByMenu = "Order.ViewByMenu";
        public const string OrderDelete = "Order.Delete";

        public const string MenuItemCreate = "MenuItem.Create";
        public const string MenuItemUpdate = "MenuItem.Update";
        public const string MenuItemView = "MenuItem.View";
        public const string MenuItemViewAll = "MenuItem.ViewAll";
        public const string MenuItemDelete = "MenuItem.Delete";


        public const string ProductCreate = "Product.Create";
        public const string ProductUpdate = "Product.Update";
        public const string ProductView = "Product.View";

        public const string InventoryView = "Inventory.View";
        public const string InventoryUpdate = "Inventory.Update";
        public const string InventoryCreate = "Inventory.Create";
        public const string InventoryViewByMenu = "Inventory.ViewByMenu";


        public const string RolePermissionManage = "RolePermission.Manage";
    }
}

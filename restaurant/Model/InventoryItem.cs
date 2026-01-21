using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace restaurant.Model
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }      // FK to MenuItem
        public MenuItem? MenuItem { get; set; }  // navigation (optional for swagger)
        public decimal Quantity { get; set; }    // current quantity in stock (units / kg / liter)
        public string Unit { get; set; } = "pcs";// unit label
        public decimal ReorderLevel { get; set; } // when to alert (e.g. 5)
        public bool IsActive { get; set; } = true;
    }
}

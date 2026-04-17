using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using restaurant.Data.Enums;
namespace restaurant.Model
{

    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Precision(18, 2)]
        public decimal TotalPrice { get; set; }


        public OrderStatus Status { get; set; } = OrderStatus.Pending;

 
        public string? CustomerNotes { get; set; } 
        public string? DeliveryAddress { get; set; } 
        public decimal? ServiceFee { get; set; }

      
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [JsonIgnore]
        public ICollection<OrderDetail> Details { get; set; } = new List<OrderDetail>();
        public bool IsDeleted { get; set; } = false;
    }


}

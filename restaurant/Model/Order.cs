using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace restaurant.Model
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        [Precision(18, 2)]
        public decimal TotalPrice { get; set; }// بييجي من services

        public string Status { get; set; }

        // FK to Identity User (GUID)
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        [JsonIgnore]
        public ICollection<OrderDetail>? Details { get; set; }
    }
}

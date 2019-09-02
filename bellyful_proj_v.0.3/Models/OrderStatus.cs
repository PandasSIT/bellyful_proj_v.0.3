using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Order = new HashSet<Order>();
        }

        public int StatusId { get; set; }
        public string Content { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}

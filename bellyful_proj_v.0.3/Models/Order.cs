using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int? StatusId { get; set; }
        public int? VolunteerId { get; set; }
        public int RecipientId { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? AssignDatetime { get; set; }
        public DateTime? PickupDatetime { get; set; }
        public DateTime? DeliveredDatetime { get; set; }

        public virtual Recipient Recipient { get; set; }
        public virtual OrderStatus Status { get; set; }
        public virtual Volunteer Volunteer { get; set; }
    }
}

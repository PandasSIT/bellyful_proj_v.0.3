
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//namespace bellyful_proj_v.0.3.ViewModels
namespace bellyful_proj_v._0._3.ViewModels
{
    public class OrderIndexViewModel:IComparable<OrderIndexViewModel>
    {
        [Display(Name = "No.")]
        public int OrderId { get; set; }
       // public string Status { get; set; }
        public int StatusId { get; set; }
        [Display(Name = "Recipient")]
        public string RIdName { get; set; }

        [Display(Name = "Delivery By")]
        public string VIdName { get; set; }
        [Display(Name = "Placed")]
        public DateTime? PlacedTime { get; set; }
        [Display(Name = "Assigned")]
        public DateTime? AssignedTime { get; set; }
        [Display(Name = "PickedUp")]
        public DateTime? PickedUpTime { get; set; }
        [Display(Name = "Delivered")]
        public DateTime? DeliveredTime { get; set; }
        [Display(Name = "Address")]
        public string TheRecipientAddress { get; set; }

        [Display (Name = "Meal Info")]
        public string MealInfo { get; set; }


        [Display(Name = "Dog On Property")]
        public bool TheRecipientDogOnProperty { get; set; }

      

        public int CompareTo(OrderIndexViewModel other)
        {
            int i = other.StatusId - StatusId;
            if (i==0)
            {
                var timespan = other.DeliveredTime- DeliveredTime ;
                if (timespan !=null)
                {
                    i = (int)timespan.Value.TotalSeconds;
                }else if (other.PickedUpTime - PickedUpTime != null)
                {
                    i = (int)(other.PickedUpTime - PickedUpTime).Value.TotalSeconds;
                }
                else if (other.AssignedTime - AssignedTime != null)
                {
                    i = (int)(other.AssignedTime - AssignedTime).Value.TotalSeconds;
                }
                else if (other.PlacedTime - PlacedTime != null)
                {
                    i = (int)(other.PlacedTime - PlacedTime).Value.TotalSeconds;
                }

            }

            return i;
        }
    }
}

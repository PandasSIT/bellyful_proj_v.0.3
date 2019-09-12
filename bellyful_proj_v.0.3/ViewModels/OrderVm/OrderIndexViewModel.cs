using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//namespace bellyful_proj_v.0.3.ViewModels
namespace bellyful_proj_v._0._3.ViewModels
{
    public class OrderIndexViewModel
    {

        public int OrderId { get; set; }
        public string Status { get; set; }
        [Display(Name = "Recipient")]
        public string RIdName { get; set; }

        [Display(Name = "Deliveary Man")]
        public string VIdName { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
         public string PlacedTime { get; set; }

        public string AssignedTime { get; set; }
        public string PickedUpTime { get; set; }
        public string DeliveredTime { get; set; }
    }
}

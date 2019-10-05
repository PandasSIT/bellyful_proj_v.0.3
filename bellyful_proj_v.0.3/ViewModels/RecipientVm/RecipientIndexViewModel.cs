using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bellyful_proj_v._0._3.ViewModels.RecipientVm
{
    public class RecipientIndexViewModel
    {
        [Display(Name = "No.")]//1
        public int RecipientId { get; set; }
        // public string Status { get; set; }
        [Display(Name = "Full Name")]//2
        public string RFullName { get; set; }
        [Display(Name = "Address")]//3
        public string RAddress { get; set; }
        [Display(Name = "PhoneNumber")]//4
        public string RPhone { get; set; }
        [Display(Name = "Email")]  //5
        public string REmail { get; set; }
        [Display(Name = "DogOnProperty")]//6
        public string DogOnProperty { get; set; }
        [Display(Name = "Dietary")]   //7
        public string Dietary { get; set; }

    }
}

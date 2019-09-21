using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace bellyful_proj_v._0._3.ViewModels.VolunteerVM
{
    public class VolunteerEditViewModel
    {
        public int VolunteerId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime Dob { get; set; }
        [Required]
        public string Email { get; set; }
        [Required, DataType(DataType.PhoneNumber)]
        public string PreferredPhone { get; set; }
        public string AlternativePhone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string TownCity { get; set; }
        public string PostCode { get; set; }
        public int? StatusId { get; set; }
        public int? BranchId { get; set; }
        public int? RoleId { get; set; }
        [Required]
        public bool? IsAssignedUserAccount { get; set; }

        //============= EmergencyContact
        [Required,Display(Name ="First Name")]
        public string EFirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string ELastName { get; set; }
        [Required, Display(Name = "Phone Number"),DataType(DataType.PhoneNumber)]
        public string EPhoneNumber { get; set; }
        [Required, Display(Name = "Relationship")]
        public string ERelationship { get; set; }

        //============PoliceInfo
        public DateTime? PoliceVetDate { get; set; }
        public bool? PoliceVetVerified { get; set; }
        //==========TraningInfo
        public bool? DeliveryTraining { get; set; }
        public bool? HSTraining { get; set; }
        public bool? FirstAidRaining { get; set; }
        public string OtherTrainingSkill { get; set; }


        [TempData]
        public string StatusMessage { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace bellyful_proj_v._0._3.Models
{
    public partial class Volunteer
    {
        public Volunteer()
        {
            Order = new HashSet<Order>();
        }

        public int VolunteerId { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required,DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
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
        public bool? IsAssignedUserAccount { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual VolunteerRole Role { get; set; }
        public virtual VolunteerStatus Status { get; set; }
        public virtual VolunteerEmergencyContact VolunteerEmergencyContact { get; set; }
        public virtual VolunteerPoliceInfo VolunteerPoliceInfo { get; set; }
        public virtual VolunteerTrainingInfo VolunteerTrainingInfo { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}

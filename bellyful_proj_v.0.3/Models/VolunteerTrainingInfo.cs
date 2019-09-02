using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class VolunteerTrainingInfo
    {
        public int VolunteerId { get; set; }
        public bool DeliveryTraining { get; set; }
        public bool HSTraining { get; set; }
        public bool FirstAidRaining { get; set; }
        public string OtherTrainingSkill { get; set; }

        public virtual Volunteer Volunteer { get; set; }
    }
}

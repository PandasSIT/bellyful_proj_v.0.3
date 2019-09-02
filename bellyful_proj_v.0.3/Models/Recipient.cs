using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class Recipient
    {
        public Recipient()
        {
            Order = new HashSet<Order>();
        }

        public int RecipientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressNumStreet { get; set; }
        public string TownCity { get; set; }
        public int? Postcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool? DogOnProperty { get; set; }
        public int BranchId { get; set; }
        public int ReferralReasonId { get; set; }
        public string OtherReferralInfo { get; set; }
        public int AdultsNum { get; set; }
        public int Under5ChildrenNum { get; set; }
        public int _510ChildrenNum { get; set; }
        public int _1117ChildrenNum { get; set; }
        public int? DietaryRequirementId { get; set; }
        public string OtherAllergyInfo { get; set; }
        public string AdditionalInfo { get; set; }
        public int? ReferrerId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual DietaryRequirement DietaryRequirement { get; set; }
        public virtual ReferralReason ReferralReason { get; set; }
        public virtual Referrer Referrer { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bellyful_proj_v._0._3.Models
{
    public partial class Recipient
    {
        public Recipient()
        {
            Order = new HashSet<Order>();
        }

        public int RecipientId { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string AddressNumStreet { get; set; }
        [Required]
        public string TownCity { get; set; }
        public int? Postcode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool? DogOnProperty { get; set; }
        [Required]
        public int BranchId { get; set; }
        [Required]
        public int ReferralReasonId { get; set; }
        public string OtherReferralInfo { get; set; }
        [Required]
        public int AdultsNum { get; set; }
        [Required]
        public int Under5ChildrenNum { get; set; }
        [Required]
        public int _510ChildrenNum { get; set; }
        [Required]
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bellyful_proj_v._0._3.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace bellyful_proj_v._0._3.Models
{
    public partial class bellyful_v03Context : DbContext
    {
        public bellyful_v03Context()
        {
        }

        public bellyful_v03Context(DbContextOptions<bellyful_v03Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Batch> Batch { get; set; }
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<DietaryRequirement> DietaryRequirement { get; set; }
        public virtual DbSet<MealInstock> MealInstock { get; set; }
        public virtual DbSet<MealType> MealType { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Recipient> Recipient { get; set; }
        public virtual DbSet<ReferralReason> ReferralReason { get; set; }
        public virtual DbSet<Referrer> Referrer { get; set; }
        public virtual DbSet<ReferrerRole> ReferrerRole { get; set; }
        public virtual DbSet<Volunteer> Volunteer { get; set; }
        public virtual DbSet<VolunteerEmergencyContact> VolunteerEmergencyContact { get; set; }
        public virtual DbSet<VolunteerPoliceInfo> VolunteerPoliceInfo { get; set; }
        public virtual DbSet<VolunteerRole> VolunteerRole { get; set; }
        public virtual DbSet<VolunteerStatus> VolunteerStatus { get; set; }
        public virtual DbSet<VolunteerTrainingInfo> VolunteerTrainingInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=192.168.1.25;Initial Catalog=bellyful_v0.4;User ID=bellyful_dev;Password=Password1!;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Batch>(entity =>
            {
                entity.ToTable("batch");

                entity.Property(e => e.BatchId).HasColumnName("batch_id");

                entity.Property(e => e.AddAmount).HasColumnName("add_amount");

                entity.Property(e => e.MealTypeId).HasColumnName("meal_type_id");

                entity.Property(e => e.ProductionDate)
                    .HasColumnName("production_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.MealType)
                    .WithMany(p => p.Batch)
                    .HasForeignKey(d => d.MealTypeId)
                    .HasConstraintName("a batch has a meal type");
            });

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.ToTable("branch");

                entity.Property(e => e.BranchId).HasColumnName("branch_id");

                entity.Property(e => e.AddressNumStreet)
                    .HasColumnName("address_num_street")
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.TownCity)
                    .HasColumnName("town_city")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DietaryRequirement>(entity =>
            {
                entity.ToTable("dietary_requirement");

                entity.Property(e => e.DietaryRequirementId).HasColumnName("dietary_requirement_id");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(125)
                    .IsUnicode(false);

                entity.Property(e => e.DietaryName)
                    .IsRequired()
                    .HasColumnName("dietary_name")
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.MatchedSetMeal)
                    .HasColumnName("matched_set_meal")
                    .HasMaxLength(125)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MealInstock>(entity =>
            {
                entity.HasKey(e => e.MealTypeId)
                    .HasName("PK__meal_ins__6F7616D8B7770892");

                entity.ToTable("meal_instock");

                entity.Property(e => e.MealTypeId)
                    .HasColumnName("meal_type_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.InstockAmount).HasColumnName("instock_amount");

                entity.HasOne(d => d.MealType)
                    .WithOne(p => p.MealInstock)
                    .HasForeignKey<MealInstock>(d => d.MealTypeId)
                    .HasConstraintName("one meal type has one instock");
            });

            modelBuilder.Entity<MealType>(entity =>
            {
                entity.ToTable("meal_type");

                entity.Property(e => e.MealTypeId).HasColumnName("meal_type_id");

                entity.Property(e => e.MealTypeName)
                    .IsRequired()
                    .HasColumnName("meal_type_name")
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.ShelfLocation)
                    .IsRequired()
                    .HasColumnName("shelf_location")
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.AssignDatetime)
                    .HasColumnName("assign_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedDatetime)
                    .HasColumnName("created_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeliveredDatetime)
                    .HasColumnName("delivered_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.PickupDatetime)
                    .HasColumnName("pickup_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.RecipientId).HasColumnName("recipient_id");

                entity.Property(e => e.StatusId)
                    .HasColumnName("status_id")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.VolunteerId).HasColumnName("volunteer_id");

                entity.HasOne(d => d.Recipient)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.RecipientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("an order has a recipient");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("every order has a status");

                entity.HasOne(d => d.Volunteer)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.VolunteerId)
                    .HasConstraintName("an order can assign a volunteer as delivery man");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK__order_st__3683B531D994162B");

                entity.ToTable("order_status");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Recipient>(entity =>
            {
                entity.ToTable("recipient");

                entity.HasIndex(e => new { e.FirstName, e.LastName, e.AddressNumStreet })
                    .HasName("UQ__recipien__0D43C48A1E7C9BA3")
                    .IsUnique();

                entity.Property(e => e.RecipientId).HasColumnName("recipient_id");

                entity.Property(e => e.AdditionalInfo)
                    .HasColumnName("additional_info")
                    .HasMaxLength(2555)
                    .IsUnicode(false);

                entity.Property(e => e.AddressNumStreet)
                    .IsRequired()
                    .HasColumnName("address_num_street")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AdultsNum).HasColumnName("adults_num");

                entity.Property(e => e.BranchId).HasColumnName("branch_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DietaryRequirementId).HasColumnName("dietary_requirement_id");

                entity.Property(e => e.DogOnProperty).HasColumnName("dog_on_property");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.OtherAllergyInfo)
                    .HasColumnName("other_allergy_info")
                    .HasMaxLength(2555)
                    .IsUnicode(false);

                entity.Property(e => e.OtherReferralInfo)
                    .HasColumnName("other_referral_info")
                    .HasMaxLength(2555)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnName("phone_number")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Postcode).HasColumnName("postcode");

                entity.Property(e => e.ReferralReasonId).HasColumnName("referral_reason_id");

                entity.Property(e => e.ReferrerId).HasColumnName("referrer_id");

                entity.Property(e => e.TownCity)
                    .IsRequired()
                    .HasColumnName("town_city")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Under5ChildrenNum).HasColumnName("under5_children_num");

                entity.Property(e => e._1117ChildrenNum).HasColumnName("11-17_children_num");

                entity.Property(e => e._510ChildrenNum).HasColumnName("5-10_children_num");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.Recipient)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Recipient belones to a Branch");

                entity.HasOne(d => d.DietaryRequirement)
                    .WithMany(p => p.Recipient)
                    .HasForeignKey(d => d.DietaryRequirementId)
                    .HasConstraintName("Recipient has a requirement");

                entity.HasOne(d => d.ReferralReason)
                    .WithMany(p => p.Recipient)
                    .HasForeignKey(d => d.ReferralReasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Recipient has a Reason");

                entity.HasOne(d => d.Referrer)
                    .WithMany(p => p.Recipient)
                    .HasForeignKey(d => d.ReferrerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Recipient has/not has a Referrer");
            });

            modelBuilder.Entity<ReferralReason>(entity =>
            {
                entity.ToTable("referral_reason");

                entity.Property(e => e.ReferralReasonId).HasColumnName("referral_reason_id");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Referrer>(entity =>
            {
                entity.ToTable("referrer");

                entity.HasIndex(e => new { e.FirstName, e.LastName, e.OrganisationName })
                    .HasName("UQ__referrer__21BE543F9515083C")
                    .IsUnique();

                entity.Property(e => e.ReferrerId).HasColumnName("referrer_id");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.OrganisationName)
                    .HasColumnName("organisation_name")
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnName("phone_number")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.TownCity)
                    .HasColumnName("town_city")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Referrer)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Referrer has/not has a role");
            });

            modelBuilder.Entity<ReferrerRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__referrer__760965CC526A5A07");

                entity.ToTable("referrer_role");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasColumnName("role_name")
                    .HasMaxLength(35)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Volunteer>(entity =>
            {
                entity.ToTable("volunteer");

                entity.HasIndex(e => new { e.FirstName, e.LastName, e.Dob })
                    .HasName("UQ__voluntee__5846F7A1E1DF965B")
                    .IsUnique();

                entity.Property(e => e.VolunteerId).HasColumnName("volunteer_id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AlternativePhone)
                    .HasColumnName("alternative_phone")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.BranchId).HasColumnName("branch_id");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.PostCode)
                    .HasColumnName("post_code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PreferredPhone)
                    .IsRequired()
                    .HasColumnName("preferred_phone")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.TownCity)
                    .IsRequired()
                    .HasColumnName("town_city")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.Volunteer)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("a volunteer belones to a branch");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Volunteer)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("a volunteer has a role");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Volunteer)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("a volunteer has a status");
            });

            modelBuilder.Entity<VolunteerEmergencyContact>(entity =>
            {
                entity.HasKey(e => e.VolunteerId)
                    .HasName("PK__voluntee__0FE766B1B7894FD0");

                entity.ToTable("volunteer_emergency_contact");

                entity.Property(e => e.VolunteerId)
                    .HasColumnName("volunteer_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnName("phone_number")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Relationship)
                    .IsRequired()
                    .HasColumnName("relationship")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Volunteer)
                    .WithOne(p => p.VolunteerEmergencyContact)
                    .HasForeignKey<VolunteerEmergencyContact>(d => d.VolunteerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("a volunteer has a emergency contact");
            });

            modelBuilder.Entity<VolunteerPoliceInfo>(entity =>
            {
                entity.HasKey(e => e.VolunteerId)
                    .HasName("PK__voluntee__0FE766B1C228BCB1");

                entity.ToTable("volunteer_police_info");

                entity.Property(e => e.VolunteerId)
                    .HasColumnName("volunteer_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.PoliceVetDate)
                    .HasColumnName("police_vet_date")
                    .HasColumnType("date");

                entity.Property(e => e.PoliceVetVerified).HasColumnName("police_vet_verified");

                entity.HasOne(d => d.Volunteer)
                    .WithOne(p => p.VolunteerPoliceInfo)
                    .HasForeignKey<VolunteerPoliceInfo>(d => d.VolunteerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("a volunteer has a police info");
            });

            modelBuilder.Entity<VolunteerRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__voluntee__760965CCABE2EDB2");

                entity.ToTable("volunteer_role");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasColumnName("role_name")
                    .HasMaxLength(35)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VolunteerStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK__voluntee__3683B531D609B864");

                entity.ToTable("volunteer_status");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VolunteerTrainingInfo>(entity =>
            {
                entity.HasKey(e => e.VolunteerId)
                    .HasName("PK__voluntee__0FE766B1CF0C3AF3");

                entity.ToTable("volunteer_training_info");

                entity.Property(e => e.VolunteerId)
                    .HasColumnName("volunteer_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DeliveryTraining).HasColumnName("delivery_training");

                entity.Property(e => e.FirstAidRaining).HasColumnName("first_aid_raining");

                entity.Property(e => e.HSTraining).HasColumnName("H&S_training");

                entity.Property(e => e.OtherTrainingSkill)
                    .HasColumnName("other_training_skill")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Volunteer)
                    .WithOne(p => p.VolunteerTrainingInfo)
                    .HasForeignKey<VolunteerTrainingInfo>(d => d.VolunteerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("a voluteer has a train info");
            });
        }

       
        /// <summary>
        /// 获取Volunteer Selections， a=-1：获取已分配Account；
        /// a=Null: 获取全部；
        /// a= VolunteerId: 获取已分配+该Volunteer
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public async Task<List<VolunteerForSelection>> GetVolunteerForSelection(int? a)
        {
            IQueryable<Volunteer> sss;
            if (a == -1)
            {
                 sss = Volunteer.Where(x => x.IsAssignedUserAccount != true);
            }else if (a == null)
            {
                sss = Volunteer;
            }
            else
            {
                //返回 没有分配Account 的志愿者，或者 Id 为 a的志愿者
                sss = Volunteer.Where(x => x.IsAssignedUserAccount != true || x.VolunteerId == a);
            }            
            return await sss.Select(volunteer => new VolunteerForSelection
                 {VId = volunteer.VolunteerId, IdFullName = volunteer.VolunteerId +
                  ". " + volunteer.FirstName + "   " + volunteer.LastName }).OrderBy(c => c.VId).ToListAsync();
            
        }

        public async Task<List<RecipientForSelection>> GetRecipientForSelection()
        {
            return await Recipient.Select(recipient => new RecipientForSelection
            {
                RId = recipient.RecipientId,
                IdFullName = recipient.RecipientId +
                             ". " + recipient.FirstName + "   " + recipient.LastName
            }).OrderBy(c => c.RId).ToListAsync();
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Telemedicine.Models.Models
{
    public partial class TelemedicineAppContext : DbContext
    {
        public TelemedicineAppContext()
        {
        }

        public TelemedicineAppContext(DbContextOptions<TelemedicineAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointments> Appointments { get; set; }
        public virtual DbSet<Consultations> Consultations { get; set; }
        public virtual DbSet<Conversations> Conversations { get; set; }
        public virtual DbSet<DoctorAwards> DoctorAwards { get; set; }
        public virtual DbSet<DoctorEducations> DoctorEducations { get; set; }
        public virtual DbSet<DoctorExperiences> DoctorExperiences { get; set; }
        public virtual DbSet<DoctorProfiles> DoctorProfiles { get; set; }
        public virtual DbSet<Feedbacks> Feedbacks { get; set; }
        public virtual DbSet<Invoices> Invoices { get; set; }
        public virtual DbSet<Logins> Logins { get; set; }
        public virtual DbSet<Lookups> Lookups { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<PatientAttachments> PatientAttachments { get; set; }
        public virtual DbSet<SOAPNotes> SOAPNotes { get; set; }
        public virtual DbSet<SpecialtyLookups> SpecialtyLookups { get; set; }
        public virtual DbSet<States> States { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<UserStates> UserStates { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=otherappssql.database.windows.net;Database=IClinicNow;User Id=otherapp-admin; Password=MjhyNaiPta_123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointments>(entity =>
            {
                entity.HasKey(e => e.AppointmentId)
                    .HasName("PK_UserAppointments");

                entity.Property(e => e.CreatedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.StartDate).HasColumnType("smalldatetime");

                entity.HasOne(d => d.AppointmentType)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.AppointmentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.AppointmentsCreatedBy)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ModifiedBy)
                    .WithMany(p => p.AppointmentsModifiedBy)
                    .HasForeignKey(d => d.ModifiedById);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AppointmentsUser)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Consultations>(entity =>
            {
                entity.HasKey(e => e.ConsultationId)
                    .HasName("PK_UserConsultations");

                entity.Property(e => e.SessionId)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Appointment)
                    .WithMany(p => p.Consultations)
                    .HasForeignKey(d => d.AppointmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Conversations>(entity =>
            {
                entity.HasKey(e => e.ConversationId);

                entity.Property(e => e.CreatedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.ConversationsFromUser)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Conversations_Conversations");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ConversationsToUser)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DoctorAwards>(entity =>
            {
                entity.HasKey(e => e.DoctorAwardId);

                entity.Property(e => e.AwardDate).HasColumnType("smalldatetime");

                entity.Property(e => e.CreatedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.Institution).HasMaxLength(100);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.DoctorAwardsCreatedBy)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ModifiedBy)
                    .WithMany(p => p.DoctorAwardsModifiedBy)
                    .HasForeignKey(d => d.ModifiedById);
            });

            modelBuilder.Entity<DoctorEducations>(entity =>
            {
                entity.HasKey(e => e.DoctorEducationId);

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.CreatedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.StartDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.DoctorEducationsCreatedBy)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DoctorEducationType)
                    .WithMany(p => p.DoctorEducations)
                    .HasForeignKey(d => d.DoctorEducationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ModifiedBy)
                    .WithMany(p => p.DoctorEducationsModifiedBy)
                    .HasForeignKey(d => d.ModifiedById);
            });

            modelBuilder.Entity<DoctorExperiences>(entity =>
            {
                entity.HasKey(e => e.DoctorExperienceId);

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.CreatedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.Organization)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.StartDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.DoctorExperiencesCreatedBy)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ModifiedBy)
                    .WithMany(p => p.DoctorExperiencesModifiedBy)
                    .HasForeignKey(d => d.ModifiedById);
            });

            modelBuilder.Entity<DoctorProfiles>(entity =>
            {
                entity.HasKey(e => e.DoctorProfileId);

                entity.Property(e => e.LicensedUrl).HasMaxLength(255);

                entity.HasOne(d => d.SpecialtyLookup)
                    .WithMany(p => p.DoctorProfiles)
                    .HasForeignKey(d => d.SpecialtyLookupId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DoctorProfiles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Feedbacks>(entity =>
            {
                entity.HasKey(e => e.FeedbackId)
                    .HasName("PK_UserFeedbacks");

                entity.Property(e => e.CreatedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.FeedbacksFromUser)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.FeedbacksToUser)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Invoices>(entity =>
            {
                entity.HasKey(e => e.InvoiceId);

                entity.Property(e => e.AmountPaid).HasColumnType("money");

                entity.Property(e => e.ReferenceNumber)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Appointment)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.AppointmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Logins>(entity =>
            {
                entity.HasKey(e => e.LoginId);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Logins)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Lookups>(entity =>
            {
                entity.HasKey(e => e.LookupId);

                entity.Property(e => e.LongTitle)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LookupType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ShortTitle)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.HasKey(e => e.NotificationId);

                entity.Property(e => e.CreatedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.NotificationsFromUser)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.NotificationsToUser)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<PatientAttachments>(entity =>
            {
                entity.HasKey(e => e.PatientAttachmentId)
                    .HasName("PK_UserAttachments");

                entity.Property(e => e.AttachmentDate).HasColumnType("smalldatetime");

                entity.Property(e => e.CreatedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.PatientAttachmentsCreatedBy)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ModifiedBy)
                    .WithMany(p => p.PatientAttachmentsModifiedBy)
                    .HasForeignKey(d => d.ModifiedById);
            });

            modelBuilder.Entity<SOAPNotes>(entity =>
            {
                entity.HasKey(e => e.SOAPNoteId)
                    .HasName("PK_UserNotes");

                entity.HasOne(d => d.Appointment)
                    .WithMany(p => p.SOAPNotes)
                    .HasForeignKey(d => d.AppointmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<SpecialtyLookups>(entity =>
            {
                entity.HasKey(e => e.SpecialtyLookupId)
                    .HasName("PK_Specialties");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<States>(entity =>
            {
                entity.HasKey(e => e.StateId);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.HasKey(e => e.UserRoleId);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<UserStates>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.StateId });

                entity.HasOne(d => d.State)
                    .WithMany(p => p.UserStates)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserStates)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.CreatedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("smalldatetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Latitude).HasMaxLength(20);

                entity.Property(e => e.Longitude).HasMaxLength(20);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.ModifiedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PostalCode).HasMaxLength(20);

                entity.Property(e => e.PrimaryAddress).HasMaxLength(255);

                entity.Property(e => e.ProfilePictureUrl).HasMaxLength(255);

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.UsersGender)
                    .HasForeignKey(d => d.GenderId);

                entity.HasOne(d => d.PreferredLanguageType)
                    .WithMany(p => p.UsersPreferredLanguageType)
                    .HasForeignKey(d => d.PreferredLanguageTypeId);

                entity.HasOne(d => d.SalutationType)
                    .WithMany(p => p.UsersSalutationType)
                    .HasForeignKey(d => d.SalutationTypeId);

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

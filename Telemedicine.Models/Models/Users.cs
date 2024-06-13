using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class Users
    {
        public Users()
        {
            AppointmentsCreatedBy = new HashSet<Appointments>();
            AppointmentsModifiedBy = new HashSet<Appointments>();
            AppointmentsUser = new HashSet<Appointments>();
            ConversationsFromUser = new HashSet<Conversations>();
            ConversationsToUser = new HashSet<Conversations>();
            DoctorAwardsCreatedBy = new HashSet<DoctorAwards>();
            DoctorAwardsModifiedBy = new HashSet<DoctorAwards>();
            DoctorEducationsCreatedBy = new HashSet<DoctorEducations>();
            DoctorEducationsModifiedBy = new HashSet<DoctorEducations>();
            DoctorExperiencesCreatedBy = new HashSet<DoctorExperiences>();
            DoctorExperiencesModifiedBy = new HashSet<DoctorExperiences>();
            DoctorProfiles = new HashSet<DoctorProfiles>();
            FeedbacksFromUser = new HashSet<Feedbacks>();
            FeedbacksToUser = new HashSet<Feedbacks>();
            Logins = new HashSet<Logins>();
            NotificationsFromUser = new HashSet<Notifications>();
            NotificationsToUser = new HashSet<Notifications>();
            PatientAttachmentsCreatedBy = new HashSet<PatientAttachments>();
            PatientAttachmentsModifiedBy = new HashSet<PatientAttachments>();
            UserStates = new HashSet<UserStates>();
        }

        public long UserId { get; set; }
        public int UserRoleId { get; set; }
        public int? SalutationTypeId { get; set; }
        public int? GenderId { get; set; }
        public int? PreferredLanguageTypeId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Phone { get; set; }
        public string PrimaryAddress { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PostalCode { get; set; }
        public string Bio { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        public virtual Lookups Gender { get; set; }
        public virtual Lookups PreferredLanguageType { get; set; }
        public virtual Lookups SalutationType { get; set; }
        public virtual UserRoles UserRole { get; set; }
        public virtual ICollection<Appointments> AppointmentsCreatedBy { get; set; }
        public virtual ICollection<Appointments> AppointmentsModifiedBy { get; set; }
        public virtual ICollection<Appointments> AppointmentsUser { get; set; }
        public virtual ICollection<Conversations> ConversationsFromUser { get; set; }
        public virtual ICollection<Conversations> ConversationsToUser { get; set; }
        public virtual ICollection<DoctorAwards> DoctorAwardsCreatedBy { get; set; }
        public virtual ICollection<DoctorAwards> DoctorAwardsModifiedBy { get; set; }
        public virtual ICollection<DoctorEducations> DoctorEducationsCreatedBy { get; set; }
        public virtual ICollection<DoctorEducations> DoctorEducationsModifiedBy { get; set; }
        public virtual ICollection<DoctorExperiences> DoctorExperiencesCreatedBy { get; set; }
        public virtual ICollection<DoctorExperiences> DoctorExperiencesModifiedBy { get; set; }
        public virtual ICollection<DoctorProfiles> DoctorProfiles { get; set; }
        public virtual ICollection<Feedbacks> FeedbacksFromUser { get; set; }
        public virtual ICollection<Feedbacks> FeedbacksToUser { get; set; }
        public virtual ICollection<Logins> Logins { get; set; }
        public virtual ICollection<Notifications> NotificationsFromUser { get; set; }
        public virtual ICollection<Notifications> NotificationsToUser { get; set; }
        public virtual ICollection<PatientAttachments> PatientAttachmentsCreatedBy { get; set; }
        public virtual ICollection<PatientAttachments> PatientAttachmentsModifiedBy { get; set; }
        public virtual ICollection<UserStates> UserStates { get; set; }
    }
}

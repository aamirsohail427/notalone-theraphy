using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.StoreProcedureDto
{
    public class GetDoctorDetailDto
    {
        public int UserId { get; set; }
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
        public string SalutationType { get; set; }
        public string GenderType { get; set; }
        public string PreferredLanguageType { get; set; }
        public List<Doctorawardslist> DoctorAwardsList { get; set; }
        public List<Doctoreducationslist> DoctorEducationsList { get; set; }
        public List<Doctorexperienceslist> DoctorExperiencesList { get; set; }
        public List<Feedbackslist> FeedbacksList { get; set; }
        public List<Appointmentslist> AppointmentsList { get; set; }
    }
    public class Doctorawardslist
    {
        public int DoctorAwardId { get; set; }
        public string Title { get; set; }
        public string Institution { get; set; }
        public DateTime AwardDate { get; set; }
    }

    public class Doctoreducationslist
    {
        public int DoctorEducationId { get; set; }
        public string DoctorEducationType { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class Doctorexperienceslist
    {
        public long DoctorExperienceId { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Organization { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class Feedbackslist
    {
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public FromUser FromUser { get; set; }
        public ToUser ToUser { get; set; }
    }

    public class Appointmentslist
    {
        public int AppointmentId { get; set; }
        public string AppointmentType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class FromUser
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public object MiddleName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }

    }

    public class ToUser
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }

    }
}

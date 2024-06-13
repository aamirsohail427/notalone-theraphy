using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.StoreProcedureDto
{
    public class GetDoctorsByFiltersDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<Doctor> Doctors { get; set; }
    }

    public class Doctor
    {
        public int UserId { get; set; }
        public string SalutationType { get; set; }
        public string GenderType { get; set; }
        public string PreferredLanguageType { get; set; }
        public string DoctorName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Phone { get; set; }
        public string PrimaryAddress { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PostalCode { get; set; }
        public string Bio { get; set; }
        public decimal? Rating { get; set; }
        public string Distance { get; set; }
        public string Specialty { get; set; }
        public List<Appointmentslot> AppointmentSlots { get; set; }
    }

    public class Appointmentslot
    {
        public int AppointmentId { get; set; }
        public string AppointmentType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

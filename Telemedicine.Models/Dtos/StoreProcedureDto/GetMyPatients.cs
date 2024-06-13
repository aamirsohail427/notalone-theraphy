using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.StoreProcedureDto
{
    public class GetMyPatients
    {
        public int UserId { get; set; }
        public string SalutationType { get; set; }
        public string GenderType { get; set; }
        public string PreferredLanguageType { get; set; }
        public string PatientName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Phone { get; set; }
        public string PrimaryAddress { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PostalCode { get; set; }
        public string Bio { get; set; }
        public string Distance { get; set; }
    }
}

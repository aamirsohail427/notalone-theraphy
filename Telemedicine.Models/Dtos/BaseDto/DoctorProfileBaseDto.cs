using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.BaseDto
{
    public class DoctorProfileBaseDto
    {
        public long DoctorProfileId { get; set; }
        public long UserId { get; set; }
        public string LicensedUrl { get; set; }
        public int SpecialtyLookupId { get; set; }
    }
}

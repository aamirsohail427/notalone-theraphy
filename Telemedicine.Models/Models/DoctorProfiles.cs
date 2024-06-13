using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class DoctorProfiles
    {
        public long DoctorProfileId { get; set; }
        public long UserId { get; set; }
        public string LicensedUrl { get; set; }
        public int SpecialtyLookupId { get; set; }

        public virtual SpecialtyLookups SpecialtyLookup { get; set; }
        public virtual Users User { get; set; }
    }
}

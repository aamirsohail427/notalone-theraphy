using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class SpecialtyLookups
    {
        public SpecialtyLookups()
        {
            DoctorProfiles = new HashSet<DoctorProfiles>();
        }

        public int SpecialtyLookupId { get; set; }
        public string Title { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<DoctorProfiles> DoctorProfiles { get; set; }
    }
}

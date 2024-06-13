using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class Lookups
    {
        public Lookups()
        {
            Appointments = new HashSet<Appointments>();
            DoctorEducations = new HashSet<DoctorEducations>();
            UsersGender = new HashSet<Users>();
            UsersPreferredLanguageType = new HashSet<Users>();
            UsersSalutationType = new HashSet<Users>();
        }

        public int LookupId { get; set; }
        public string ShortTitle { get; set; }
        public string LongTitle { get; set; }
        public string LookupType { get; set; }

        public virtual ICollection<Appointments> Appointments { get; set; }
        public virtual ICollection<DoctorEducations> DoctorEducations { get; set; }
        public virtual ICollection<Users> UsersGender { get; set; }
        public virtual ICollection<Users> UsersPreferredLanguageType { get; set; }
        public virtual ICollection<Users> UsersSalutationType { get; set; }
    }
}

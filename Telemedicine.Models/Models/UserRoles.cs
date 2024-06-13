using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class UserRoles
    {
        public UserRoles()
        {
            Users = new HashSet<Users>();
        }

        public int UserRoleId { get; set; }
        public string Title { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}

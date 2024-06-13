using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class States
    {
        public States()
        {
            UserStates = new HashSet<UserStates>();
        }

        public int StateId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }

        public virtual ICollection<UserStates> UserStates { get; set; }
    }
}

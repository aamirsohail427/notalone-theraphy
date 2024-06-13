using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class Logins
    {
        public long LoginId { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsLoginAllowed { get; set; }

        public virtual Users User { get; set; }
    }
}

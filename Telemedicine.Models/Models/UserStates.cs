using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class UserStates
    {
        public long UserId { get; set; }
        public int StateId { get; set; }

        public virtual States State { get; set; }
        public virtual Users User { get; set; }
    }
}

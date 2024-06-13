using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class Notifications
    {
        public long NotificationId { get; set; }
        public int NotificationTypeId { get; set; }
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public virtual Users FromUser { get; set; }
        public virtual Users ToUser { get; set; }
    }
}

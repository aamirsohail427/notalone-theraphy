using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class Feedbacks
    {
        public long FeedbackId { get; set; }
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        public virtual Users FromUser { get; set; }
        public virtual Users ToUser { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.BaseDto
{
    public class FeedbackBaseDto
    {
        public long FeedbackId { get; set; }
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

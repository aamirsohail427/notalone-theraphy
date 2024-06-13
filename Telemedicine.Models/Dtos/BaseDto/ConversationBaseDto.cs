using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.BaseDto
{
    public class ConversationBaseDto
    {
        public long ConversationId { get; set; }
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public string Message { get; set; } = null;
        public bool IsRead { get; set; } = true;
        public bool? IsActive { get; set; } = true;
        public long? ParentId { get; set; } = null;
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    }
}

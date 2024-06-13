using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class Conversations
    {
        public Conversations()
        {
            InverseParent = new HashSet<Conversations>();
        }

        public long ConversationId { get; set; }
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public bool? IsActive { get; set; }
        public long? ParentId { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public virtual Users FromUser { get; set; }
        public virtual Conversations Parent { get; set; }
        public virtual Users ToUser { get; set; }
        public virtual ICollection<Conversations> InverseParent { get; set; }
    }
}

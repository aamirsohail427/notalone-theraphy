using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class PatientAttachments
    {
        public long PatientAttachmentId { get; set; }
        public string Title { get; set; }
        public DateTime AttachmentDate { get; set; }
        public string Url { get; set; }
        public string Notes { get; set; }
        public bool? IsActive { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        public virtual Users CreatedBy { get; set; }
        public virtual Users ModifiedBy { get; set; }
    }
}

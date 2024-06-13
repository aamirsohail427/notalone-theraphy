using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.BaseDto
{
    public class PatientAttachmentBaseDto
    {
        public long PatientAttachmentId { get; set; }
        public string Title { get; set; }
        public DateTime AttachmentDate { get; set; }
        public string Url { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

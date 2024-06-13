using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class DoctorAwards
    {
        public long DoctorAwardId { get; set; }
        public string Title { get; set; }
        public string Institution { get; set; }
        public DateTime AwardDate { get; set; }
        public bool? IsActive { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        public virtual Users CreatedBy { get; set; }
        public virtual Users ModifiedBy { get; set; }
    }
}

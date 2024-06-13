using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class DoctorEducations
    {
        public long DoctorEducationId { get; set; }
        public int DoctorEducationTypeId { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        public virtual Users CreatedBy { get; set; }
        public virtual Lookups DoctorEducationType { get; set; }
        public virtual Users ModifiedBy { get; set; }
    }
}

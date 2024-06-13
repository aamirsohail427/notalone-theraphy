﻿using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class DoctorExperiences
    {
        public long DoctorExperienceId { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Organization { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        public virtual Users CreatedBy { get; set; }
        public virtual Users ModifiedBy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.BaseDto
{
    public class DoctorExperienceBaseDto
    {
        public long DoctorExperienceId { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Organization { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

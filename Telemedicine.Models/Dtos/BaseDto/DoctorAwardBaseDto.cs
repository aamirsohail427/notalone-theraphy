using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.BaseDto
{
    public class DoctorAwardBaseDto
    {
        public long DoctorAwardId { get; set; }
        public string Title { get; set; }
        public string Institution { get; set; }
        public DateTime AwardDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

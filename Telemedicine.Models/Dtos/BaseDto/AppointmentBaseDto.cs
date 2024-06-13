using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.BaseDto
{
    public class AppointmentBaseDto
    {
        public long AppointmentId { get; set; }
        public int AppointmentTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long? UserId { get; set; }
        public bool IsActive { get; set; } = true;
        public long CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}

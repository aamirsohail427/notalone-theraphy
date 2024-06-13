using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.StoreProcedureDto
{
    public class CheckUpcomingAppointmentsDto
    {
        public int AppointmentId { get; set; }
        public int AppointmentTypeId { get; set; }
        public string AppointmentType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}

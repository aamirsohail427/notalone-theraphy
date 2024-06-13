using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.StoreProcedureDto
{
    public class PatientDashboardStatsDto
    {
        public int TodayAppointments { get; set; }
        public int TotalAppointments { get; set; }
        public int TotalFeedbacks { get; set; }
        public decimal TotalPaid { get; set; }
    }
}

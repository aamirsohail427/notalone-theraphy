using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.StoreProcedureDto
{
    public class DoctorDashboardStatsDto
    {
        public int TodayAppointments { get; set; }
        public int TotalPatients { get; set; }
        public int TotalFeedbacks { get; set; }
        public decimal TotalEarns { get; set; }
    }
}

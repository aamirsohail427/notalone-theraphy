using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.StoreProcedureDto
{
    public class AdminDashboardStatsDto
    {
        public int TotalDoctors { get; set; }
        public int TotalPatients { get; set; }
        public int TotalNegativeFeedbacks { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}

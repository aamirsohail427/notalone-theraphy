using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos.StoreProcedureDto;

namespace Telemedicine.App.Models
{
    public class DashboardViewModel
    {
        public AdminDashboardStatsDto AdminDashboardStatsDto { get; set; }
        public DoctorDashboardStatsDto DoctorDashboardStatsDto { get; set; }
        public PatientDashboardStatsDto PatientDashboardStatsDto { get; set; }
    }
}

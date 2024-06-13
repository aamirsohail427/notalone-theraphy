using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.RequestDto
{
    public class GetDoctorsByFiltersRequestDto
    {
        public long UserId { get; set; }
        public string DoctorName { get; set; }
        public int? SpecialtyLookupId { get; set; }
        public string Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int PageNumber { get; set; } = 1;
    }
}

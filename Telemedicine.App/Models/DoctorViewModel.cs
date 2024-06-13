using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos.RequestDto;

namespace Telemedicine.App.Models
{
    public class DoctorViewModel
    {
        public DoctorViewModel()
        {
            SpecialtyLookups = new List<SelectListItem>();
        }
        public GetDoctorsByFiltersRequestDto GetDoctorsByFiltersRequestDto { get; set; }
        public List<SelectListItem> SpecialtyLookups { get; set; }
    }
}

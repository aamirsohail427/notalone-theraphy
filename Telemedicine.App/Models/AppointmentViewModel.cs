using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos.RequestDto;

namespace Telemedicine.App.Models
{
    public class AppointmentViewModel
    {
        public AppointmentViewModel()
        {
            AppointmentTypes = new List<SelectListItem>();
            //CreateSlotRequestDto = new CreateSlotRequestDto();
        }

        public List<SelectListItem> AppointmentTypes { get; set; }
        public AppointmentRequestDto AppointmentRequestDto { get; set; }
        public CreateSlotRequestDto CreateSlotRequestDto { get; set; }
    }
}

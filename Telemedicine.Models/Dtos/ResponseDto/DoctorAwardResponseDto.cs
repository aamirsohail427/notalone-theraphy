using System;
using System.Collections.Generic;
using System.Text;
using Telemedicine.Models.Dtos.BaseDto;

namespace Telemedicine.Models.Dtos.ResponseDto
{
    public class DoctorAwardResponseDto : DoctorAwardBaseDto
    {
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}

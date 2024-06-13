using System;
using System.Collections.Generic;
using System.Text;
using Telemedicine.Models.Dtos.BaseDto;

namespace Telemedicine.Models.Dtos.ResponseDto
{
    public class DoctorEducationResponseDto : DoctorEducationBaseDto
    {
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        public LookupResponseDto DoctorEducationType { get; set; }
    }
}

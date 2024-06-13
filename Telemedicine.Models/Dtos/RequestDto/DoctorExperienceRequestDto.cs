﻿using System;
using System.Collections.Generic;
using System.Text;
using Telemedicine.Models.Dtos.BaseDto;

namespace Telemedicine.Models.Dtos.RequestDto
{
    public class DoctorExperienceRequestDto : DoctorExperienceBaseDto
    {
        public long CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
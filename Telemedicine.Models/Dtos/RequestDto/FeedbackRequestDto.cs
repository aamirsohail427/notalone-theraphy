﻿using System;
using System.Collections.Generic;
using System.Text;
using Telemedicine.Models.Dtos.BaseDto;

namespace Telemedicine.Models.Dtos.RequestDto
{
    public class FeedbackRequestDto : FeedbackBaseDto
    {
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    }
}
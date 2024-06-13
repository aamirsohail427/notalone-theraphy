﻿using System;
using System.Collections.Generic;
using System.Text;
using Telemedicine.Models.Dtos.BaseDto;

namespace Telemedicine.Models.Dtos.ResponseDto
{
    public class NotificationResponseDto : NotificationBaseDto
    {
        public UserResponseDto FromUser { get; set; }
        public UserResponseDto ToUser { get; set; }
    }
}
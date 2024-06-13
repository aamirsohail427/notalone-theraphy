using System;
using System.Collections.Generic;
using System.Text;
using Telemedicine.Models.Dtos.BaseDto;

namespace Telemedicine.Models.Dtos.ResponseDto
{
    public class LoginResponseDto : LoginBaseDto
    {
        public long LoginId { get; set; }
        public long UserId { get; set; }
        public bool IsLoginAllowed { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Telemedicine.Models.Dtos.BaseDto;

namespace Telemedicine.Models.Dtos.ResponseDto
{
    public class UserResponseDto : UserBaseDto
    {
        public List<LoginResponseDto> Logins { get; set; }
    }
}

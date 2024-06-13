using System;
using System.Collections.Generic;
using System.Text;
using Telemedicine.Models.Dtos.BaseDto;

namespace Telemedicine.Models.Dtos.ResponseDto
{
    public class ConversationResponseDto : ConversationBaseDto
    {
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }

    }
}

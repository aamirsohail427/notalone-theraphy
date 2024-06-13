using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;

namespace Telemedicine.App.Models
{
    public class FeedbackViewModel
    {
        public FeedbackRequestDto FeedbackRequestDto { get; set; }
        public UserResponseDto UserResponseDto { get; set; }
    }
}

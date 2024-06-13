using System;
using System.Collections.Generic;
using System.Text;
using Telemedicine.Models.Dtos.BaseDto;

namespace Telemedicine.Models.Dtos.ResponseDto
{
    public class AppointmentResponseDto : AppointmentBaseDto
    {
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        public UserResponseDto CreatedBy { get; set; }
        public UserResponseDto User { get; set; }
        public List<ConsultationResponseDto> Consultations { get; set; }
        public List<SOAPNoteResponseDto> SOAPNotes { get; set; }
    }
}

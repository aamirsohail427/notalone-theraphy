using System;
using System.Collections.Generic;
using System.Text;
using Telemedicine.Models.Dtos.BaseDto;

namespace Telemedicine.Models.Dtos.ResponseDto
{
    public class PatientProfileDetailResponseDto : UserBaseDto
    {
        public LookupResponseDto Gender { get; set; }
        public LookupResponseDto PreferredLanguageType { get; set; }
        public LookupResponseDto SalutationType { get; set; }
        public List<PatientAttachmentResponseDto> PatientAttachmentsCreatedBy { get; set; }
        public List<SOAPNoteResponseDto> SOAPNotes { get; set; }
        public List<AppointmentResponseDto> Appointments { get; set; }
    }
}

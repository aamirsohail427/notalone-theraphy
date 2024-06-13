using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;
using Telemedicine.Utilities;

namespace Telemedicine.App.Models
{
    public class ConsultationViewModel
    {
        public SOAPNoteRequestDto SOAPNoteRequestDto { get; set; }
        public long AppointmentId { get; set; }
        public int ApiKey { get; set; } = TokboxConfigurations.API_KEY;
        public string SessionId { get; set; }
        public string Token { get; set; }
        public long PatientId { get; set; }
        public long DoctorId { get; set; }
        public PatientProfileDetailResponseDto PatientProfileDetail { get; set; }
    }
}

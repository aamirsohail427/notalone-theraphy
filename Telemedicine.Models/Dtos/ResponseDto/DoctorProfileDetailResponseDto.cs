using System;
using System.Collections.Generic;
using System.Text;
using Telemedicine.Models.Dtos.BaseDto;

namespace Telemedicine.Models.Dtos.ResponseDto
{
    public class DoctorProfileDetailResponseDto : UserBaseDto
    {
        public LookupResponseDto Gender { get; set; }
        public LookupResponseDto PreferredLanguageType { get; set; }
        public LookupResponseDto SalutationType { get; set; }
        public List<DoctorAwardResponseDto> DoctorAwardsCreatedBy { get; set; }
        public List<DoctorEducationResponseDto> DoctorEducationsCreatedBy { get; set; }
        public List<DoctorExperienceResponseDto> DoctorExperiencesCreatedBy { get; set; }
        public List<FeedbackResponseDto> FeedbacksToUser { get; set; }
        public List<AppointmentResponseDto> AppointmentsCreatedBy { get; set; }
    }
}

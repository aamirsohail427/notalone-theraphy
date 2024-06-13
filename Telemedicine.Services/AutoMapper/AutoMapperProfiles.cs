using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;
using Telemedicine.Models.Models;

namespace Telemedicine.Services.AutoMapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Lookups, LookupRequestDto>().ReverseMap();
            CreateMap<Lookups, LookupResponseDto>().ReverseMap();

            CreateMap<Logins, LoginRequestDto>().ReverseMap();
            CreateMap<Logins, LoginResponseDto>().ReverseMap();

            CreateMap<Users, RegisterRequestDto>().ReverseMap();
            CreateMap<Users, UserRequestDto>().ReverseMap();
            CreateMap<Users, UserResponseDto>().ReverseMap();
            CreateMap<Users, DoctorProfileDetailResponseDto>().ReverseMap();
            CreateMap<Users, PatientProfileDetailResponseDto>().ReverseMap();

            CreateMap<UserRoles, UserRoleRequestDto>().ReverseMap();
            CreateMap<UserRoles, UserRoleResponseDto>().ReverseMap();

            CreateMap<SpecialtyLookups, SpecialtyLookupRequestDto>().ReverseMap();
            CreateMap<SpecialtyLookups, SpecialtyLookupResponseDto>().ReverseMap();

            CreateMap<DoctorProfiles, DoctorProfileRequestDto>().ReverseMap();
            CreateMap<DoctorProfiles, DoctorProfileResponseDto>().ReverseMap();

            CreateMap<DoctorAwards, DoctorAwardRequestDto>().ReverseMap();
            CreateMap<DoctorAwards, DoctorAwardResponseDto>().ReverseMap();

            CreateMap<DoctorEducations, DoctorEducationRequestDto>().ReverseMap();
            CreateMap<DoctorEducations, DoctorEducationResponseDto>().ReverseMap();

            CreateMap<DoctorExperiences, DoctorExperienceRequestDto>().ReverseMap();
            CreateMap<DoctorExperiences, DoctorExperienceResponseDto>().ReverseMap();

            CreateMap<PatientAttachments, PatientAttachmentRequestDto>().ReverseMap();
            CreateMap<PatientAttachments, PatientAttachmentResponseDto>().ReverseMap();

            CreateMap<SOAPNotes, SOAPNoteRequestDto>().ReverseMap();
            CreateMap<SOAPNotes, SOAPNoteResponseDto>().ReverseMap();

            CreateMap<Appointments, AppointmentRequestDto>().ReverseMap();
            CreateMap<Appointments, AppointmentResponseDto>().ReverseMap();

            CreateMap<Feedbacks, FeedbackRequestDto>().ReverseMap();
            CreateMap<Feedbacks, FeedbackResponseDto>().ReverseMap();

            CreateMap<Invoices, InvoiceRequestDto>().ReverseMap();
            CreateMap<Invoices, InvoiceResponseDto>().ReverseMap();

            CreateMap<Consultations, ConsultationRequestDto>().ReverseMap();
            CreateMap<Consultations, ConsultationResponseDto>().ReverseMap();

            CreateMap<Notifications, NotificationRequestDto>().ReverseMap();
            CreateMap<Notifications, NotificationResponseDto>().ReverseMap();

            CreateMap<Conversations, ConversationRequestDto>().ReverseMap();
            CreateMap<Conversations, ConversationResponseDto>().ReverseMap();

            CreateMap<States, StateRequestDto>().ReverseMap();
            CreateMap<States, StateResponseDto>().ReverseMap();

            CreateMap<UserStates, UserStateRequestDto>().ReverseMap();
            CreateMap<UserStates, UserStateResponseDto>().ReverseMap();
        }
    }
}

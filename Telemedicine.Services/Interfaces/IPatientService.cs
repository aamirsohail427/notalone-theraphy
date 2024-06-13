using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.ResponseDto;
using Telemedicine.Models.Dtos.StoreProcedureDto;

namespace Telemedicine.Services.Interfaces
{
    public interface IPatientService
    {
        Task<ResponseDto<List<GetMyPatients>>> GetByDoctorAsync(RequestDto<long> requestDto);
        Task<ResponseDto<PatientProfileDetailResponseDto>> GetPatientProfileDetailByUserAsync(RequestDto<long> requestDto);
        Task<ResponseDto<List<SOAPNoteResponseDto>>> GetSOAPNotesByUserAsync(RequestDto<long> requestDto);
        Task<ResponseDto<List<AppointmentResponseDto>>> GetAppointmentsWithSOAPNotesByUserAsync(RequestDto<long> requestDto);
    }
}

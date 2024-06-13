using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;
using Telemedicine.Models.Dtos.StoreProcedureDto;

namespace Telemedicine.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<ResponseDto<AppointmentResponseDto>> SaveAsync(long userId, AppointmentRequestDto requestDto);
        Task<ResponseDto<bool>> DeleteByIdAsync(RequestDto<long> requestDto);
        Task<ResponseDto<AppointmentResponseDto>> GetByIdAsync(RequestDto<long> requestDto);
        Task<ResponseDto<AppointmentResponseDto>> BookAsync(long userId, RequestDto<long> requestDto);
        Task<ResponseDto<List<AppointmentResponseDto>>> CheckOverlapAppointmentsAsync(CheckOverlapAppointmentRequestDto requestDto);
        Task<ResponseDto<bool>> CreateSlotsAsync(long userId, CreateSlotRequestDto requestDto);
        Task<ResponseDto<List<AppointmentResponseDto>>> GetForDoctorAsync(RequestDto<long> requestDto);
        Task<ResponseDto<List<AppointmentResponseDto>>> GetForPatientAsync(RequestDto<long> requestDto);
        Task<ResponseDto<List<CheckUpcomingAppointmentsDto>>> GetUpcomingsAsync(RequestDto<long> requestDto);
        Task<ResponseDto<AppointmentResponseDto>> CancelAppointmentAsync(RequestDto<long> requestDto);
        Task<ResponseDto<AppointmentResponseDto>> DeleteAppointmentAsync(RequestDto<long> requestDto);
    }
}

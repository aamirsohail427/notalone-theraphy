using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;

namespace Telemedicine.Services.Interfaces
{
    public interface IPatientAttachmentService
    {
        Task<ResponseDto<PatientAttachmentResponseDto>> SaveAsync(long userId, PatientAttachmentRequestDto requestDto);
        Task<ResponseDto<PatientAttachmentResponseDto>> GetByIdAsync(RequestDto<long> requestDto);
        Task<ResponseDto<List<PatientAttachmentResponseDto>>> GetAllByUserAsync(RequestDto<long> requestDto);
        Task<ResponseDto<bool>> DeleteByIdAsync(RequestDto<long> requestDto);
    }
}

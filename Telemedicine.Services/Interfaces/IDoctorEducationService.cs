using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;

namespace Telemedicine.Services.Interfaces
{
    public interface IDoctorEducationService
    {
        Task<ResponseDto<DoctorEducationResponseDto>> SaveAsync(long userId, DoctorEducationRequestDto requestDto);
        Task<ResponseDto<DoctorEducationResponseDto>> GetByIdAsync(RequestDto<long> requestDto);
        Task<ResponseDto<List<DoctorEducationResponseDto>>> GetAllByUserAsync(RequestDto<long> requestDto);
        Task<ResponseDto<bool>> DeleteByIdAsync(RequestDto<long> requestDto);
    }
}

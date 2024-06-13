using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;

namespace Telemedicine.Services.Interfaces
{
    public interface IDoctorExperienceService
    {
        Task<ResponseDto<DoctorExperienceResponseDto>> SaveAsync(long userId, DoctorExperienceRequestDto requestDto);
        Task<ResponseDto<DoctorExperienceResponseDto>> GetByIdAsync(RequestDto<long> requestDto);
        Task<ResponseDto<List<DoctorExperienceResponseDto>>> GetAllByUserAsync(RequestDto<long> requestDto);
        Task<ResponseDto<bool>> DeleteByIdAsync(RequestDto<long> requestDto);
    }
}

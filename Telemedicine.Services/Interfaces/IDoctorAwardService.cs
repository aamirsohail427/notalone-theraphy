using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;

namespace Telemedicine.Services.Interfaces
{
    public interface IDoctorAwardService
    {
        Task<ResponseDto<DoctorAwardResponseDto>> SaveAsync(long userId, DoctorAwardRequestDto requestDto);
        Task<ResponseDto<DoctorAwardResponseDto>> GetByIdAsync(RequestDto<long> requestDto);
        Task<ResponseDto<List<DoctorAwardResponseDto>>> GetAllByUserAsync(RequestDto<long> requestDto);
        Task<ResponseDto<bool>> DeleteByIdAsync(RequestDto<long> requestDto);
    }
}

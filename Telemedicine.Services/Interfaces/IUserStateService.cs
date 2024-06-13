using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;

namespace Telemedicine.Services.Interfaces
{
    public interface IUserStateService
    {
        Task<ResponseDto<List<UserStateResponseDto>>> SaveAsync(long userId, List<UserStateRequestDto> requestDto);
        Task<ResponseDto<List<UserStateResponseDto>>> GetAllByUserAsync(RequestDto<long> requestDto);
    }
}

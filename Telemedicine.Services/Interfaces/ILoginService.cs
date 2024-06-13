using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;

namespace Telemedicine.Services.Interfaces
{
    public interface ILoginService
    {
        Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginRequestDto requestDto);
        Task<ResponseDto<bool>> ResetPasswordAsync(ResetPasswordRequestDto requestDto);
        Task<ResponseDto<bool>> UpdateStatusAsync(RequestDto<long> requestDto);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;

namespace Telemedicine.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ResponseDto<NotificationResponseDto>> SaveAsync(NotificationRequestDto requestDto);
        Task<ResponseDto<List<NotificationResponseDto>>> GetAllAsync(RequestDto<long> requestDto);
        Task<ResponseDto<bool>> ReadAllAsync(RequestDto<long> requestDto);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;

namespace Telemedicine.Services.Interfaces
{
    public interface IConsultationService
    {
        Task<ResponseDto<ConsultationResponseDto>> SaveAsync(ConsultationRequestDto requestDto);
        Task<ResponseDto<ConsultationResponseDto>> GetByIdAsync(RequestDto<long> requestDto);
    }
}

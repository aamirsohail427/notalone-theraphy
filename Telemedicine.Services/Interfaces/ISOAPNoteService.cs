using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;

namespace Telemedicine.Services.Interfaces
{
    public interface ISOAPNoteService
    {
        Task<ResponseDto<SOAPNoteResponseDto>> SaveAsync(long userId, SOAPNoteRequestDto requestDto);
        Task<ResponseDto<SOAPNoteResponseDto>> GetByIdAsync(RequestDto<long> requestDto);
        Task<ResponseDto<List<SOAPNoteResponseDto>>> GetAllByAppointmentAsync(RequestDto<long> requestDto);
        Task<ResponseDto<List<SOAPNoteResponseDto>>> GetAllAsync(RequestDto<long> requestDto);
        Task<ResponseDto<bool>> DeleteByIdAsync(RequestDto<long> requestDto);
    }
}

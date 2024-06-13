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
    public interface IConversationService
    {
        Task<ResponseDto<ConversationResponseDto>> InitiateConversationAsync(ConversationRequestDto requestDto);
        Task<ResponseDto<ConversationResponseDto>> SaveAsync(ConversationRequestDto requestDto);

        Task<ResponseDto<List<ConversationUsersDto>>> GetConversationsUserAsync(RequestDto<long> requestDto);
        Task<ResponseDto<List<ConversationResponseDto>>> GetByIdAsync(RequestDto<long> requestDto);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.ResponseDto;

namespace Telemedicine.Services.Interfaces
{
    public interface IStateService
    {
        Task<ResponseDto<List<StateResponseDto>>> GetAllAsync();
    }
}

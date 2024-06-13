using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;

namespace Telemedicine.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<ResponseDto<InvoiceResponseDto>> SaveAsync(InvoiceRequestDto requestDto);
        Task<ResponseDto<List<InvoiceResponseDto>>> GetForPatientAsync(RequestDto<long> requestDto);
    }
}

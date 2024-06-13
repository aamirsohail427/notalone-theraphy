using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;
using Telemedicine.Models.Models;
using Telemedicine.Services.Interfaces;
using Telemedicine.Utilities;

namespace Telemedicine.Services.Services
{
    public class InvoiceService : IInvoiceService
    {
        private const string ENTITY = "Feedback";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public InvoiceService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<InvoiceResponseDto>>> GetForPatientAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Invoices
                .Include(x => x.Appointment)
                .ThenInclude(x=> x.CreatedBy)
                .Where(x => x.Appointment.User.UserId == requestDto.Key).ToListAsync();
            return Responses.OKGetAll<List<InvoiceResponseDto>>(ENTITY, _mapper.Map<List<InvoiceResponseDto>>(data));
        }

        public async Task<ResponseDto<InvoiceResponseDto>> SaveAsync(InvoiceRequestDto requestDto)
        {
            if (requestDto.InvoiceId is 0)
            {
                var mapperObject = _mapper.Map<Invoices>(requestDto);

                _context.Invoices.Add(mapperObject);
                await _context.SaveChangesAsync();
                return Responses.OKAdded<InvoiceResponseDto>(ENTITY, _mapper.Map<InvoiceResponseDto>(mapperObject));
            }
            else
            {
                var data = await _context.Invoices.FindAsync(requestDto.InvoiceId);
                if (data is null)
                    return Responses.NotFound<InvoiceResponseDto>(ENTITY, null);

                var mapperObject = _mapper.Map<InvoiceRequestDto, Invoices>(requestDto, data);
                await _context.SaveChangesAsync();
                return Responses.OKUpdated<InvoiceResponseDto>(ENTITY, _mapper.Map<InvoiceResponseDto>(mapperObject));
            }
        }
    }
}

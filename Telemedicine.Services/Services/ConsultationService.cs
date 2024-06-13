using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    public class ConsultationService : IConsultationService
    {
        private const string ENTITY = "Consultation";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public ConsultationService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseDto<ConsultationResponseDto>> GetByIdAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Consultations.FirstOrDefaultAsync(x=> x.AppointmentId == requestDto.Key);
            if (data is null)
                return Responses.NotFound<ConsultationResponseDto>(ENTITY, null);

            return Responses.OKGetAll<ConsultationResponseDto>(ENTITY, _mapper.Map<ConsultationResponseDto>(data));
        }
        public async Task<ResponseDto<ConsultationResponseDto>> SaveAsync(ConsultationRequestDto requestDto)
        {
            var data = await _context.Consultations.FindAsync(requestDto.AppointmentId);
            if(data is null)
            {
                var mapperObject = _mapper.Map<Consultations>(requestDto);

                _context.Consultations.Add(mapperObject);
                await _context.SaveChangesAsync();
                return Responses.OKAdded<ConsultationResponseDto>(ENTITY, _mapper.Map<ConsultationResponseDto>(mapperObject));
            }
            else
            {
                var mapperObject = _mapper.Map<ConsultationRequestDto, Consultations>(requestDto, data);

                await _context.SaveChangesAsync();
                return Responses.OKUpdated<ConsultationResponseDto>(ENTITY, _mapper.Map<ConsultationResponseDto>(mapperObject));
            }
        }
    }
}

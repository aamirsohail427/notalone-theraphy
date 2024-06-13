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
    public class SOAPNoteService : ISOAPNoteService
    {
        private const string ENTITY = "SOAPNote";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public SOAPNoteService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<bool>> DeleteByIdAsync(RequestDto<long> requestDto)
        {
            var data = await _context.SOAPNotes.FindAsync(requestDto.Key);
            if (data is null)
                return Responses.NotFound<bool>(ENTITY, false);

            _context.SOAPNotes.Remove(data);
            await _context.SaveChangesAsync();

            return Responses.OKDeleted<bool>(ENTITY, true);
        }

        public async Task<ResponseDto<List<SOAPNoteResponseDto>>> GetAllAsync(RequestDto<long> requestDto)
        {
            var data = await _context.SOAPNotes.ToListAsync();
            if (data is null)
                return Responses.NotFound<List<SOAPNoteResponseDto>>(ENTITY, null);

            return Responses.OKGetAll<List<SOAPNoteResponseDto>>(ENTITY, _mapper.Map<List<SOAPNoteResponseDto>>(data));
        }

        public async Task<ResponseDto<List<SOAPNoteResponseDto>>> GetAllByAppointmentAsync(RequestDto<long> requestDto)
        {
            var data = await _context.SOAPNotes.Where(x => x.AppointmentId == requestDto.Key).ToListAsync();
            if (data is null)
                return Responses.NotFound<List<SOAPNoteResponseDto>>(ENTITY, null);

            return Responses.OKGetAll<List<SOAPNoteResponseDto>>(ENTITY, _mapper.Map<List<SOAPNoteResponseDto>>(data));
        }

        public async Task<ResponseDto<SOAPNoteResponseDto>> GetByIdAsync(RequestDto<long> requestDto)
        {
            var data = await _context.SOAPNotes.FindAsync(requestDto.Key);
            if (data is null)
                return Responses.NotFound<SOAPNoteResponseDto>(ENTITY, null);

            return Responses.OKGetAll<SOAPNoteResponseDto>(ENTITY, _mapper.Map<SOAPNoteResponseDto>(data));
        }

        public async Task<ResponseDto<SOAPNoteResponseDto>> SaveAsync(long userId, SOAPNoteRequestDto requestDto)
        {
            if (requestDto.SOAPNoteId is 0)
            {
                var mapperObject = _mapper.Map<SOAPNotes>(requestDto);

                _context.SOAPNotes.Add(mapperObject);
                await _context.SaveChangesAsync();
                return Responses.OKAdded<SOAPNoteResponseDto>(ENTITY, _mapper.Map<SOAPNoteResponseDto>(mapperObject));
            }
            else
            {
                var data = await _context.SOAPNotes.FindAsync(requestDto.SOAPNoteId);
                if (data is null)
                    return Responses.NotFound<SOAPNoteResponseDto>(ENTITY, null);

                var mapperObject = _mapper.Map<SOAPNoteRequestDto, SOAPNotes>(requestDto, data);
                await _context.SaveChangesAsync();
                return Responses.OKUpdated<SOAPNoteResponseDto>(ENTITY, _mapper.Map<SOAPNoteResponseDto>(mapperObject));
            }
        }

        public Task<ResponseDto<SOAPNoteResponseDto>> SaveAsync(long userId, DoctorAwardRequestDto requestDto)
        {
            throw new NotImplementedException();
        }
    }
}

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
    public class PatientAttachmentService : IPatientAttachmentService
    {
        private const string ENTITY = "Patient Attachment";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public PatientAttachmentService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<bool>> DeleteByIdAsync(RequestDto<long> requestDto)
        {
            var data = await _context.PatientAttachments.FindAsync(requestDto.Key);
            if (data is null)
                return Responses.NotFound<bool>(ENTITY, false);

            data.IsActive = false;
            await _context.SaveChangesAsync();

            return Responses.OKDeleted<bool>(ENTITY, true);
        }

        public async Task<ResponseDto<List<PatientAttachmentResponseDto>>> GetAllByUserAsync(RequestDto<long> requestDto)
        {
            var data = await _context.PatientAttachments.Where(x => x.CreatedById == requestDto.Key && x.IsActive == true).ToListAsync();
            if (data is null)
                return Responses.NotFound<List<PatientAttachmentResponseDto>>(ENTITY, null);

            return Responses.OKGetAll<List<PatientAttachmentResponseDto>>(ENTITY, _mapper.Map<List<PatientAttachmentResponseDto>>(data));
        }

        public async Task<ResponseDto<PatientAttachmentResponseDto>> GetByIdAsync(RequestDto<long> requestDto)
        {
            var data = await _context.PatientAttachments.FindAsync(requestDto.Key);
            if (data is null)
                return Responses.NotFound<PatientAttachmentResponseDto>(ENTITY, null);

            return Responses.OKGetAll<PatientAttachmentResponseDto>(ENTITY, _mapper.Map<PatientAttachmentResponseDto>(data));
        }

        public async Task<ResponseDto<PatientAttachmentResponseDto>> SaveAsync(long userId, PatientAttachmentRequestDto requestDto)
        {
            if (requestDto.PatientAttachmentId is 0)
            {
                var mapperObject = _mapper.Map<PatientAttachments>(requestDto);
                mapperObject.CreatedById = userId;
                mapperObject.CreatedDateTime = DateTime.UtcNow;

                _context.PatientAttachments.Add(mapperObject);
                await _context.SaveChangesAsync();
                return Responses.OKAdded<PatientAttachmentResponseDto>(ENTITY, _mapper.Map<PatientAttachmentResponseDto>(mapperObject));
            }
            else
            {
                var data = await _context.PatientAttachments.FindAsync(requestDto.PatientAttachmentId);
                if (data is null)
                    return Responses.NotFound<PatientAttachmentResponseDto>(ENTITY, null);

                requestDto.CreatedById = data.CreatedById;
                requestDto.CreatedDateTime = data.CreatedDateTime;

                var mapperObject = _mapper.Map<PatientAttachmentRequestDto, PatientAttachments>(requestDto, data);
                mapperObject.ModifiedById = userId;
                mapperObject.ModifiedDateTime = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Responses.OKUpdated<PatientAttachmentResponseDto>(ENTITY, _mapper.Map<PatientAttachmentResponseDto>(mapperObject));
            }
        }
    }
}

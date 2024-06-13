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
    public class DoctorEducationService : IDoctorEducationService
    {
        private const string ENTITY = "Doctor Education";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public DoctorEducationService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<bool>> DeleteByIdAsync(RequestDto<long> requestDto)
        {
            var data = await _context.DoctorEducations.FindAsync(requestDto.Key);
            if (data is null)
                return Responses.NotFound<bool>(ENTITY, false);

            data.IsActive = false;
            await _context.SaveChangesAsync();

            return Responses.OKDeleted<bool>(ENTITY, true);
        }

        public async Task<ResponseDto<List<DoctorEducationResponseDto>>> GetAllByUserAsync(RequestDto<long> requestDto)
        {
            var data = await _context.DoctorEducations.Include(x=> x.DoctorEducationType)
                .Where(x => x.CreatedById == requestDto.Key && x.IsActive == true).ToListAsync();
            if (data is null)
                return Responses.NotFound<List<DoctorEducationResponseDto>>(ENTITY, null);

            return Responses.OKGetAll<List<DoctorEducationResponseDto>>(ENTITY, _mapper.Map<List<DoctorEducationResponseDto>>(data));
        }

        public async Task<ResponseDto<DoctorEducationResponseDto>> GetByIdAsync(RequestDto<long> requestDto)
        {
            var data = await _context.DoctorEducations.FindAsync(requestDto.Key);
            if (data is null)
                return Responses.NotFound<DoctorEducationResponseDto>(ENTITY, null);

            return Responses.OKGetAll<DoctorEducationResponseDto>(ENTITY, _mapper.Map<DoctorEducationResponseDto>(data));
        }

        public async Task<ResponseDto<DoctorEducationResponseDto>> SaveAsync(long userId, DoctorEducationRequestDto requestDto)
        {
            if (requestDto.DoctorEducationId is 0)
            {
                var mapperObject = _mapper.Map<DoctorEducations>(requestDto);
                mapperObject.CreatedById = userId;
                mapperObject.CreatedDateTime = DateTime.UtcNow;

                _context.DoctorEducations.Add(mapperObject);
                await _context.SaveChangesAsync();
                return Responses.OKAdded<DoctorEducationResponseDto>(ENTITY, _mapper.Map<DoctorEducationResponseDto>(mapperObject));
            }
            else
            {
                var data = await _context.DoctorEducations.FindAsync(requestDto.DoctorEducationId);
                if (data is null)
                    return Responses.NotFound<DoctorEducationResponseDto>(ENTITY, null);

                requestDto.CreatedById = data.CreatedById;
                requestDto.CreatedDateTime = data.CreatedDateTime;

                var mapperObject = _mapper.Map<DoctorEducationRequestDto, DoctorEducations>(requestDto, data);
                mapperObject.ModifiedById = userId;
                mapperObject.ModifiedDateTime = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Responses.OKUpdated<DoctorEducationResponseDto>(ENTITY, _mapper.Map<DoctorEducationResponseDto>(mapperObject));
            }
        }
    }
}

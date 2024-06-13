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
    public class DoctorExperienceService : IDoctorExperienceService
    {
        private const string ENTITY = "Doctor Experience";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public DoctorExperienceService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<bool>> DeleteByIdAsync(RequestDto<long> requestDto)
        {
            var data = await _context.DoctorExperiences.FindAsync(requestDto.Key);
            if (data is null)
                return Responses.NotFound<bool>(ENTITY, false);

            data.IsActive = false;
            await _context.SaveChangesAsync();

            return Responses.OKDeleted<bool>(ENTITY, true);
        }

        public async Task<ResponseDto<List<DoctorExperienceResponseDto>>> GetAllByUserAsync(RequestDto<long> requestDto)
        {
            var data = await _context.DoctorExperiences.Where(x => x.CreatedById == requestDto.Key && x.IsActive == true).ToListAsync();
            if (data is null)
                return Responses.NotFound<List<DoctorExperienceResponseDto>>(ENTITY, null);

            return Responses.OKGetAll<List<DoctorExperienceResponseDto>>(ENTITY, _mapper.Map<List<DoctorExperienceResponseDto>>(data));
        }

        public async Task<ResponseDto<DoctorExperienceResponseDto>> GetByIdAsync(RequestDto<long> requestDto)
        {
            var data = await _context.DoctorExperiences.FindAsync(requestDto.Key);
            if (data is null)
                return Responses.NotFound<DoctorExperienceResponseDto>(ENTITY, null);

            return Responses.OKGetAll<DoctorExperienceResponseDto>(ENTITY, _mapper.Map<DoctorExperienceResponseDto>(data));
        }

        public async Task<ResponseDto<DoctorExperienceResponseDto>> SaveAsync(long userId, DoctorExperienceRequestDto requestDto)
        {
            if (requestDto.DoctorExperienceId is 0)
            {
                var mapperObject = _mapper.Map<DoctorExperiences>(requestDto);
                mapperObject.CreatedById = userId;
                mapperObject.CreatedDateTime = DateTime.UtcNow;

                _context.DoctorExperiences.Add(mapperObject);
                await _context.SaveChangesAsync();
                return Responses.OKAdded<DoctorExperienceResponseDto>(ENTITY, _mapper.Map<DoctorExperienceResponseDto>(mapperObject));
            }
            else
            {
                var data = await _context.DoctorExperiences.FindAsync(requestDto.DoctorExperienceId);
                if (data is null)
                    return Responses.NotFound<DoctorExperienceResponseDto>(ENTITY, null);

                requestDto.CreatedById = data.CreatedById;
                requestDto.CreatedDateTime = data.CreatedDateTime;

                var mapperObject = _mapper.Map<DoctorExperienceRequestDto, DoctorExperiences>(requestDto, data);
                mapperObject.ModifiedById = userId;
                mapperObject.ModifiedDateTime = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Responses.OKUpdated<DoctorExperienceResponseDto>(ENTITY, _mapper.Map<DoctorExperienceResponseDto>(mapperObject));
            }
        }
    }
}

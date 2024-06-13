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
    public class DoctorAwardService : IDoctorAwardService
    {
        private const string ENTITY = "Doctor Award";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public DoctorAwardService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<bool>> DeleteByIdAsync(RequestDto<long> requestDto)
        {
            var data = await _context.DoctorAwards.FindAsync(requestDto.Key);
            if (data is null)
                return Responses.NotFound<bool>(ENTITY, false);

            data.IsActive = false;
            await _context.SaveChangesAsync();

            return Responses.OKDeleted<bool>(ENTITY, true);
        }

        public async Task<ResponseDto<List<DoctorAwardResponseDto>>> GetAllByUserAsync(RequestDto<long> requestDto)
        {
            var data = await _context.DoctorAwards.Where(x=> x.CreatedById == requestDto.Key && x.IsActive == true).ToListAsync();
            if (data is null)
                return Responses.NotFound<List<DoctorAwardResponseDto>>(ENTITY, null);

            return Responses.OKGetAll<List<DoctorAwardResponseDto>>(ENTITY, _mapper.Map<List<DoctorAwardResponseDto>>(data));
        }

        public async Task<ResponseDto<DoctorAwardResponseDto>> GetByIdAsync(RequestDto<long> requestDto)
        {
            var data = await _context.DoctorAwards.FindAsync(requestDto.Key);
            if (data is null)
                return Responses.NotFound<DoctorAwardResponseDto>(ENTITY, null);

            return Responses.OKGetAll<DoctorAwardResponseDto>(ENTITY, _mapper.Map<DoctorAwardResponseDto>(data));
        }

        public async Task<ResponseDto<DoctorAwardResponseDto>> SaveAsync(long userId, DoctorAwardRequestDto requestDto)
        {
            if(requestDto.DoctorAwardId is 0)
            {
                var mapperObject = _mapper.Map<DoctorAwards>(requestDto);
                mapperObject.CreatedById = userId;
                mapperObject.CreatedDateTime = DateTime.UtcNow;

                _context.DoctorAwards.Add(mapperObject);
                await _context.SaveChangesAsync();
                return Responses.OKAdded<DoctorAwardResponseDto>(ENTITY, _mapper.Map<DoctorAwardResponseDto>(mapperObject));
            }
            else
            {
                var data = await _context.DoctorAwards.FindAsync(requestDto.DoctorAwardId);
                if (data is null)
                    return Responses.NotFound<DoctorAwardResponseDto>(ENTITY, null);

                requestDto.CreatedById = data.CreatedById;
                requestDto.CreatedDateTime = data.CreatedDateTime;

                var mapperObject = _mapper.Map<DoctorAwardRequestDto, DoctorAwards>(requestDto, data);
                mapperObject.ModifiedById = userId;
                mapperObject.ModifiedDateTime = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Responses.OKUpdated<DoctorAwardResponseDto>(ENTITY, _mapper.Map<DoctorAwardResponseDto>(mapperObject));
            }
        }
    }
}

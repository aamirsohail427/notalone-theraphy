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
    public class UserStateService : IUserStateService
    {
        private const string ENTITY = "User State";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public UserStateService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<UserStateResponseDto>>> GetAllByUserAsync(RequestDto<long> requestDto)
        {
            var data = await _context.UserStates.Where(x => x.UserId == requestDto.Key).ToListAsync();
            if (data is null)
                Responses.NotFound<List<UserStateResponseDto>>(ENTITY, null);

            return Responses.OKGetAll<List<UserStateResponseDto>>(ENTITY, _mapper.Map<List<UserStateResponseDto>>(data));
        }

        public async Task<ResponseDto<List<UserStateResponseDto>>> SaveAsync(long userId, List<UserStateRequestDto> requestDto)
        {
            var mapperObject = _mapper.Map<List<UserStates>>(requestDto);
            var data = await _context.UserStates.Where(x => x.UserId == userId).ToListAsync();
            if (data.Count is 0)
            {
                _context.UserStates.AddRange(mapperObject);
                await _context.SaveChangesAsync();
            }
            else if (data.Any())
            {
                _context.UserStates.RemoveRange(data);
                await _context.SaveChangesAsync();

                _context.UserStates.AddRange(mapperObject);
                await _context.SaveChangesAsync();
            }
            return Responses.OKAdded<List<UserStateResponseDto>>(ENTITY, _mapper.Map<List<UserStateResponseDto>>(mapperObject));
        }
    }
}

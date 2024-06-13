using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.ResponseDto;
using Telemedicine.Models.Models;
using Telemedicine.Services.Interfaces;

namespace Telemedicine.Services.Services
{
    public class LookupService : ILookupService
    {
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public LookupService(IMapper mapper, TelemedicineAppContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ResponseDto<List<LookupResponseDto>>> GetByTypeAsync(RequestDto<string> requestDto)
        {
            var data = await _context.Lookups.Where(x => x.LookupType.Equals(requestDto.Key)).ToListAsync();
            
            return new ResponseDto<List<LookupResponseDto>>()
            {
                Data = _mapper.Map<List<LookupResponseDto>>(data)
            };
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.ResponseDto;
using Telemedicine.Models.Models;
using Telemedicine.Services.Interfaces;

namespace Telemedicine.Services.Services
{
    public class SpecialtyLookupService : ISpecialtyLookupService
    {
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public SpecialtyLookupService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<SpecialtyLookupResponseDto>>> GetAllAsync()
        {
            var data = await _context.SpecialtyLookups.ToListAsync();

            return new ResponseDto<List<SpecialtyLookupResponseDto>>()
            {
                Data = _mapper.Map<List<SpecialtyLookupResponseDto>>(data)
            };
        }
    }
}

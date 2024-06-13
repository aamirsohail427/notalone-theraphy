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
    public class StateService : IStateService
    {
        private const string ENTITY = "State";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public StateService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<StateResponseDto>>> GetAllAsync()
        {
            var data = await _context.States.ToListAsync();

            return new ResponseDto<List<StateResponseDto>>()
            {
                Data = _mapper.Map<List<StateResponseDto>>(data)
            };
        }
    }
}

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
    public class UserRoleService : IUserRoleService
    {
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public UserRoleService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseDto<List<UserRoleResponseDto>>> GetAllAsync()
        {
            var data = await _context.UserRoles.ToListAsync();

            return new ResponseDto<List<UserRoleResponseDto>>()
            {
                Data = _mapper.Map<List<UserRoleResponseDto>>(data)
            };
        }
    }
}

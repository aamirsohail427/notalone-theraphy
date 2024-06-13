using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;
using Telemedicine.Models.Models;
using Telemedicine.Services.Interfaces;

namespace Telemedicine.Services.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public RegisterService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<UserResponseDto>> RegisterAsync(RegisterRequestDto requestDto)
        {
            var mapperObj = _mapper.Map<Users>(requestDto);
            mapperObj.IsActive = true;
            mapperObj.CreatedDateTime = DateTime.UtcNow;
            mapperObj.Logins = new List<Logins>();
            mapperObj.Logins.Add(new Logins()
            {
                Email = requestDto.Email,
                Password = requestDto.Password,
                IsLoginAllowed = true
            });
            
            _context.Users.Add(mapperObj);
            await _context.SaveChangesAsync();

            return new ResponseDto<UserResponseDto>()
            {
                Data = _mapper.Map<UserResponseDto>(mapperObj)
            };
        }
    }
}

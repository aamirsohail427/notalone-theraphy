using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    public class LoginService : ILoginService
    {
        private const string ENTITY = "User";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public LoginService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginRequestDto requestDto)
        {
            var data = await _context.Logins.FirstOrDefaultAsync(x => x.Email.Equals(requestDto.Email) && x.Password.Equals(requestDto.Password));
            if (data is null)
                Responses.NotFound();

            return new ResponseDto<LoginResponseDto>()
            {
                Data = _mapper.Map<LoginResponseDto>(data)
            };
        }

        public async Task<ResponseDto<bool>> ResetPasswordAsync(ResetPasswordRequestDto requestDto)
        {
            var objFromDb = await _context.Logins.FindAsync(requestDto.LoginId);
            if(!(objFromDb is null))
            {
                if (objFromDb.Password.Equals(requestDto.CurrentPassword))
                {
                    objFromDb.Password = requestDto.NewPassword;
                    await _context.SaveChangesAsync();
                    return Responses.OKUpdated<bool>("Password", true);
                }
                return new ResponseDto<bool>()
                {
                    Status = 0,
                    Message = ResponseStrings.InvalidPassword,
                    Data = false
                };
            }
            return Responses.NotFound<bool>(ENTITY, false);
        }

        public async Task<ResponseDto<bool>> UpdateStatusAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Logins.FindAsync(requestDto.Key);
            if (data is null)
                return Responses.NotFound<bool>(ENTITY, false);

            if (data.IsLoginAllowed)
                data.IsLoginAllowed = false;
            else
                data.IsLoginAllowed = true;

            await _context.SaveChangesAsync();
            return Responses.OKUpdated<bool>(ENTITY, true);
        }
    }
}

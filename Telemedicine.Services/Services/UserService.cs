using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;
using Telemedicine.Models.Dtos.StoreProcedureDto;
using Telemedicine.Models.Models;
using Telemedicine.Services.Interfaces;
using Telemedicine.Utilities;

namespace Telemedicine.Services.Services
{
    public class UserService : IUserService
    {
        private const string ENTITY = "User";
        private readonly IMapper _mapper;
        private readonly TelemedicineAppContext _context;

        public UserService(TelemedicineAppContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ResponseDto<AdminDashboardStatsDto>> AdminDashboardStatsByUserAsync(RequestDto<long> requestDto)
        {
            using (var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                var result = await conn.QueryAsync<string>(sql: "[dbo].[spAdminDashboardStats]",
                    param: new
                    {
                        UserId = requestDto.Key
                    },
                    commandType: CommandType.StoredProcedure);

                var builder = new StringBuilder();
                foreach (var r in result)
                {
                    builder.Append(r);
                }
                var response = JsonConvert.DeserializeObject<AdminDashboardStatsDto>(builder.ToString());

                return new ResponseDto<AdminDashboardStatsDto>()
                {
                    Data = response
                };
            }
        }

        public async Task<ResponseDto<DoctorDashboardStatsDto>> DoctorDashboardStatsByUserAsync(RequestDto<long> requestDto)
        {
            using (var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                var result = await conn.QueryAsync<string>(sql: "[dbo].[spDoctorDashboardStats]",
                    param: new
                    {
                        UserId = requestDto.Key
                    },
                    commandType: CommandType.StoredProcedure);

                var builder = new StringBuilder();
                foreach (var r in result)
                {
                    builder.Append(r);
                }
                var response = JsonConvert.DeserializeObject<DoctorDashboardStatsDto>(builder.ToString());

                return new ResponseDto<DoctorDashboardStatsDto>()
                {
                    Data = response
                };
            }
        }

        public async Task<ResponseDto<UserResponseDto>> GetByIdAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Users.FindAsync(requestDto.Key);
            if (data is null)
                Responses.NotFound<UserResponseDto>(ENTITY, null);

            return Responses.OKGet<UserResponseDto>(ENTITY, _mapper.Map<UserResponseDto>(data));
        }

        public async Task<ResponseDto<List<UserResponseDto>>> GetByTypeAsync(RequestDto<int> requestDto)
        {
            var data = await _context.Users.Where(x => x.UserRoleId == requestDto.Key).ToListAsync();
            return new ResponseDto<List<UserResponseDto>>()
            {
                Data = _mapper.Map<List<UserResponseDto>>(data)
            };
        }

        public async Task<ResponseDto<List<UserResponseDto>>> GetUsersAsync()
        {
            var data = await _context.Users
                .Include(x => x.Logins).ToListAsync();

            return Responses.OKGetAll(ENTITY, _mapper.Map<List<UserResponseDto>>(data));
        }

        public async Task<ResponseDto<PatientDashboardStatsDto>> PatientDashboardStatsByUserAsync(RequestDto<long> requestDto)
        {
            using (var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                var result = await conn.QueryAsync<string>(sql: "[dbo].[spPatientDashboardStats]",
                    param: new
                    {
                        UserId = requestDto.Key
                    },
                    commandType: CommandType.StoredProcedure);

                var builder = new StringBuilder();
                foreach (var r in result)
                {
                    builder.Append(r);
                }
                var response = JsonConvert.DeserializeObject<PatientDashboardStatsDto>(builder.ToString());

                return new ResponseDto<PatientDashboardStatsDto>()
                {
                    Data = response
                };
            }
        }

        public async Task<ResponseDto<UserRequestDto>> PopulateUserAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Users.FindAsync(requestDto.Key);
            if (data is null)
                Responses.NotFound();

            return new ResponseDto<UserRequestDto>()
            {
                Data = _mapper.Map<UserRequestDto>(data)
            };
        }

        public async Task<ResponseDto<UserResponseDto>> UpdateAsync(long id, UserRequestDto requestDto)
        {
            var data = await _context.Users.FindAsync(id);
            if(!(data is null))
            {
                var mapperObject = _mapper.Map<UserRequestDto, Users>(requestDto, data);
                mapperObject.ModifiedDateTime = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return Responses.OKUpdated<UserResponseDto>(ENTITY, _mapper.Map<UserResponseDto>(mapperObject));
            }
            return Responses.NotFound<UserResponseDto>(ENTITY, _mapper.Map<UserResponseDto>(requestDto));
        }

        public async Task<ResponseDto<bool>> UpdateProfilePictureAsync(long id, RequestDto<string> requestDto)
        {
            var data = await _context.Users.FindAsync(id);
            if (!(data is null))
            {
                data.ProfilePictureUrl = requestDto.Key;
                await _context.SaveChangesAsync();
                return Responses.OKUpdated<bool>(ENTITY, true);
            }
            return Responses.NotFound<bool>(ENTITY, false);
        }
    }
}

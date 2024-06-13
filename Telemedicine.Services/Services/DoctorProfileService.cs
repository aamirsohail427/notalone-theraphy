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
    public class DoctorProfileService : IDoctorProfileService
    {
        private const string ENTITY = "Doctor Profile";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public DoctorProfileService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseDto<bool>> DeleteLicenseByIdAsync(RequestDto<long> requestDto)
        {
            var data = await _context.DoctorProfiles.FirstOrDefaultAsync(x => x.UserId == requestDto.Key);
            if (data is null)
                Responses.NotFound<bool>(ENTITY, false);

            data.LicensedUrl = null;
            await _context.SaveChangesAsync();

            return Responses.OKDeleted<bool>("State(s) Licensed to Practice", true);
        }
        public async Task<ResponseDto<DoctorProfileResponseDto>> GetByUserAsync(RequestDto<long> requestDto)
        {
            var data = await _context.DoctorProfiles.FirstOrDefaultAsync(x => x.UserId == requestDto.Key);
            if (data is null)
                Responses.NotFound<DoctorProfileResponseDto>(ENTITY, _mapper.Map<DoctorProfileResponseDto>(data));

            return Responses.OKGet<DoctorProfileResponseDto>(ENTITY, _mapper.Map<DoctorProfileResponseDto>(data));
        }

        public async Task<ResponseDto<DoctorProfileDetailResponseDto>> GetDoctorProfileDetailByUserAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Users
                .Include(x=> x.Gender)
                .Include(x=> x.PreferredLanguageType)
                .Include(x=> x.SalutationType)
                .Include(x=> x.DoctorAwardsCreatedBy)
                .Include(x=> x.DoctorEducationsCreatedBy)
                .ThenInclude(x=> x.DoctorEducationType)
                .Include(x=> x.DoctorExperiencesCreatedBy)
                .Include(x=> x.FeedbacksToUser)
                .FirstOrDefaultAsync(x=> x.UserId == requestDto.Key);
            if (data is null)
                return Responses.NotFound<DoctorProfileDetailResponseDto>(ENTITY, null);

            return Responses.OKGetAll<DoctorProfileDetailResponseDto>(ENTITY, _mapper.Map<DoctorProfileDetailResponseDto>(data));
        }

        public async Task<ResponseDto<GetDoctorsByFiltersDto>> GetDoctorsByFiltersAsync(GetDoctorsByFiltersRequestDto requestDto)
        {
            using (var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                var result = await conn.QueryAsync<string>(sql: "[dbo].[spGetDoctorsByFilters]",
                    param: new
                    {
                        UserId = requestDto.UserId,
                        DoctorName = requestDto.DoctorName,
                        Address = requestDto.Address,
                        Latitude = requestDto.Latitude,
                        Longitude = requestDto.Longitude,
                        PageNumber = requestDto.PageNumber
                    },

                commandType: CommandType.StoredProcedure);
                StringBuilder builder = new StringBuilder();
                foreach (var r in result)
                {
                    builder.Append(r);
                }
                var response = JsonConvert.DeserializeObject<GetDoctorsByFiltersDto>(builder.ToString());
                return new ResponseDto<GetDoctorsByFiltersDto>()
                {
                    Data = response
                };
            }
        }
        public async Task<ResponseDto<GetDoctorsByFiltersDto>> GetDoctorsByDoctorNameAndStateAsync(GetDoctorsByFiltersRequestDto requestDto)
        {
            using (var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                var result = await conn.QueryAsync<string>(sql: "[dbo].[spGetDoctorsByDoctorNameAndState]",
                    param: new
                    {
                        UserId = requestDto.UserId,
                        DoctorName = requestDto.DoctorName,
                        SpecialtyLookupId = requestDto.SpecialtyLookupId,
                        PageNumber = requestDto.PageNumber
                        
                    },

                commandType: CommandType.StoredProcedure);
                StringBuilder builder = new StringBuilder();
                foreach (var r in result)
                {
                    builder.Append(r);
                }
                var response = JsonConvert.DeserializeObject<GetDoctorsByFiltersDto>(builder.ToString());
                return new ResponseDto<GetDoctorsByFiltersDto>()
                {
                    Data = response
                };
            }
        }

        public async Task<ResponseDto<DoctorProfileResponseDto>> SaveAsync(long userId, DoctorProfileRequestDto requestDto)
        {
            var data = await _context.DoctorProfiles.FirstOrDefaultAsync(x => x.UserId == userId);
            if (data is null)
            {
                var mapperObject = _mapper.Map<DoctorProfiles>(requestDto);
                _context.DoctorProfiles.Add(mapperObject);
                await _context.SaveChangesAsync();
                return Responses.OKAdded<DoctorProfileResponseDto>(ENTITY, _mapper.Map<DoctorProfileResponseDto>(data));
            }
            else
            {
                requestDto.DoctorProfileId = data.DoctorProfileId;
                if (string.IsNullOrEmpty(requestDto.LicensedUrl))
                    requestDto.LicensedUrl = data.LicensedUrl;

                var mapperObject = _mapper.Map<DoctorProfileRequestDto, DoctorProfiles>(requestDto, data);
                await _context.SaveChangesAsync();
                return Responses.OKUpdated<DoctorProfileResponseDto>(ENTITY, _mapper.Map<DoctorProfileResponseDto>(mapperObject));
            }
        }

        public async Task<ResponseDto<GetDoctorDetailDto>> GetDoctorDetailAsync(RequestDto<long> requestDto)
        {
            using (var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                var result = await conn.QueryAsync<string>(sql: "[dbo].[spGetDoctorDetail]",
                    param: new
                    {
                        UserId = requestDto.Key
                    },

                commandType: CommandType.StoredProcedure);
                StringBuilder builder = new StringBuilder();
                foreach (var r in result)
                {
                    builder.Append(r);
                }
                var response = JsonConvert.DeserializeObject<GetDoctorDetailDto>(builder.ToString());
                return new ResponseDto<GetDoctorDetailDto>()
                {
                    Data = response
                };
            }
        }
    }
}

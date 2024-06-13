using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.ResponseDto;
using Telemedicine.Models.Dtos.StoreProcedureDto;
using Telemedicine.Models.Models;
using Telemedicine.Services.Interfaces;
using Telemedicine.Utilities;

namespace Telemedicine.Services.Services
{
    public class PatientService : IPatientService
    {
        private const string ENTITY = "Patient";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public PatientService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseDto<List<GetMyPatients>>> GetByDoctorAsync(RequestDto<long> requestDto)
        {
            using (var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                var result = await conn.QueryAsync<GetMyPatients>
                    (
                        sql: "[dbo].[spGetMyPatients]",
                        param: new { UserId = requestDto.Key },
                        commandType: CommandType.StoredProcedure
                    );

                return Responses.OKGetAll<List<GetMyPatients>>(ENTITY, result.ToList());
            }
        }

        public async Task<ResponseDto<PatientProfileDetailResponseDto>> GetPatientProfileDetailByUserAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Users
                .Include(x => x.Gender)
                .Include(x => x.PreferredLanguageType)
                .Include(x => x.SalutationType)
                .Include(x => x.PatientAttachmentsCreatedBy)
                .FirstOrDefaultAsync(x => x.UserId == requestDto.Key);
            if (data is null)
                return Responses.NotFound<PatientProfileDetailResponseDto>(ENTITY, null);

            return Responses.OKGetAll<PatientProfileDetailResponseDto>(ENTITY, _mapper.Map<PatientProfileDetailResponseDto>(data));
        }

        public async Task<ResponseDto<List<SOAPNoteResponseDto>>> GetSOAPNotesByUserAsync(RequestDto<long> requestDto)
        {
            List<SOAPNoteResponseDto> soapNoteResponseDtos = new List<SOAPNoteResponseDto>();
            var data = await _context.Appointments
                .Include(x=> x.SOAPNotes)
                .Where(x => x.UserId == requestDto.Key).ToListAsync();
            if (data is null)
                return Responses.NotFound<List<SOAPNoteResponseDto>>(ENTITY, null);

            foreach(var item in data)
            {
                foreach(var soapNote in item.SOAPNotes)
                {
                    soapNoteResponseDtos.Add(_mapper.Map<SOAPNoteResponseDto>(soapNote));
                }
            }

            return Responses.OKGetAll<List<SOAPNoteResponseDto>>(ENTITY, soapNoteResponseDtos);
        }

        public async Task<ResponseDto<List<AppointmentResponseDto>>> GetAppointmentsWithSOAPNotesByUserAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Appointments
                .Include(x => x.SOAPNotes)
                .Include(x=> x.CreatedBy)
                .Where(x => x.UserId == requestDto.Key).ToListAsync();
            if (data is null)
                return Responses.NotFound<List<AppointmentResponseDto>>(ENTITY, null);

            return Responses.OKGetAll<List<AppointmentResponseDto>>(ENTITY, _mapper.Map<List<AppointmentResponseDto>>(data));
        }
    }
}

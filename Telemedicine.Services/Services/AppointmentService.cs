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
    public class AppointmentService : IAppointmentService
    {
        private const string ENTITY = "Appointment";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;
        private readonly ISessionManager _sessionManager;
        private readonly IConversationService _conversationService;

        public AppointmentService(TelemedicineAppContext context, IMapper mapper, ISessionManager sessionManager, IConversationService conversationService)
        {
            _context = context;
            _mapper = mapper;
            _sessionManager = sessionManager;
            _conversationService = conversationService;
        }

        public async Task<ResponseDto<AppointmentResponseDto>> BookAsync(long userId, RequestDto<long> requestDto)
        {
            var data = await _context.Appointments.FindAsync(requestDto.Key);
            if (data is null)
                return Responses.NotFound<AppointmentResponseDto>(ENTITY, null);

            data.UserId = userId;
            data.ModifiedById = userId;
            data.ModifiedDateTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();

           await _conversationService.InitiateConversationAsync(new ConversationRequestDto
            {
                FromUserId = userId,
                ToUserId = data.CreatedById
            });

            return new ResponseDto<AppointmentResponseDto>()
            {
                Message = "Appointment booked successfully",
                Data = _mapper.Map<AppointmentResponseDto>(data)
            };
        }

        public async Task<ResponseDto<AppointmentResponseDto>> CancelAppointmentAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Appointments.FindAsync(requestDto.Key);
            var objectToReturn = data;

            if (data is null)
                return Responses.NotFound<AppointmentResponseDto>(ENTITY, null);

            data.UserId = null;
            await _context.SaveChangesAsync();
            return Responses.OK<AppointmentResponseDto>("Appointment canceled successfully", _mapper.Map<AppointmentResponseDto>(objectToReturn));
        }

        public async Task<ResponseDto<List<AppointmentResponseDto>>> CheckOverlapAppointmentsAsync(CheckOverlapAppointmentRequestDto requestDto)
        {
            using (var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                var result = await conn.QueryAsync<AppointmentResponseDto>(sql: "[dbo].[spCheckOverlapAppointments]",
                    param: new
                    {
                        UserId = requestDto.UserId,
                        StartDatetime = requestDto.StartDateTime,
                        EndDatetime = requestDto.EndDateTime
                    },
                    commandType: CommandType.StoredProcedure);

                return new ResponseDto<List<AppointmentResponseDto>>()
                {
                    Data = result.ToList()
                };
            }
        }

        public async Task<ResponseDto<bool>> CreateSlotsAsync(long userId, CreateSlotRequestDto requestDto)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("No appointment created because all slots are being overlaped");
            bool isBuilderHasSlot = false;

            List<Appointments> appointments = new List<Appointments>();

            DateTime startDatetime = requestDto.Date.Date.Add(requestDto.StartTime).ToTimezoneFormatDateTimeToUTC(_sessionManager.GetTimezoneId());
            DateTime endDatetime = requestDto.Date.Date.Add(requestDto.EndTime).ToTimezoneFormatDateTimeToUTC(_sessionManager.GetTimezoneId());
            TimeSpan difference = endDatetime.Subtract(startDatetime);
            double totalMinutes = difference.TotalMinutes;
            double loopLength = totalMinutes / requestDto.Interval;

            var checkOverlapAppointments = await CheckOverlapAppointmentsAsync(new CheckOverlapAppointmentRequestDto()
            {
                UserId = userId,
                StartDateTime = startDatetime,
                EndDateTime = endDatetime
            });
            var overlapAppointments = checkOverlapAppointments.Data;

            for (var i = 1; i <= loopLength; i++)
            {
                if (!(overlapAppointments is null))
                {
                    if (overlapAppointments.Any())
                    {
                        var interval = requestDto.Interval - 1;
                        var apts = from oa in overlapAppointments
                                   where oa.StartDate.Ticks >= startDatetime.Ticks && oa.StartDate.Ticks <= startDatetime.AddMinutes(interval).Ticks
                                   select oa;

                        if (apts.ToList().Count is 0)
                        {
                            if (!isBuilderHasSlot)
                            {
                                builder = new StringBuilder();
                                builder.Append("Appointment Slots created successfully");
                                isBuilderHasSlot = true;
                            }

                            appointments.Add(new Appointments()
                            {
                                AppointmentTypeId = requestDto.AppointmentTypeId,
                                CreatedById = userId,
                                CreatedDateTime = DateTime.UtcNow,
                                StartDate = startDatetime,
                                EndDate = startDatetime.AddMinutes(requestDto.Interval),
                                IsActive = true,
                            });

                            builder.Append("<br>" + startDatetime.ToTimezoneFormatDateTime(_sessionManager.GetTimezoneId()) + " - " + startDatetime.AddMinutes(requestDto.Interval).ToTimezoneFormatDateTime(_sessionManager.GetTimezoneId()) + "");
                        }
                        //foreach (var item in overlapAppointments)
                        //{
                        //    DateTime overlapAppointmentStartDate = item.StartDate;
                        //    DateTime overlapAppointmentEndDate = item.EndDate;

                        //    if (
                        //        (startDatetime.Ticks > overlapAppointmentStartDate.Ticks && startDatetime.Ticks < overlapAppointmentEndDate.Ticks)
                        //        &&
                        //        (startDatetime.AddMinutes(createSlotRequestDto.Interval).Ticks > overlapAppointmentStartDate.Ticks && startDatetime.AddMinutes(createSlotRequestDto.Interval).Ticks < overlapAppointmentEndDate.Ticks)
                        //       )
                        //    {

                        //    }
                        //}
                    }
                    else
                    {
                        if (!isBuilderHasSlot)
                        {
                            builder = new StringBuilder();
                            builder.Append("Appointment Slots created successfully");
                            isBuilderHasSlot = true;
                        }

                        appointments.Add(new Appointments()
                        {
                            AppointmentTypeId = requestDto.AppointmentTypeId,
                            CreatedById = userId,
                            CreatedDateTime = DateTime.UtcNow,
                            StartDate = startDatetime,
                            EndDate = startDatetime.AddMinutes(requestDto.Interval),
                            IsActive = true,
                        });
                        builder.Append("<br>" + startDatetime.ToTimezoneFormatDateTime(_sessionManager.GetTimezoneId()) + " - " + startDatetime.AddMinutes(requestDto.Interval).ToTimezoneFormatDateTime(_sessionManager.GetTimezoneId()) + "");
                    }
                    startDatetime = startDatetime.AddMinutes(requestDto.Interval);
                }
            }

            _context.Appointments.AddRange(appointments);
            await _context.SaveChangesAsync();
            return new ResponseDto<bool>()
            {
                Message = builder.ToString(),
                Data = true
            };
        }

        public async Task<ResponseDto<AppointmentResponseDto>> DeleteAppointmentAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Appointments.FindAsync(requestDto.Key);
            if (data is null)
                return Responses.NotFound<AppointmentResponseDto>(ENTITY, null);

            data.IsActive = false;
            await _context.SaveChangesAsync();
            return Responses.OKDeleted<AppointmentResponseDto>(ENTITY, _mapper.Map<AppointmentResponseDto>(data));
        }

        public async Task<ResponseDto<bool>> DeleteByIdAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Appointments.FindAsync(requestDto.Key);
            if (data is null)
                return Responses.NotFound<bool>(ENTITY, false);

            data.IsActive = false;
            await _context.SaveChangesAsync();

            return Responses.OKDeleted<bool>(ENTITY, true);
        }

        public async Task<ResponseDto<AppointmentResponseDto>> GetByIdAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Appointments
                .Include(x => x.Consultations)
                .FirstOrDefaultAsync(x => x.AppointmentId == requestDto.Key);
            if (data is null)
                return Responses.NotFound<AppointmentResponseDto>(ENTITY, null);

            return Responses.OKGet<AppointmentResponseDto>(ENTITY, _mapper.Map<AppointmentResponseDto>(data));
        }

        public async Task<ResponseDto<List<AppointmentResponseDto>>> GetForDoctorAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Appointments.Include(x => x.User).Where(x => x.CreatedById == requestDto.Key && x.IsActive == true).ToListAsync();
            if (data.Any())
            {
                data.ForEach(item => item.StartDate = Convert.ToDateTime(item.StartDate.ToTimezoneFormatDateTime(_sessionManager.GetTimezoneId())));
                data.ForEach(item => item.EndDate = Convert.ToDateTime(item.EndDate.ToTimezoneFormatDateTime(_sessionManager.GetTimezoneId())));
            }
            return Responses.OKGetAll<List<AppointmentResponseDto>>(ENTITY, _mapper.Map<List<AppointmentResponseDto>>(data));
        }

        public async Task<ResponseDto<List<AppointmentResponseDto>>> GetForPatientAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Appointments.Include(x => x.CreatedBy).Where(x => x.UserId == requestDto.Key && x.IsActive == true).ToListAsync();
            if (data.Any())
            {
                data.ForEach(item => item.StartDate = Convert.ToDateTime(item.StartDate.ToTimezoneFormatDateTime(_sessionManager.GetTimezoneId())));
                data.ForEach(item => item.EndDate = Convert.ToDateTime(item.EndDate.ToTimezoneFormatDateTime(_sessionManager.GetTimezoneId())));
            }
            return Responses.OKGetAll<List<AppointmentResponseDto>>(ENTITY, _mapper.Map<List<AppointmentResponseDto>>(data));
        }

        public async Task<ResponseDto<List<CheckUpcomingAppointmentsDto>>> GetUpcomingsAsync(RequestDto<long> requestDto)
        {
            using (var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                var result = await conn.QueryAsync<string>(sql: "[dbo].[spCheckUpcomingAppointments]",
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
                var response = JsonConvert.DeserializeObject<List<CheckUpcomingAppointmentsDto>>(builder.ToString());

                return new ResponseDto<List<CheckUpcomingAppointmentsDto>>()
                {
                    Data = response
                };
            }
        }

        public async Task<ResponseDto<AppointmentResponseDto>> SaveAsync(long userId, AppointmentRequestDto requestDto)
        {
            if (requestDto.AppointmentId is 0)
            {
                var mapperObject = _mapper.Map<Appointments>(requestDto);
                mapperObject.CreatedById = userId;
                mapperObject.CreatedDateTime = DateTime.UtcNow;

                _context.Appointments.Add(mapperObject);
                await _context.SaveChangesAsync();
                return Responses.OKAdded<AppointmentResponseDto>(ENTITY, _mapper.Map<AppointmentResponseDto>(mapperObject));
            }
            else
            {
                var data = await _context.Appointments.FindAsync(requestDto.AppointmentId);
                if (data is null)
                    return Responses.NotFound<AppointmentResponseDto>(ENTITY, null);

                requestDto.CreatedById = data.CreatedById;
                requestDto.CreatedDateTime = data.CreatedDateTime;

                var mapperObject = _mapper.Map<AppointmentRequestDto, Appointments>(requestDto, data);
                mapperObject.ModifiedById = userId;
                mapperObject.ModifiedDateTime = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Responses.OKUpdated<AppointmentResponseDto>(ENTITY, _mapper.Map<AppointmentResponseDto>(mapperObject));
            }
        }
    }
}

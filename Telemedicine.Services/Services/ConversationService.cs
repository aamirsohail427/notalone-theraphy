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
    public class ConversationService : IConversationService
    {
        private const string ENTITY = "Conversation";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public ConversationService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<ConversationResponseDto>> InitiateConversationAsync(ConversationRequestDto requestDto)
        {
            if (!(await _context.Conversations.AnyAsync(x => (x.FromUserId == requestDto.FromUserId
                                                         && x.ToUserId == requestDto.ToUserId) ||
                                                         (x.ToUserId == requestDto.FromUserId
                                                         && x.FromUserId == requestDto.ToUserId)
                                                         && x.ParentId == null)))
            {
                var mapperObject = _mapper.Map<Conversations>(requestDto);
                _context.Conversations.Add(mapperObject);
                await _context.SaveChangesAsync();
                return Responses.OKAdded<ConversationResponseDto>(ENTITY, _mapper.Map<ConversationResponseDto>(mapperObject));
            }
            return Responses.OKAdded<ConversationResponseDto>(ENTITY, new ConversationResponseDto());
        }

        public async Task<ResponseDto<ConversationResponseDto>> SaveAsync(ConversationRequestDto requestDto)
        {
            var mapperObject = _mapper.Map<Conversations>(requestDto);
            _context.Conversations.Add(mapperObject);
            await _context.SaveChangesAsync();

            return Responses.OKAdded<ConversationResponseDto>(ENTITY, new ConversationResponseDto());
        }
        public async Task<ResponseDto<List<ConversationResponseDto>>> GetByIdAsync(RequestDto<long> requestDto)
        {
            using (var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {

                var result = await conn.QueryAsync<ConversationResponseDto>(sql: "[dbo].[spGetConversationDetail]",
                param: new
                {
                    ParentId = requestDto.Key
                },
                commandType: CommandType.StoredProcedure);
                var response = result.ToList();

                return new ResponseDto<List<ConversationResponseDto>>()
                {
                    Data = response
                };


            }
        }
        public async Task<ResponseDto<List<ConversationUsersDto>>> GetConversationsUserAsync(RequestDto<long> requestDto)
        {
            using (var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {

                var result = await conn.QueryAsync<ConversationUsersDto>(sql: "[dbo].[spGetConversationUsers]",
                param: new
                {
                    UserId = requestDto.Key
                },
                commandType: CommandType.StoredProcedure);
                var response = result.ToList();

                return new ResponseDto<List<ConversationUsersDto>>()
                {
                    Data = response
                };


            }
        }
    }
}

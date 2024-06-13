using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class FeedbackService : IFeedbackService
    {
        private const string ENTITY = "Feedback";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public FeedbackService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<FeedbackResponseDto>>> GetForDoctorAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Feedbacks.Include(x => x.FromUser).Where(x => x.ToUserId == requestDto.Key).ToListAsync();
            return Responses.OKGetAll<List<FeedbackResponseDto>>(ENTITY, _mapper.Map<List<FeedbackResponseDto>>(data));
        }

        public async Task<ResponseDto<List<FeedbackResponseDto>>> GetForPatientAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Feedbacks.Include(x => x.ToUser).Where(x => x.FromUserId == requestDto.Key).ToListAsync();
            return Responses.OKGetAll<List<FeedbackResponseDto>>(ENTITY, _mapper.Map<List<FeedbackResponseDto>>(data));
        }

        public async Task<ResponseDto<FeedbackResponseDto>> SaveAsync(FeedbackRequestDto requestDto)
        {
            var data = await _context.Feedbacks.FirstOrDefaultAsync(x => x.FromUserId == requestDto.FromUserId && x.ToUserId == requestDto.ToUserId);
            if(data is null)
            {
                var mapperObject = _mapper.Map<Feedbacks>(requestDto);
                mapperObject.CreatedDateTime = DateTime.UtcNow;
                _context.Feedbacks.Add(mapperObject);
                await _context.SaveChangesAsync();
                return Responses.OKAdded<FeedbackResponseDto>(ENTITY, _mapper.Map<FeedbackResponseDto>(mapperObject));
            }
            else
            {
                requestDto.FeedbackId = data.FeedbackId;
                requestDto.CreatedDateTime = data.CreatedDateTime;
                var mapperObject = _mapper.Map<FeedbackRequestDto, Feedbacks>(requestDto, data);
                mapperObject.ModifiedDateTime = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return Responses.OKUpdated<FeedbackResponseDto>(ENTITY, _mapper.Map<FeedbackResponseDto>(mapperObject));
            }
        }
    }
}

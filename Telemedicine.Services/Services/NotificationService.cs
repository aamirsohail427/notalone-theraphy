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
    public class NotificationService : INotificationService
    {
        private const string ENTITY = "Notification";
        private readonly TelemedicineAppContext _context;
        private readonly IMapper _mapper;

        public NotificationService(TelemedicineAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<NotificationResponseDto>>> GetAllAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Notifications.Where(x=> x.ToUserId == requestDto.Key).OrderByDescending(x=> x.CreatedDateTime).ToListAsync();
            if (data is null)
                return Responses.NotFound<List<NotificationResponseDto>>(ENTITY, null);

            return Responses.OKGetAll<List<NotificationResponseDto>>(ENTITY, _mapper.Map<List<NotificationResponseDto>>(data));
        }

        public async Task<ResponseDto<bool>> ReadAllAsync(RequestDto<long> requestDto)
        {
            var data = await _context.Notifications.Where(x => x.ToUserId == requestDto.Key).ToListAsync();
            if (data is null)
                return Responses.NotFound<bool>(ENTITY, false);

            data.ForEach(item => item.IsRead = true);
            await _context.SaveChangesAsync();

            return Responses.OKUpdated<bool>(ENTITY, true);
        }

        public async Task<ResponseDto<NotificationResponseDto>> SaveAsync(NotificationRequestDto requestDto)
        {
            if (requestDto.NotificationId is 0)
            {
                var mapperObject = _mapper.Map<Notifications>(requestDto);

                _context.Notifications.Add(mapperObject);
                await _context.SaveChangesAsync();
                return Responses.OKAdded<NotificationResponseDto>(ENTITY, _mapper.Map<NotificationResponseDto>(mapperObject));
            }
            else
            {
                var data = await _context.Notifications.FindAsync(requestDto.NotificationId);
                if (data is null)
                    return Responses.NotFound<NotificationResponseDto>(ENTITY, null);

                var mapperObject = _mapper.Map<NotificationRequestDto, Notifications>(requestDto, data);
                await _context.SaveChangesAsync();
                return Responses.OKUpdated<NotificationResponseDto>(ENTITY, _mapper.Map<NotificationResponseDto>(mapperObject));
            }
        }
    }
}

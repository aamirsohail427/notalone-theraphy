﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;

namespace Telemedicine.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<ResponseDto<List<FeedbackResponseDto>>> GetForPatientAsync(RequestDto<long> requestDto);
        Task<ResponseDto<List<FeedbackResponseDto>>> GetForDoctorAsync(RequestDto<long> requestDto);
        Task<ResponseDto<FeedbackResponseDto>> SaveAsync(FeedbackRequestDto requestDto);
    }
}
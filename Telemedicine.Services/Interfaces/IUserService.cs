using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;
using Telemedicine.Models.Dtos.StoreProcedureDto;

namespace Telemedicine.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseDto<UserResponseDto>> GetByIdAsync(RequestDto<long> requestDto);
        Task<ResponseDto<List<UserResponseDto>>> GetByTypeAsync(RequestDto<int> requestDto);
        Task<ResponseDto<UserRequestDto>> PopulateUserAsync(RequestDto<long> requestDto);
        Task<ResponseDto<UserResponseDto>> UpdateAsync(long id, UserRequestDto requestDto);
        Task<ResponseDto<bool>> UpdateProfilePictureAsync(long id, RequestDto<string> requestDto);
        Task<ResponseDto<AdminDashboardStatsDto>> AdminDashboardStatsByUserAsync(RequestDto<long> requestDto);
        Task<ResponseDto<DoctorDashboardStatsDto>> DoctorDashboardStatsByUserAsync(RequestDto<long> requestDto);
        Task<ResponseDto<PatientDashboardStatsDto>> PatientDashboardStatsByUserAsync(RequestDto<long> requestDto);
        Task<ResponseDto<List<UserResponseDto>>> GetUsersAsync();
    }
}

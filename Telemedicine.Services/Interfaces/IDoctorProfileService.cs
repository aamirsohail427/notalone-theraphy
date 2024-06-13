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
    public interface IDoctorProfileService
    {
        Task<ResponseDto<DoctorProfileResponseDto>> GetByUserAsync(RequestDto<long> requestDto);
        Task<ResponseDto<DoctorProfileResponseDto>> SaveAsync(long userId, DoctorProfileRequestDto requestDto);
        Task<ResponseDto<bool>> DeleteLicenseByIdAsync(RequestDto<long> requestDto);
        Task<ResponseDto<GetDoctorsByFiltersDto>> GetDoctorsByFiltersAsync(GetDoctorsByFiltersRequestDto requestDto);
        Task<ResponseDto<GetDoctorsByFiltersDto>> GetDoctorsByDoctorNameAndStateAsync(GetDoctorsByFiltersRequestDto requestDto);
        Task<ResponseDto<DoctorProfileDetailResponseDto>> GetDoctorProfileDetailByUserAsync(RequestDto<long> requestDto);
        Task<ResponseDto<GetDoctorDetailDto>> GetDoctorDetailAsync(RequestDto<long> requestDto);
    }
}

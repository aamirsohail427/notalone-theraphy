using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;

namespace Telemedicine.Services.Interfaces
{
    public interface ISessionManager
    {
        public long GetLoginId();
        public string GetProfilePicture();
        public string GetRole();
        public long GetUserId();
        public string GetUsername();
        public int GetUserRoleId();
        public string GetTimezoneId();
        public string GetTokenId();
        public List<Claim> RefreshCookieClaims(UserSessionRequestDto requestDto);
    }
}

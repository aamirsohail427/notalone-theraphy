
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;
using Telemedicine.Services.Interfaces;
using Telemedicine.Utilities;

namespace Telemedicine.Services.Services
{
    public class SessionManager : ISessionManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long GetLoginId()
        {
            return Convert.ToInt64(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == SessionStrings.LoginId)?.Value);
        }

        public string GetProfilePicture()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == SessionStrings.ProfilePicture)?.Value;
        }

        public string GetRole()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        }

        public string GetTimezoneId()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == SessionStrings.TimeZoneId)?.Value;
        }

        public long GetUserId()
        {
            return Convert.ToInt64(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == SessionStrings.UserId)?.Value);
        }

        public string GetUsername()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == SessionStrings.Username)?.Value;
        }

        public int GetUserRoleId()
        {
            return Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == SessionStrings.UserRoleId)?.Value);
        }

        public string GetTokenId()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == SessionStrings.TokenId)?.Value;
        }
        public List<Claim> RefreshCookieClaims(UserSessionRequestDto requestDto)
        {
            return new List<Claim>
                        {
                            new Claim(SessionStrings.LoginId, requestDto.LoginId),
                            new Claim(SessionStrings.UserId, requestDto.UserId),
                            new Claim(SessionStrings.UserRoleId, requestDto.UserRoleId),
                            new Claim(SessionStrings.Username, requestDto.Username),
                            new Claim(SessionStrings.ProfilePicture, requestDto.ProfilePicture),
                            new Claim(ClaimTypes.Role, requestDto.Role),
                            new Claim(SessionStrings.TimeZoneId, requestDto.TimeZoneId),
                            new Claim(SessionStrings.TokenId,requestDto.TokenId),
                        };
        }
    }
}

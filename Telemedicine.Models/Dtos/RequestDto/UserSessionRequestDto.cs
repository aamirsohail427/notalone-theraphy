using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.RequestDto
{
    public class UserSessionRequestDto
    {
        public string LoginId { get; set; }
        public string UserId { get; set; }
        public string UserRoleId { get; set; }
        public string Username { get; set; }
        public string ProfilePicture { get; set; }
        public string Role { get; set; }
        public string TimeZoneId { get; set; }
        public string TokenId { get; set; }
    }
}

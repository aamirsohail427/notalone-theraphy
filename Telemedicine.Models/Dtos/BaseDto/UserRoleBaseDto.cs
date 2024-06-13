using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.BaseDto
{
    public class UserRoleBaseDto
    {
        public int UserRoleId { get; set; }
        public string Title { get; set; }
        public bool? IsActive { get; set; }
    }
}

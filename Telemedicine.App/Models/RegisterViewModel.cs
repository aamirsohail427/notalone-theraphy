using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos.RequestDto;

namespace Telemedicine.App.Models
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            UserRoles = new List<SelectListItem>();
            RegisterRequestDto = new RegisterRequestDto();
        }

        [Required]
        public int UserRoleId { get; set; }
        public List<SelectListItem> UserRoles { get; set; }
       
        public RegisterRequestDto RegisterRequestDto { get; set; }
    }
}

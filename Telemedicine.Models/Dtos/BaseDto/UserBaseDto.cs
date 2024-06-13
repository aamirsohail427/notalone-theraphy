using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Telemedicine.Models.Dtos.BaseDto
{
    public class UserBaseDto
    {
        public long UserId { get; set; }
        public int UserRoleId { get; set; }
        public int? SalutationTypeId { get; set; }
        public int? GenderId { get; set; }
        public int? PreferredLanguageTypeId { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Phone { get; set; }
        public string PrimaryAddress { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PostalCode { get; set; }
        public string Bio { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDateTime { get; set; }
    }
}

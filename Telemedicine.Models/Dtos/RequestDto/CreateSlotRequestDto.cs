using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Telemedicine.Models.Dtos.RequestDto
{
    public class CreateSlotRequestDto
    {
        [Required]
        [Display(Name = "Appointment Type")]
        public int AppointmentTypeId { get; set; }

        [Required]
        [Display(Name = "Interval")]
        public int Interval { get; set; }

        [Required]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [RestrictPreviousDate(ErrorMessage = "You cannot create previous date appointment slots")]
        [Display(Name = "Appointment Date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; }
    }
    public class RestrictPreviousDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dt = (DateTime)value;
            if (dt >= DateTime.UtcNow)
                return true;

            return false;
        }
    }
}

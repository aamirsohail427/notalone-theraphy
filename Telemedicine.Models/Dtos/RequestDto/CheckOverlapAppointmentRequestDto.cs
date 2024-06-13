using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.RequestDto
{
    public class CheckOverlapAppointmentRequestDto
    {
        public long UserId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}

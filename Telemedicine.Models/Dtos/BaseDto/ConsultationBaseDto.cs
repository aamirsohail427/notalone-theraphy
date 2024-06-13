using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.BaseDto
{
    public class ConsultationBaseDto
    {
        public long ConsultationId { get; set; }
        public long AppointmentId { get; set; }
        public string SessionId { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class Consultations
    {
        public long ConsultationId { get; set; }
        public long AppointmentId { get; set; }
        public string SessionId { get; set; }

        public virtual Appointments Appointment { get; set; }
    }
}

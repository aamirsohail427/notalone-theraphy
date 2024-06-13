using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class SOAPNotes
    {
        public long SOAPNoteId { get; set; }
        public long AppointmentId { get; set; }
        public string Subjective { get; set; }
        public string Objective { get; set; }
        public string Assessment { get; set; }
        public string Plans { get; set; }

        public virtual Appointments Appointment { get; set; }
    }
}

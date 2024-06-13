using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.BaseDto
{
    public class SOAPNoteBaseDto
    {
        public long SOAPNoteId { get; set; }
        public long AppointmentId { get; set; }
        public string Subjective { get; set; }
        public string Objective { get; set; }
        public string Assessment { get; set; }
        public string Plans { get; set; }
    }
}

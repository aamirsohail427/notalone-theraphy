using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.BaseDto
{
    public class InvoiceBaseDto
    {
        public long InvoiceId { get; set; }
        public long AppointmentId { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal AmountPaid { get; set; }
    }
}

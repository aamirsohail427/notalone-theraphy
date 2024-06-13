using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class Invoices
    {
        public long InvoiceId { get; set; }
        public long AppointmentId { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal AmountPaid { get; set; }

        public virtual Appointments Appointment { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Telemedicine.Models.Models
{
    public partial class Appointments
    {
        public Appointments()
        {
            Consultations = new HashSet<Consultations>();
            Invoices = new HashSet<Invoices>();
            SOAPNotes = new HashSet<SOAPNotes>();
        }

        public long AppointmentId { get; set; }
        public int AppointmentTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long? UserId { get; set; }
        public bool? IsActive { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        public virtual Lookups AppointmentType { get; set; }
        public virtual Users CreatedBy { get; set; }
        public virtual Users ModifiedBy { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<Consultations> Consultations { get; set; }
        public virtual ICollection<Invoices> Invoices { get; set; }
        public virtual ICollection<SOAPNotes> SOAPNotes { get; set; }
    }
}

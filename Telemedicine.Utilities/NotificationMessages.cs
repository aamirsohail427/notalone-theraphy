using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Utilities
{
    public class NotificationMessages
    {
        public static string AppointmentBooked(string fromUsername)
        {
            return string.Format("{0} booked your appointment", fromUsername);
        }
        public static string SOAPNoteAdded(string fromUsername)
        {
            return string.Format("{0} added Clinical Note", fromUsername);
        }

        public static string AppointmentCanceled(string fromUsername)
        {
            return string.Format("{0} canceled your appointment", fromUsername);
        }
    }
}

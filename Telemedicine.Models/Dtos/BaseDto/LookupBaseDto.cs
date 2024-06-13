using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.BaseDto
{
    public class LookupBaseDto
    {
        public int LookupId { get; set; }
        public string ShortTitle { get; set; }
        public string LongTitle { get; set; }
        public string LookupType { get; set; }
    }
}

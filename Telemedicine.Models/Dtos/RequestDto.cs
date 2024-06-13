using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos
{
    public class RequestDto<T>
    {
        public T Key { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos
{
    public class ResponseDto<T>
    {
        public int Status { get; set; } = 1;
        public string Message { get; set; } = "Success";
        public T Data { get; set; }
    }
}

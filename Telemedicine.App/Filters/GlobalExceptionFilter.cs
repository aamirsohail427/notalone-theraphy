using Telemedicine.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Telemedicine.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var response = new ResponseDto<string>()
            {
                Status = 0,
                Message = context.Exception.Message,
                Data = null
            };

            context.ExceptionHandled = true;
            context.Result = new JsonResult(response);
        }
    }
}

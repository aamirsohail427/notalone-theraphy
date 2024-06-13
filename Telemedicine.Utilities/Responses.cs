using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Telemedicine.Models.Dtos;

namespace Telemedicine.Utilities
{
    public static class Responses
    {
        public static ResponseDto<dynamic> NotFound()
        {
            return new ResponseDto<dynamic>()
            {
                Status = Convert.ToInt32(HttpStatusCode.NotFound),
                Message = ResponseStrings.NotFound,
                Data = null
            };
        }

        public static ResponseDto<dynamic> Unauthorized()
        {
            return new ResponseDto<dynamic>()
            {
                Status = Convert.ToInt32(HttpStatusCode.Unauthorized),
                Message = ResponseStrings.Unauthorized,
                Data = null
            };
        }
        public static ResponseDto<T> OK<T>(string message, T data)
        {
            return new ResponseDto<T>()
            {
                Message = string.Format("{0}", message),
                Data = data
            };
        }

        public static ResponseDto<T> OKGet<T>(string message, T data)
        {
            return new ResponseDto<T>()
            {
                Message = string.Format("{0} get successfully", message),
                Data = data
            };
        }

        public static ResponseDto<T> OKGetAll<T>(string message, T data)
        {
            return new ResponseDto<T>()
            {
                Message = string.Format("{0}s get successfully", message),
                Data = data
            };
        }

        public static ResponseDto<T> OKAdded<T>(string message, T data)
        {
            return new ResponseDto<T>()
            {
                Message = string.Format("{0} added successfully", message),
                Data = data
            };
        }

        public static ResponseDto<T> OKUpdated<T>(string message, T data)
        {
            return new ResponseDto<T>()
            {
                Message = string.Format("{0} updated successfully", message),
                Data = data
            };
        }

        public static ResponseDto<T> OKDeleted<T>(string message, T data)
        {
            return new ResponseDto<T>()
            {
                Message = string.Format("{0} deleted successfully", message),
                Data = data
            };
        }

        public static ResponseDto<T> NotFound<T>(string message, T data)
        {
            return new ResponseDto<T>()
            {
                Status = Convert.ToInt32(HttpStatusCode.NotFound),
                Message = string.Format("This {0} not found in our database", message),
                Data = data
            };
        }

        public static ResponseDto<T> SomethingWentWrong<T>(string message, T data)
        {
            return new ResponseDto<T>()
            {
                Status = 0,
                Message = string.Format("Something went wrong <b>{0}</b>", message),
                Data = data
            };
        }
    }
}

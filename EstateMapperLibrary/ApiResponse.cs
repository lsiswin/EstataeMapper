using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateMapperLibrary.Models;

namespace EstateMapperLibrary
{
    public class ApiResponse<T>
    {
        public ApiResponse() { }

        public ApiResponse(ResultStatus status, T result, string message)
        {
            Status = status;
            Result = result;
            Message = message;
        }

        public ResultStatus Status { get; set; }
        public T Result { get; set; }

        public string Message { get; set; }
    }
    public class ApiResponse
    {
        public ApiResponse() { }

        public ApiResponse(ResultStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public ResultStatus Status { get; set; }
        public string Message { get; set; }
    }

}

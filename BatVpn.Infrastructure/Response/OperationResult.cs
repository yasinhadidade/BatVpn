using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace BatVpn.Infrastructure.Response
{
    public class OperationResult
    {
        public int StatusCode { get; set; }
        public bool IsSuccedded { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }


        public OperationResult Succedded()
        {
            IsSuccedded = true;
            StatusCode = 200;
            Message = "Operation Is Successfully Done";
            return this;
        }

        public OperationResult Failed(string message)
        {
            IsSuccedded = false;
            Message = message;
            return this;
        }
    }

    public class OperationResult<T>
    {
        public int StatusCode { get; set; }
        public bool IsSuccedded { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public T Data { get; set; }


        public OperationResult<T> Succedded(T data)
        {
            IsSuccedded = true;
            StatusCode = 200;
            Message = "Operation Is Successfully Done";
            Data = data;
            return this;
        }

        public OperationResult<T> Failed(string message)
        {
            IsSuccedded = false;
            Message = message;
            return this;
        }
        public OperationResult<T> Failed(string message, int statusCode)
        {
            IsSuccedded = false;
            Message = message;
            StatusCode = statusCode;
            return this;
        }
    }
}

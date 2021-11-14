using System;

namespace SubModels.Utilities
{
    public class HttpResponseException : Exception
    {
        public int StatusCode { get; set; } = 500;
        public object Content { get; set; }
       
    }
}

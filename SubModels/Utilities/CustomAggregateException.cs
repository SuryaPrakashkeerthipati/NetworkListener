using System;

namespace SubModels.Utilities
{
    public class CustomAggregateException : Exception
    {
        public int StatusCode { get; set; } = 500;
        public object Content { get; set; }
        public Exception Exception { get; set; }
    }
}

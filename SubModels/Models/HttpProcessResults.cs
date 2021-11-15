
using Newtonsoft.Json;
using System.Net;

namespace SubModels.Models
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class HttpProcessResults<T>
    {
        private T _result;
        public bool IsSuccess { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public T Result
        {
            get { return _result; }
            set
            {
                _result = value;
                IsSuccess = true;
            }
        }
        public string Message { get; set; }
    }
}

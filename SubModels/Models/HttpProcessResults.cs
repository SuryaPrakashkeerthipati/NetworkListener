
using Newtonsoft.Json;
using System.Net;

namespace SubModels.Models
{
    public class HttpProcessResults<T>
    {
        private T _result;

        public bool IsSuccess { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Result
        {
            get { return _result; }
            set
            {
                _result = value;
                IsSuccess = true;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

    }
}

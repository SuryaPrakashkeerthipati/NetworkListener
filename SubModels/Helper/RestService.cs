using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SubModels.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SubModels.Helper
{
    public class RestService : IRestService
    {

        private readonly ILogger<RestService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public RestService(ILogger<RestService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        /// <summary>
        ///#1.It's a generic get method and ability to fetch the data based on given uri and TResult
        ///#2.Used to communicate external service providers
        /// </summary>
        /// <typeparam name="TResult">i.e class like GeoIPModel,RdapModel,RdnsModel</typeparam>
        /// <param name="uri">string</param>
        /// <returns></returns>
        public async Task<HttpProcessResults<TResult>> GetExternalServiceAsync<TResult>(string uri)
        {
            HttpProcessResults<TResult> httpProcessResults = new HttpProcessResults<TResult>();
            var response = await HttpRequestAsync(new HttpRequestMessage(HttpMethod.Get, uri));
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<TResult>(content);
                httpProcessResults.Result = result;
                _logger.LogInformation(nameof(HttpRequestAsync), $"Executing call to {uri} .Response code: {response.StatusCode}");
            }

            if (response.StatusCode == HttpStatusCode.RequestTimeout || response.StatusCode == HttpStatusCode.GatewayTimeout)
            {
                _logger.LogWarning(nameof(HttpRequestAsync), $"Timeout({response.StatusCode}) when calling {uri} . Retrying...");
                response = await HttpRequestAsync(new HttpRequestMessage(HttpMethod.Get, uri));
            }

            if (!response.IsSuccessStatusCode)
            {
                httpProcessResults.Message = content;
                _logger.LogError(nameof(HttpRequestAsync), $"Error when executing call to {uri}.Response code: {response.StatusCode}, response :{content}");
            }

            httpProcessResults.HttpStatusCode = response.StatusCode;
            httpProcessResults.IsSuccess = response.IsSuccessStatusCode;

            return httpProcessResults;
        }
        /// <summary>
        ///#1.It's a generic get method and ability to fetch the data based on given uri and TResult
        ///#2.Used to communicate internal api's 
        /// .i,e NetworkListner.Api --> GeoIp.Api 
        ///      NetworkListner.Api --> Rdap.Api 
        ///      NetworkListner.Api --> Rdns.Api 
        ///      NetworkListner.Api --> Ping.Api 
        /// </summary>
        /// <typeparam name="TResult">i.e class like GeoIPModel,RdapModel,RdnsModel</typeparam>
        /// <param name="uri">string</param>
        /// <returns></returns>
        public async Task<HttpProcessResults<TResult>> GetInternalServiceAsync<TResult>(string uri)
        {
            HttpProcessResults<TResult> httpProcessResults = new HttpProcessResults<TResult>();
            var response = await HttpRequestAsync(new HttpRequestMessage(HttpMethod.Get, uri));
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var httpResults = JsonConvert.DeserializeObject<HttpProcessResults<TResult>>(content);
                httpProcessResults.Result = httpResults.Result;
                _logger.LogInformation(nameof(HttpRequestAsync), $"Executing call to {uri} .Response code: {response.StatusCode}");
            }

            if (response.StatusCode == HttpStatusCode.RequestTimeout || response.StatusCode == HttpStatusCode.GatewayTimeout)
            {
                _logger.LogWarning(nameof(HttpRequestAsync), $"Timeout({response.StatusCode}) when calling {uri} . Retrying...");
                response = await HttpRequestAsync(new HttpRequestMessage(HttpMethod.Get, uri));
            }

            if (!response.IsSuccessStatusCode)
            {
                httpProcessResults.Message = content;
                _logger.LogError(nameof(HttpRequestAsync), $"Error when executing call to {uri}.Response code: {response.StatusCode}, response :{content}");
            }

            httpProcessResults.HttpStatusCode = response.StatusCode;
            httpProcessResults.IsSuccess = response.IsSuccessStatusCode;

            return httpProcessResults;
        }

        public async Task<string> GetAsync(string uri)
        {
            var response = await HttpRequestAsync(new HttpRequestMessage(HttpMethod.Get, uri));
            return await response.Content.ReadAsStringAsync();
        }
        private async Task<HttpResponseMessage> HttpRequestAsync(HttpRequestMessage requestMessage)
        {

            var response = await HttpRequestSendAsync(requestMessage);

            return response;
        }
        private async Task<HttpResponseMessage> HttpRequestSendAsync(HttpRequestMessage requestMessage)
        {
            HttpResponseMessage httpResponseMessage = null;
            try
            {
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.Timeout = TimeSpan.FromSeconds(1000);

                    httpResponseMessage = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex?.Message.ToString())
                };
            }
            return httpResponseMessage;
        }
    }
}

using Microsoft.Extensions.Caching.Abstractions;
using Microsoft.Extensions.Caching.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Opt.Saga.Core
{
    public interface ISagaHttpClient : IDisposable
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
        Task<HttpResponseMessage> GetAsync(HttpRequestMessage request);
    }
    public class SagaHttpClient : ISagaHttpClient
    {
        private InMemoryCacheHandler handler;

        public HttpClient HttpClient { get; set; }
        public SagaHttpClient()
        {
            var httpClientHandler = new HttpClientHandler();
            var cacheExpirationPerHttpResponseCode = CacheExpirationProvider.CreateSimple(TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
            handler = new InMemoryCacheHandler(httpClientHandler, cacheExpirationPerHttpResponseCode);
            HttpClient = new HttpClient(handler);
        }
        public async Task<HttpResponseMessage> GetAsync(HttpRequestMessage request)
        {
            var httpResponse = await HttpClient.SendAsync((request));

            return httpResponse;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {

            var httpResponse = await HttpClient.SendAsync((request));
            string content = await httpResponse.Content.ReadAsStringAsync();

            return httpResponse;

        }

        public HttpRequestMessage CreateRequest(SagaHttpClientRequest request)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage
            {
                Content = new StringContent(request.Content, Encoding.UTF8, request.ContentType),
                Method = request.Method,
                RequestUri = request.EndpointUri,
            };
            return requestMessage;
        }

        public void Dispose()
        {
            HttpClient.Dispose();
        }

        internal void AddDefaultRequestHeaders(string v, string transactionId)
        {
            if (!this.HttpClient.DefaultRequestHeaders.Contains(v))
                this.HttpClient.DefaultRequestHeaders.Add(v, transactionId);
        }
    }
    public class SagaHttpClientRequest
    {
        public string TransactionId { get; set; }
        public HttpMethod Method { get; set; }
        public string Content { get; set; }
        public Uri EndpointUri { get; set; }
        public string ContentType { get; set; }
    }
    public class SagaHttpClientResponse
    {
        public SagaHttpClientRequest Request { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
        public bool IsSuccessStatusCode => StatusCode == HttpStatusCode.OK;

    }
}

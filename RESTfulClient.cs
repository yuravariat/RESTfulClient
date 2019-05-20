using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RESTfulClient
{
    public class RestClient
    {
        private string baseUrl;
        public string BaseUrl
        {
            get
            {
                return baseUrl ?? string.Empty;
            }
            set
            {
                baseUrl = value;
            }
        }
        public TimeSpan? Timeout { get; set; }
        public List<Cookie> Cookies;
        public NetworkCredential Credentials;
        public ILogger logger;
        public WebProxy Proxy { get; set; }
        public string RequestUserAgent { get; set; }
        public string ClientSourceName { get; set; }

        public RestClient(ILogger logger)
        {
            this.logger = logger;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.DefaultConnectionLimit = 10000;
        }

        public TResponse Post<TResponse>(IReturn<TResponse> request, string path = null)
        {
            string url = BaseUrl.TrimEnd('/') + "/" + (path ?? request.GetPath().TrimStart('/'));
            var httprequest = (HttpWebRequest)WebRequest.Create(url);
            httprequest.Method = "POST";

            try
            {
                AddCommonHeaders(httprequest);

                SetTimeoutProxyAndCookies(httprequest);

                byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
                httprequest.ContentLength = data.Length;

#if DEBUG
                var requestId = Guid.NewGuid().ToString();
                System.Diagnostics.Debug.WriteLine($"{requestId} - {httprequest.Method} {url} request=> {JsonConvert.SerializeObject(request)}", "RestClient");
#endif

                using (Stream stream = httprequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length); // Sending the data
                }

                string responseStr = string.Empty;

                using (HttpWebResponse response = (HttpWebResponse)httprequest.GetResponse())
                {
                    CollectCookies(response);
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader readStream = new StreamReader(responseStream, Encoding.GetEncoding(Encoding.UTF8.CodePage)))
                        {
                            responseStr = readStream.ReadToEnd();

#if DEBUG
                            System.Diagnostics.Debug.WriteLine($"{requestId} - {httprequest.Method} {url} response=> {responseStr}", "RestClient");
#endif

                            readStream.Close();
                        }
                        responseStream.Close();
                    }
                    response.Close();
                }
                return JsonConvert.DeserializeObject<TResponse>(responseStr);
            }
            catch (Exception ex)
            {
                logger.WriteLog("RestClient error", ex, Level.Error);
            }
            return default(TResponse);
        }
        public async Task<TResponse> PostAsync<TResponse>(IReturn<TResponse> request, string path = null)
        {
            string url = BaseUrl.TrimEnd('/') + "/" + (path ?? request.GetPath()).TrimStart('/');
            var httprequest = (HttpWebRequest)WebRequest.Create(url);
            httprequest.Method = "POST";

            try
            {
                AddCommonHeaders(httprequest);

                SetTimeoutProxyAndCookies(httprequest);

                byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
                httprequest.ContentLength = data.Length;

#if DEBUG
                var requestId = Guid.NewGuid().ToString();
                System.Diagnostics.Debug.WriteLine($"{requestId} - {httprequest.Method} {url} request=> {JsonConvert.SerializeObject(request)}", "RestClient");
#endif

                using (Stream stream = await httprequest.GetRequestStreamAsync())
                {
                    await stream.WriteAsync(data, 0, data.Length); // Sending the data
                }

                string responseStr = string.Empty;

                using (HttpWebResponse response = (HttpWebResponse)(await httprequest.GetResponseAsync()))
                {
                    CollectCookies(response);
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader readStream = new StreamReader(responseStream, Encoding.GetEncoding(Encoding.UTF8.CodePage)))
                        {
                            responseStr = await readStream.ReadToEndAsync();
#if DEBUG
                            System.Diagnostics.Debug.WriteLine($"{requestId} - {httprequest.Method} {url} response=> {responseStr}", "RestClient");
#endif
                            readStream.Close();
                        }
                        responseStream.Close();
                    }
                    response.Close();
                }
                return JsonConvert.DeserializeObject<TResponse>(responseStr);
            }
            catch (Exception ex)
            {
                logger.WriteLog("RestClient error", ex, Level.Error);
            }
            return default(TResponse);
        }
        public TResponse Get<TResponse>(IReturn<TResponse> request, string path = null)
        {
            string url = BaseUrl.TrimEnd('/') + "/" + (path ?? request.GetPath()).TrimStart('/');
            var httprequest = (HttpWebRequest)WebRequest.Create(url);
            httprequest.Method = "GET";

            try
            {
                AddCommonHeaders(httprequest);

                SetTimeoutProxyAndCookies(httprequest);

                string responseStr = string.Empty;

#if DEBUG
                var requestId = Guid.NewGuid().ToString();
                System.Diagnostics.Debug.WriteLine($"{requestId} - {httprequest.Method} {url}", "RestClient");
#endif

                using (HttpWebResponse response = (HttpWebResponse)httprequest.GetResponse())
                {
                    CollectCookies(response);
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader readStream = new StreamReader(responseStream, Encoding.GetEncoding(Encoding.UTF8.CodePage)))
                        {
                            responseStr = readStream.ReadToEnd();
#if DEBUG
                            System.Diagnostics.Debug.WriteLine($"{requestId} - {httprequest.Method} {url} response=> {responseStr}", "RestClient");
#endif
                            readStream.Close();
                        }
                        responseStream.Close();
                    }
                    response.Close();
                }
                return JsonConvert.DeserializeObject<TResponse>(responseStr);
            }
            catch (Exception ex)
            {
                logger.WriteLog("RestClient error", ex, Level.Error);
            }
            return default(TResponse);
        }
        public async Task<TResponse> GetAsync<TResponse>(IReturn<TResponse> request, string path = null)
        {
            string url = BaseUrl.TrimEnd('/') + "/" + (path ?? request.GetPath()).TrimStart('/');
            var httprequest = (HttpWebRequest)WebRequest.Create(url);
            httprequest.Method = "GET";

            try
            {
                AddCommonHeaders(httprequest);

                SetTimeoutProxyAndCookies(httprequest);

                string responseStr = string.Empty;

#if DEBUG
                var requestId = Guid.NewGuid().ToString();
                System.Diagnostics.Debug.WriteLine($"{requestId} - {httprequest.Method} {url}", "RestClient");
#endif

                using (HttpWebResponse response = (HttpWebResponse)(await httprequest.GetResponseAsync()))
                {
                    CollectCookies(response);
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader readStream = new StreamReader(responseStream, Encoding.GetEncoding(Encoding.UTF8.CodePage)))
                        {
                            responseStr = await readStream.ReadToEndAsync();
#if DEBUG
                            System.Diagnostics.Debug.WriteLine($"{requestId} - {httprequest.Method} {url} response=> {responseStr}", "RestClient");
#endif
                            readStream.Close();
                        }
                        responseStream.Close();
                    }
                    response.Close();
                }
                return JsonConvert.DeserializeObject<TResponse>(responseStr);
            }
            catch (Exception ex)
            {
                logger.WriteLog("RestClient error", ex, Level.Error);
            }
            return default(TResponse);
        }
        private void AddCommonHeaders(HttpWebRequest request)
        {
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            request.AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);
            request.ContentType = "application/json";
            request.Accept = request.ContentType;
            if (!string.IsNullOrEmpty(RequestUserAgent))
            {
                request.UserAgent = RequestUserAgent;
            }
            if (!string.IsNullOrEmpty(ClientSourceName))
            {
                request.Headers.Add("client", ClientSourceName);
            }
            //request.KeepAlive = true;
        }
        private void SetTimeoutProxyAndCookies(HttpWebRequest request)
        {
            if (Timeout.HasValue && Timeout.Value > TimeSpan.FromSeconds(10))
            {
                request.Timeout = (int)Timeout.Value.TotalMilliseconds;
            }
            if (Proxy != null)
            {
                request.Proxy = Proxy;
            }
            else
            {
                //httprequest.Proxy = null;
            }
            if (Cookies != null && Cookies.Count > 0)
            {
                CookieContainer cookiesContainer = new CookieContainer();
                foreach (var cookie in Cookies)
                {
                    cookiesContainer.Add(cookie);
                }
                request.CookieContainer = cookiesContainer;
            }
        }
        private void CollectCookies(HttpWebResponse response)
        {
            Cookies = new List<Cookie>();
            if (response.Cookies != null && response.Cookies.Count > 0)
            {
                for (int i = 0; i < response.Cookies.Count; i++)
                {
                    Cookies.Add(response.Cookies[i]);
                }
            }
        }
    }
}

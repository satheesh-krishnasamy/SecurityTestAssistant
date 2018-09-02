using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace SecurityTestAssistant.Library.Logic
{
    public class HttpResponseReceivedEventArgs : EventArgs
    {
        public HttpResponseHeaders ResponseHeaders { get; private set; }
        public string Url { get; private set; }

        public HttpResponseReceivedEventArgs(string url, HttpResponseHeaders headers)
        {
            this.Url = url;
            this.ResponseHeaders = headers;
        }
    }

    public delegate void OnHttpResponseReceived(object sender, HttpResponseReceivedEventArgs e);

    public interface IHttpResponseProvider
    {
        event OnHttpResponseReceived HttpResponseReceivedEvent;
    }

    public class HttpCaller: IHttpResponseProvider
    {
        private readonly SecurityProtocolType OriginalProtocol = ServicePointManager.SecurityProtocol;

        public event OnHttpResponseReceived HttpResponseReceivedEvent;

        public void MakeRequest(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3|
                SecurityProtocolType.Tls;
            var requestClient = new HttpClient();
            //c.BaseAddress = webBrowser1.Url;
            var result = requestClient.GetAsync(url).GetAwaiter().GetResult();

            if(this.HttpResponseReceivedEvent!= null)
            {
                this.HttpResponseReceivedEvent(this, new HttpResponseReceivedEventArgs(url, result.Headers));
            }
        }

        private bool TrustAllCertificate(object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}

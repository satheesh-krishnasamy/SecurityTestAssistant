using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTestAssistant.Library.Net
{
    public class HttpHeader
    {
        public HttpHeader(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class HttpCookie
    {
        public string Name { get; set; }
        public bool HttpOnly { get; set; }
        public bool IsSecure { get; set; }
        public DateTime LifeTime { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public string SameSite { get; internal set; }
        public string MaxAge { get; internal set; }
        public string Language { get; internal set; }
        public bool IsSessionCookie
        {
            get
            {
                return (this.LifeTime == DateTime.MinValue && string.IsNullOrWhiteSpace(this.MaxAge));
            }
        }
    }

    public enum UrlScheme
    {
        None = 0,
        Http = 1,
        Https = 2
    }

    public enum HttpMethod
    {
        None = 0,
        Get,
        Post,
        Put,
        Delete
    }

    public abstract class HttpRequestResponseBase
    {
        public HttpRequestResponseBase(
            string url,
            UrlScheme urlScheme,
            string httpMethod)
        {
            this.Url = url;
            this.UrlScheme = urlScheme;
            this.HttpMethod = httpMethod;
        }

        public string Url { get; private set; }
        public UrlScheme UrlScheme { get; private set; }
        public string HttpMethod { get; private set; }
        public IList<HttpHeader> Headers { get; private set; } = new List<HttpHeader>();
        public IList<HttpCookie> Cookies { get; private set; } = new List<HttpCookie>();

        public virtual IList<KeyValuePair<string, string>> GetAdditionalProperties()
        {
            var additionalProps = new List<KeyValuePair<string, string>>();
            additionalProps.Add(new KeyValuePair<string, string>("Url", this.Url));
            additionalProps.Add(new KeyValuePair<string, string>("UrlScheme", this.UrlScheme.ToString()));
            additionalProps.Add(new KeyValuePair<string, string>("HttpMethod", this.HttpMethod));


            return additionalProps;
        }
    }

    public class HttpRequest : HttpRequestResponseBase
    {
        public HttpRequest(string url, UrlScheme urlScheme, string httpMethod) : base(url, urlScheme, httpMethod)
        {
        }
    }

    public class HttpResponse : HttpRequestResponseBase
    {
        public HttpResponse(string url, UrlScheme urlScheme, string httpMethod) : base(url, urlScheme, httpMethod)
        {
        }

        public Encoding Encoding { get; internal set; }
    }


    public class HttpRequestRaisedEventArgs : EventArgs
    {
        public HttpRequest Request { get; private set; }

        public HttpRequestRaisedEventArgs(
            string url,
            UrlScheme urlScheme,
            string httpMethod)
        {
            this.Request = new HttpRequest(url, urlScheme, httpMethod);
        }
    }

    public class HttpResponseReceivedEventArgs2 : EventArgs
    {
        public HttpResponse Response { get; private set; }

        public HttpResponseReceivedEventArgs2(
            string url,
            UrlScheme urlScheme,
            string httpMethod)
        {
            this.Response = new HttpResponse(url, urlScheme, httpMethod);
        }
    }


    public delegate void HttpUrlRequesting(object sender, HttpRequestRaisedEventArgs e);
    public delegate void HttpResponseReceived(object sender, HttpResponseReceivedEventArgs2 e);


}

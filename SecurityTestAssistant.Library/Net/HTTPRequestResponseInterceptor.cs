namespace SecurityTestAssistant.Library.Net
{
    using SecurityTestAssistant.Library.Utils;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Titanium.Web.Proxy;
    using Titanium.Web.Proxy.EventArguments;
    using Titanium.Web.Proxy.Http;
    using Titanium.Web.Proxy.Models;

    public sealed class HTTPRequestResponseInterceptor : ISecurityTestHTTPTrafficListener, IDisposable
    {
        private IList<string> DomainNamesToBeWatched { get; set; }
        public string GenericCertificateName { get; set; }

        private readonly ProxyServer proxyServer;
        private ExplicitProxyEndPoint explicitEndPoint;


        public HTTPRequestResponseInterceptor() : this(new List<string>() { "localhost" }, "localhost")
        {

        }

        public HTTPRequestResponseInterceptor(
            IEnumerable<string> domainNamesToBeWatched,
            string genericCertificateName
            )
        {
            if (domainNamesToBeWatched == null)
            {
                this.DomainNamesToBeWatched = new List<string>();
            }
            else
            {
                this.DomainNamesToBeWatched = new List<string>();
                foreach (var domainName in domainNamesToBeWatched)
                {
                    this.ListenDomain(domainName);
                }
            }
            this.GenericCertificateName = GenericCertificateName;
            this.proxyServer = new ProxyServer();
        }

        public void DoNotListen(string domainName)
        {
            this.DomainNamesToBeWatched.Remove(domainName);
        }

        public void DoNotListenAllDomains()
        {
            this.DomainNamesToBeWatched.Clear();
        }

        public void ListenDomain(string domainName)
        {
            if (!this.DomainNamesToBeWatched.Contains(domainName))
                this.DomainNamesToBeWatched.Add(domainName);
        }

        public event HttpUrlRequesting OnHttpUrlRequested;
        public event HttpResponseReceived OnHttpResponseReceived;

        public void Dispose()
        {
            if (!this.proxyServer.ProxyRunning)
            {
                try
                {
                    this.StopListening();
                }
                catch { }
            }
            try { this.proxyServer.Dispose(); } catch { }
        }

        public void StartListening()
        {
            //locally trust root certificate used by this proxy 
            proxyServer.CertificateManager.TrustRootCertificate(true);

            //optionally set the Certificate Engine
            //Under Mono only BouncyCastle will be supported
            //proxyServer.CertificateManager.CertificateEngine = Network.CertificateEngine.BouncyCastle;

            proxyServer.BeforeRequest += OnRequest;
            proxyServer.BeforeResponse += OnResponse;
            proxyServer.ServerCertificateValidationCallback += OnCertificateValidation;
            proxyServer.ClientCertificateSelectionCallback += OnCertificateSelection;


            explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, 8000, true)
            {
                //Use self-issued generic certificate on all https requests
                //Optimizes performance by not creating a certificate for each https-enabled domain
                //Useful when certificate trust is not required by proxy clients
                //GenericCertificate = new X509Certificate2("wwwgooglecom.crt")  
                //GenericCertificate = new X509Certificate2(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "genericcert.pfx"), "password")
            };

            //Fired when a CONNECT request is received
            explicitEndPoint.BeforeTunnelConnectRequest += OnBeforeTunnelConnectRequest;
            explicitEndPoint.BeforeTunnelConnectResponse += OnBeforeTunnelConnectResponse;


            //An explicit endpoint is where the client knows about the existence of a proxy
            //So client sends request in a proxy friendly manner
            proxyServer.AddEndPoint(explicitEndPoint);
            proxyServer.Start();

            //Transparent endpoint is useful for reverse proxy (client is not aware of the existence of proxy)
            //A transparent endpoint usually requires a network router port forwarding HTTP(S) packets or DNS
            //to send data to this endPoint
            var transparentEndPoint = new TransparentProxyEndPoint(IPAddress.Any, 8001, true)
            {
                //Generic Certificate hostname to use
                //when SNI is disabled by client
                GenericCertificateName = this.GenericCertificateName
            };

            proxyServer.AddEndPoint(transparentEndPoint);

            //proxyServer.UpStreamHttpProxy = new ExternalProxy() { HostName = "localhost", Port = 8888 };
            //proxyServer.UpStreamHttpsProxy = new ExternalProxy() { HostName = "localhost", Port = 8888 };

            foreach (var endPoint in proxyServer.ProxyEndPoints)
                Console.WriteLine("Listening on '{0}' endpoint at Ip {1} and port: {2} ",
                    endPoint.GetType().Name, endPoint.IpAddress, endPoint.Port);

            //Only explicit proxies can be set as system proxy!
            proxyServer.SetAsSystemHttpProxy(explicitEndPoint);
            proxyServer.SetAsSystemHttpsProxy(explicitEndPoint);

            //wait here (You can use something else as a wait function, I am using this as a demo)
            //Console.Read();

            //Unsubscribe & Quit
        }

        public void StopListening()
        {
            explicitEndPoint.BeforeTunnelConnectRequest -= OnBeforeTunnelConnectRequest;
            explicitEndPoint.BeforeTunnelConnectResponse -= OnBeforeTunnelConnectResponse;

            proxyServer.BeforeRequest -= OnRequest;
            proxyServer.BeforeResponse -= OnResponse;
            proxyServer.ServerCertificateValidationCallback -= OnCertificateValidation;
            proxyServer.ClientCertificateSelectionCallback -= OnCertificateSelection;
            proxyServer.Stop();

        }

        #region OtherProxyServer event handlers
        private async Task OnBeforeTunnelConnectRequest(object sender, TunnelConnectSessionEventArgs e)
        {
            string hostname = e.WebSession.Request.RequestUri.Host;


            //Exclude Https addresses you don't want to proxy
            //Useful for clients that use certificate pinning
            //for example dropbox.com
            e.DecryptSsl = this.DomainNamesToBeWatched.Contains(hostname);

        }

        private Task OnBeforeTunnelConnectResponse(object sender, TunnelConnectSessionEventArgs e)
        {
            return Task.FromResult(false);
        }

        public async Task OnRequest(object sender, SessionEventArgs e)
        {
            //Console.WriteLine(e.WebSession.Request.Url);

            ////read request headers
            var requestHeaders = e.WebSession.Request.Headers;

            var method = e.WebSession.Request.Method.ToUpper();
            if ((method == "POST" || method == "PUT" || method == "PATCH"))
            {
                //Get/Set request body bytes
                byte[] bodyBytes = await e.GetRequestBody();
                e.SetRequestBody(bodyBytes);

                //Get/Set request body as string
                string bodyString = await e.GetRequestBodyAsString();
                e.SetRequestBodyString(bodyString);

                //store request 
                //so that you can find it from response handler 
                e.UserData = e.WebSession.Request;
            }

            if (this.OnHttpUrlRequested != null)
            {
                var requestEvent = new HttpRequestRaisedEventArgs(
                    e.WebSession.Request.Url,
                    e.WebSession.Request.IsHttps ? UrlScheme.Https : UrlScheme.Http,
                    e.WebSession.Request.Method);
                if (e.WebSession.Request.Headers != null)
                {
                    var cookies = "";

                    foreach (var hdr in e.WebSession.Request.Headers)
                    {
                        requestEvent.Request.Headers.Add(new Net.HttpHeader(hdr.Name, hdr.Value));

                        if (hdr.Name.ToLower() == "cookie")
                        {
                            cookies = hdr.Value;
                        }
                    }

                    var parsedCookieCollection = CookieParser.ParseRequestCookie(cookies);
                    foreach (var cookie in parsedCookieCollection)
                    {
                        requestEvent.Request.Cookies.Add(cookie);
                    }
                }

                this.OnHttpUrlRequested(this, requestEvent);
            }

            //To cancel a request with a custom HTML content
            //Filter URL
            //if (e.WebSession.Request.RequestUri.AbsoluteUri.Contains("googlezffdf.com"))
            //{
            //    e.Ok("<!DOCTYPE html>" +
            //          "<html><body><h1>" +
            //          "Website Blocked" +
            //          "</h1>" +
            //          "<p>Blocked by titanium web proxy.</p>" +
            //          "</body>" +
            //          "</html>");
            //}
            //Redirect example
            //if (e.WebSession.Request.RequestUri.AbsoluteUri.Contains("wikipedia.org"))
            //{
            //    e.Redirect("https://www.paypal.com");
            //}
        }

        //Modify response
        public async Task OnResponse(object sender, SessionEventArgs e)
        {
            //read response headers
            var responseHeaders = e.WebSession.Response.Headers;

            //Try removing the HSTS header
            //if (e.WebSession.Response.Headers.HeaderExists("Strict-Transport-Security"))
            //    e.WebSession.Response.Headers.RemoveHeader("Strict-Transport-Security");

            //if (!e.ProxySession.Request.Host.Equals("medeczane.sgk.gov.tr")) return;
            if (e.WebSession.Request.Method == "GET" || e.WebSession.Request.Method == "POST")
            {
                if (e.WebSession.Response.StatusCode == 200)
                {
                    if (e.WebSession.Response.ContentType != null && e.WebSession.Response.ContentType.Trim().ToLower().Contains("text/html"))
                    {
                        byte[] bodyBytes = await e.GetResponseBody();
                        //original: 
                        e.SetResponseBody(bodyBytes);

                        string body = await e.GetResponseBodyAsString();
                        //original: 
                        e.SetResponseBodyString(body);

                        //mine
                        //var response1 = new Response(bodyBytes);
                        //response1.StatusCode = e.WebSession.Response.StatusCode;
                        //response1.Headers.RemoveHeader("Strict-Transport-Security");
                        //response1.ContentType = e.WebSession.Response.ContentType;
                        //response1.KeepBody = e.WebSession.Response.KeepBody;
                        //response1.IsChunked = e.WebSession.Response.IsChunked;
                        //response1.ContentLength = e.WebSession.Response.ContentLength;
                        //response1.HttpVersion = e.WebSession.Response.HttpVersion;
                        //e.Respond(response1);
                    }
                }
            }

            if (e.UserData != null)
            {
                //access request from UserData property where we stored it in RequestHandler
                var request = (Request)e.UserData;
            }

            if (this.DomainNamesToBeWatched.Contains(e.WebSession.Request.RequestUri.Host))
            {
                if (this.OnHttpResponseReceived != null)
                {
                    var requestEvent = new HttpResponseReceivedEventArgs2(
                        e.WebSession.Request.Url,
                        e.WebSession.Request.IsHttps ? UrlScheme.Https : UrlScheme.Http,
                        e.WebSession.Request.Method);
                    if (e.WebSession.Response.Headers != null)
                    {
                        requestEvent.Response.Encoding = e.WebSession.Response.Encoding;

                        IList<string> cookies = new List<string>();
                        foreach (var hdr in e.WebSession.Response.Headers)
                        {
                            requestEvent.Response.Headers.Add(new Net.HttpHeader(hdr.Name, hdr.Value));

                            if (hdr.Name.ToLower() == "set-cookie")
                            {
                                cookies.Add(hdr.Value);
                            }
                        }

                        var parsedCookieCollection = CookieParser.ParseResponseCookie(cookies);
                        foreach (var cookie in parsedCookieCollection)
                        {
                            requestEvent.Response.Cookies.Add(cookie);
                        }
                    }

                    this.OnHttpResponseReceived(this, requestEvent);
                }
            }
        }

        /// Allows overriding default certificate validation logic
        public Task OnCertificateValidation(object sender, CertificateValidationEventArgs e)
        {
            //set IsValid to true/false based on Certificate Errors
            if (e.SslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
                e.IsValid = true;

            e.IsValid = true;

            return Task.FromResult(0);
        }

        /// Allows overriding default client certificate selection logic during mutual authentication
        public Task OnCertificateSelection(object sender, CertificateSelectionEventArgs e)
        {
            //set e.clientCertificate to override
            return Task.FromResult(0);
        }
        #endregion OtherProxyServer event handlers
    }
}

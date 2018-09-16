namespace SecurityTestAssistant.Library.Testers.Implementation
{
    using SecurityTestAssistant.Library.Config;
    using SecurityTestAssistant.Library.Models;
    using SecurityTestAssistant.Library.Net;

    /// <summary>
    /// To check whether the cookies in the HTTP response are set with the "HttpOnly" attribute
    /// </summary>
    /// <seealso cref="SecurityTestAssistant.Library.Testers.Implementation.SecurityTesterBase" />
    public class HttpOnlyResponseCookieTester : SecurityTesterBase
    {
        private readonly IHttpOnlyCookieTesterConfig Config;

        public HttpOnlyResponseCookieTester(IHttpOnlyCookieTesterConfig config)
        {
            this.Config = config;
        }

        public override void AnalyseHttpResponse(object sender, HttpResponseReceivedEventArgs2 responseEvent)
        {
            if (responseEvent.Response == null)
                return;

            foreach (var cki in responseEvent.Response.Cookies)
            {
                this.CheckForMissingHttpOnlyAttribute(responseEvent.Response, cki);
            }
        }

        private void CheckForMissingHttpOnlyAttribute(HttpResponse response, HttpCookie cki)
        {
            if (!cki.HttpOnly)
            {
                base.AddResult(new AnalysisResult(
                    $"{cki.Name} : Cookie must be secure if it is not intended to send via unsecure Http channel.",
                    SeverityType.Warning,
                    $"Review and apply httponly attribute for the cookie {cki.Name}",
                    "Http cookie",
                    response.GetAdditionalProperties(),
                    this.Config.References.Urls["HttpOnlyCookie"]));
            }
        }
    }
}

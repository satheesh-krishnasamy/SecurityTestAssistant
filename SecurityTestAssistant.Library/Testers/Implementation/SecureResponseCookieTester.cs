namespace SecurityTestAssistant.Library.Testers.Implementation
{
    using SecurityTestAssistant.Library.Config;
    using SecurityTestAssistant.Library.Models;
    using SecurityTestAssistant.Library.Net;

    /// <summary>
    /// To check whether the cookies in the HTTP response are set with the "secure" attribute
    /// </summary>
    /// <seealso cref="SecurityTestAssistant.Library.Testers.Implementation.SecurityTesterBase" />
    public class SecureResponseCookieTester : SecurityTesterBase
    {
        private readonly ISecureResponseCookieTesterConfig Config;

        public SecureResponseCookieTester(ISecureResponseCookieTesterConfig config)
        {
            this.Config = config;
        }

        public override void AnalyseHttpResponse(object sender, HttpResponseReceivedEventArgs2 responseEvent)
        {
            if (responseEvent == null)
                return;
            var response = responseEvent.Response;

            foreach (var cki in response.Cookies)
            {
                this.CheckForMissingSecureAttribute(response, cki);
            }
        }

        private void CheckForMissingSecureAttribute(HttpResponse response, HttpCookie cki)
        {
            if (!cki.IsSecure)
            {

                base.AddResult(new AnalysisResult(
                    $"{cki.Name} : Cookie must be secure if it is not intended to send via unsecure Http channel.",
                    SeverityType.Warning,
                    $"Review and apply secure attribute for the cookie {cki.Name}",
                    "Secure cookie",
                    response.GetAdditionalProperties(),
                    this.Config.References.Urls["SecureCookieAttribute"]));
            }
        }

    }
}

namespace SecurityTestAssistant.Library.Testers.Implementation
{
    using SecurityTestAssistant.Library.Config;
    using SecurityTestAssistant.Library.Models;
    using SecurityTestAssistant.Library.Net;
    using SecurityTestAssistant.Library.Utils;
    
    /// <summary>
    /// Class verifies the server fingerprinting attack using the cookie names
    /// </summary>
    /// <seealso cref="SecurityTestAssistant.Library.Testers.Implementation.SecurityTesterBase" />
    public class ServerFingerprintingByCookieNameTester : SecurityTesterBase
    {
        private readonly IServerFingerprintingByCookieNameTesterConfig Config;

        public ServerFingerprintingByCookieNameTester(
            IServerFingerprintingByCookieNameTesterConfig config)
        {
            this.Config = config;
        }

        public override void AnalyseHttpResponse(object sender, HttpResponseReceivedEventArgs2 responseEvent)
        {
            if (responseEvent == null)
                return;

            foreach (var cki in responseEvent.Response.Cookies)
            {
                this.CheckCookieForServerFingerprinting(responseEvent.Response, cki);
            }

        }

        private void CheckCookieForServerFingerprinting(HttpResponse response, HttpCookie cki)
        {
            if (!string.IsNullOrWhiteSpace(cki.Name))
            {
                var cookieOfTechnology = PatternMatchUtil.CheckPatternMatch(cki.Name, this.Config.KnownTechCookiePatterns);
                if (cookieOfTechnology.MatchFound)
                {
                    this.AddResult(
                                new AnalysisResult(
                                    $"Cookie {cki.Name} discloses the technologies and programming languages used by the web application [Found Technology: {cookieOfTechnology.MatchingPattern.Technology}].",
                                    SeverityType.Error,
                                    "Change the default cookie name used by the web development framework to a generic name, such as -id",
                                    "Server fingerprinting",
                                    response.GetAdditionalProperties(),
                                    this.Config.References.Urls["SessionIDNameFingerprinting"]));
                }
            }
        }

    }
}

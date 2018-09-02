using SecurityTestAssistant.Library.Config;
using SecurityTestAssistant.Library.Models;
using SecurityTestAssistant.Library.Net;
using System;
using System.Collections.Generic;

namespace SecurityTestAssistant.Library.Testers.Implementation
{
    /// <summary>
    /// Used to test the cookies that start with "__secure-" for the recommended practices
    /// </summary>
    /// <seealso cref="SecurityTestAssistant.Library.Testers.Implementation.SecurityTesterBase" />
    public class CookieNameWithPrefixSecureTester : SecurityTesterBase
    {
        private readonly IResponseCookieNamePrefixTesterConfig Config;

        public CookieNameWithPrefixSecureTester(IResponseCookieNamePrefixTesterConfig config)
        {
            this.Config = config;
        }

        public override void AnalyseHttpResponse(object sender, HttpResponseReceivedEventArgs2 responseEvent)
        {
            var response = responseEvent.Response;
            if (response == null)
                return;

            foreach (var cki in response.Cookies)
            {
                this.CheckCookiePrefixes(response, cki);
            }
        }

        private void CheckCookiePrefixes(HttpResponse response, HttpCookie cki)
        {
            if (!string.IsNullOrWhiteSpace(cki.Name))
            {
                var isSecurePrefixedCookie = cki.Name.StartsWith("__secure-", StringComparison.InvariantCultureIgnoreCase);

                if (isSecurePrefixedCookie)
                {
                    if (!cki.IsSecure)
                    {

                        base.AddResult(new AnalysisResult(
                            $"{cki.Name} : Cookies names with the prefixes __Secure- can be used only if they are set with the \"secure\" attribute.",
                            FindingType.Warning,
                            $"Review and apply \"secure\" attribute for the cookie {cki.Name}",
                            "Cookie prefixes",
                            response.GetAdditionalProperties(),
                            this.Config.References.CookiePrefixes));
                    }

                    if (response.UrlScheme != UrlScheme.Https)
                    {
                        base.AddResult(new AnalysisResult(
                            $"{cki.Name} : Cookies names with the prefixes __Secure- can be used only if they are from a secure (HTTPS) origin.",
                            FindingType.Warning,
                            $"cookie {cki.Name} : Ensure the site is accessed ony via HTTPs.",
                            "Cookie prefixes",
                            response.GetAdditionalProperties(),
                            this.Config.References.CookiePrefixes));
                    }
                }
            }
        }
    }
}

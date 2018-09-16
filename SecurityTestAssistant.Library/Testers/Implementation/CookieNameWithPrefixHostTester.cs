using SecurityTestAssistant.Library.Config;
using SecurityTestAssistant.Library.Models;
using SecurityTestAssistant.Library.Net;
using System;
using System.Collections.Generic;

namespace SecurityTestAssistant.Library.Testers.Implementation
{
    /// <summary>
    /// Used to test the cookies that start with "__host-" for the recommended practices
    /// </summary>
    /// <seealso cref="SecurityTestAssistant.Library.Testers.Implementation.SecurityTesterBase" />
    public class CookieNameWithPrefixHostTester : SecurityTesterBase
    {
        private readonly IResponseCookieNamePrefixTesterConfig Config;

        public CookieNameWithPrefixHostTester(IResponseCookieNamePrefixTesterConfig config)
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
                var isHostPrefixedCookie = cki.Name.StartsWith("__host-", StringComparison.InvariantCultureIgnoreCase);

                if (isHostPrefixedCookie)
                {
                    if (!cki.IsSecure)
                    {

                        base.AddResult(new AnalysisResult(
                            $"{cki.Name} : Cookies names with the prefixes __Host- can be used only if they are set with the \"secure\" attribute.",
                            SeverityType.Warning,
                            $"Review and apply \"secure\" attribute for the cookie {cki.Name}",
                            "Cookie prefixes",
                            response.GetAdditionalProperties(),
                            this.Config.References.Urls["CookiePrefixes"]));
                    }

                    if (response.UrlScheme != UrlScheme.Https)
                    {
                        base.AddResult(new AnalysisResult(
                            $"{cki.Name} : Cookies names with the prefixes __Host- can be used only if they are from a secure (HTTPS) origin.",
                            SeverityType.Warning,
                            $"cookie {cki.Name} : Ensure the site is accessed ony via HTTPs.",
                            "Cookie prefixes",
                            response.GetAdditionalProperties(),
                            this.Config.References.Urls["CookiePrefixes"]));
                    }

                    if (cki.Path == null || !cki.Path.Equals("/"))
                    {
                        if (cki.Path == null || !cki.Path.Equals("/"))
                        {
                            base.AddResult(new AnalysisResult(
                                $"{cki.Name} : cookies with the __Host- prefix must have a path of '/' (the entire host) and must not have a domain attribute.",
                                SeverityType.Warning,
                                $"Review and apply \"path\" attribute for the cookie {cki.Name} and remove the \"domain\" attibute.",
                                "Cookie prefixes",
                                response.GetAdditionalProperties(),
                                this.Config.References.Urls["CookiePrefixes"]));
                        }
                    }
                }
            }
        }
    }
}

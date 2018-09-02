using SecurityTestAssistant.Library.Config;
using SecurityTestAssistant.Library.Models;
using SecurityTestAssistant.Library.Net;
using SecurityTestAssistant.Library.Utils;
using System.Collections.Generic;
using System.Text;

namespace SecurityTestAssistant.Library.Testers.Implementation
{
    /// <summary>
    /// Class verifies the length of the session id cookie value
    /// </summary>
    /// <seealso cref="SecurityTestAssistant.Library.Testers.Implementation.SecurityTesterBase" />
    public class SessionIdLengthTester : SecurityTesterBase
    {
        private readonly ISessionIdLengthTesterConfig config;

        public SessionIdLengthTester(
            ISessionIdLengthTesterConfig config)
        {
            this.config = config;
        }

        public override void AnalyseHttpResponse(object sender, HttpResponseReceivedEventArgs2 responseEvent)
        {
            if (responseEvent == null || responseEvent.Response == null)
                return;

            var response = responseEvent.Response;

            foreach (var cki in response.Cookies)
            {
                this.CheckAllSessionIDCookies(cki, response);
            }
        }

        private void CheckAllSessionIDCookies(HttpCookie cki, HttpResponse response)
        {
            var result = PatternMatchUtil.CheckPatternMatch(cki.Name, this.config.KnownTechCookiePatterns);
            if (result.MatchFound)
            {
                this.CheckSessionIDForAcceptableLength(cki, response);
            }
        }

        private void CheckSessionIDForAcceptableLength(HttpCookie cookie, HttpResponse response)
        {
            if (!string.IsNullOrWhiteSpace(cookie.Value))
            {
                var sessionIDValue = cookie.Value.Trim();

                var encoding = response.Encoding ?? Encoding.Unicode;
                var sessionIdLengthInBytes = encoding.GetBytes(sessionIDValue).Length;
                if (sessionIdLengthInBytes < 16)
                {
                    this.AddResult(new AnalysisResult(
                        $"Cookie: {cookie.Name} has length {sessionIdLengthInBytes} bytes. The session ID length must be at least 128 bits (16 bytes) to avoid brute force attack.",
                        FindingType.Warning,
                        $"Use a lengthy value for cookie ({cookie.Name}). Refer the URL.",
                        "Session ID length",
                        response.GetAdditionalProperties(),
                        this.config.References.SessionIDLengthMustBeLong));
                }
            }
        }


    }
}

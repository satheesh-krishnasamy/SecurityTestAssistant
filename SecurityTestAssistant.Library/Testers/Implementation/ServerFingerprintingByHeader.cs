using SecurityTestAssistant.Library.Config;
using SecurityTestAssistant.Library.Models;
using SecurityTestAssistant.Library.Net;
using SecurityTestAssistant.Library.Utils;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SecurityTestAssistant.Library.Testers.Implementation
{
    /// <summary>
    /// Class verifies the server fingerprinting attack using the response headers
    /// </summary>
    /// <seealso cref="SecurityTestAssistant.Library.Testers.Implementation.SecurityTesterBase" />
    public class ServerFingerprintingByHeader : SecurityTesterBase
    {
        private readonly IServerFingerPrintingByHttpHeaderTesterConfig Config;

        public ServerFingerprintingByHeader(IServerFingerPrintingByHttpHeaderTesterConfig config)
        {
            this.Config = config;
        }

        public override void AnalyseHttpResponse(object sender, HttpResponseReceivedEventArgs2 responseEvent)
        {
            var response = responseEvent.Response;

            if (response == null || response.Headers == null || response.Headers.Count < 1)
                return;

            foreach (var hdr in response.Headers)
            {
                foreach (var hdrPattern in this.Config.KnownServerHeaderValues)
                {
                    this.CheckServerFingerprintingByHeader(response, hdr, hdrPattern);
                }
            }
        }


        private void CheckServerFingerprintingByHeader(HttpResponse response, HttpHeader hdr, ServerHeaderValuePattern serverHdrValue)
        {
            if (!string.IsNullOrWhiteSpace(hdr.Name) && hdr.Name.Equals("server", StringComparison.InvariantCultureIgnoreCase))
            {
                if (PatternComparator.AreMacthesFound(hdr.Value, serverHdrValue))
                {
                    this.AddResult(
                                new AnalysisResult(
                                    $"Header {hdr.Name} discloses the server used by the web application [Found server: {serverHdrValue.ServerOrTechnologyName}].",
                                    SeverityType.Error,
                                    "Remove the header or change the value for this header to some other value that does not say anything about the server.",
                                    "Server fingerprinting",
                                    response.GetAdditionalProperties(),
                                    this.Config.References.ServerHeader));

                }
            }
        }

    }
}

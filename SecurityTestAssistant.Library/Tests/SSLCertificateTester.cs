namespace SecurityTestAssistant.Library.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;

    public class SSLCertificateTester
    {
        private readonly SecurityProtocolType OriginalProtocol = ServicePointManager.SecurityProtocol;

        private class ConnectionTestResults
        {
            internal bool ConnectionSuccess { get; set; }
            internal bool SSLConnectionFailure { get; set; }
        }

        public IEnumerable<string> Test(string url)
        {
            try
            {
                var testResults = new List<string>();
                ServicePointManager.ServerCertificateValidationCallback += CertVal;

                {
                    var ssl3ConnResult = IsAccessible(url, SecurityProtocolType.Ssl3);
                    if (!ssl3ConnResult.ConnectionSuccess && ssl3ConnResult.SSLConnectionFailure)
                    {
                        testResults.Add($"Good: {url} does not support transort layer security protocol - SSL3.");
                    }
                    if (ssl3ConnResult.ConnectionSuccess)
                    {
                        testResults.Add($"Bad: {url} supports transort layer security protocol - SSL3.");
                    }
                }

                {
                    var tlsConnResult = IsAccessible(url, SecurityProtocolType.Tls);
                    if (!tlsConnResult.ConnectionSuccess && tlsConnResult.SSLConnectionFailure)
                    {
                        testResults.Add($"Good: {url} does not support transort layer security protocol - Tls.");
                    }
                    if (tlsConnResult.ConnectionSuccess)
                    {
                        testResults.Add($"Bad: {url} supports transort layer security protocol - Tls.");
                    }
                }

                {
                    var tls11ConnResult = IsAccessible(url, SecurityProtocolType.Tls11);
                    if (!tls11ConnResult.ConnectionSuccess && tls11ConnResult.SSLConnectionFailure)
                    {
                        testResults.Add($"Good: {url} does not support transort layer security protocol - Tls1.1.");
                    }
                    if (tls11ConnResult.ConnectionSuccess)
                    {
                        testResults.Add($"Bad: {url} supports transort layer security protocol - Tls1.1.");
                    }
                }

                {
                    var tls12ConnResult = IsAccessible(url, SecurityProtocolType.Tls11);
                    if (!tls12ConnResult.ConnectionSuccess && tls12ConnResult.SSLConnectionFailure)
                    {
                        testResults.Add($"BAD: {url} does not support transort layer security protocol - Tls1.2.");
                    }

                    if (tls12ConnResult.ConnectionSuccess)
                    {
                        testResults.Add($"Good: {url} supports transort layer security protocol - Tls1.2.");
                    }
                }

                return testResults;
            }
            finally
            {
                ServicePointManager.SecurityProtocol = OriginalProtocol;
                ServicePointManager.ServerCertificateValidationCallback -= CertVal;
            }
        }

        private ConnectionTestResults IsAccessible(string url, SecurityProtocolType spType)
        {
            ConnectionTestResults connResult = new ConnectionTestResults();
            connResult.ConnectionSuccess = false;
            connResult.SSLConnectionFailure = false;

            HttpClient c = null;
            try
            {
                ServicePointManager.SecurityProtocol = spType;
                c = new HttpClient();
                //c.BaseAddress = webBrowser1.Url;
                var result = c.GetAsync(url).GetAwaiter().GetResult();
                connResult.ConnectionSuccess = true;
            }
            catch (HttpRequestException exp)
            {
                if (exp.InnerException != null)
                {
                    var secureConnExp = exp.InnerException as WebException;
                    if (secureConnExp != null && secureConnExp.Status == WebExceptionStatus.SecureChannelFailure)
                    {
                        connResult.SSLConnectionFailure = true;
                    }
                }

            }
            finally
            {
                if (c != null)
                    c.Dispose();
            }

            return connResult;
        }

        private bool CertVal(object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}

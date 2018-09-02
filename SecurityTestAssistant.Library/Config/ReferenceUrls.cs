using System.Collections;
using System.Collections.Generic;

namespace SecurityTestAssistant.Library.Tests.Config
{
    public class ReferenceUrls
    {

        public ReferenceUrls()
        {
            SecureCookieAttribute = new List<string>{
            "https://www.owasp.org/index.php/Session_Management_Cheat_Sheet#Secure_Attribute",
            "https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Set-Cookie#Compatibility_notes"};

            this.HttpOnlyCookie = new List<string>{
            "https://www.owasp.org/index.php/Session_Management_Cheat_Sheet#HttpOnly_Attribute" };

            this.CookiePrefixes = new List<string>
            {
                "https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Set-Cookie#Cookie_prefixes"
            };

            this.SessionIDNameFingerprinting = new List<string>
            {
                "https://www.owasp.org/index.php/Session_Management_Cheat_Sheet#Session_ID_Name_Fingerprinting"
            };

            this.SessionIDLengthMustBeLong = new List<string>()
            {
                "https://www.owasp.org/index.php/Session_Management_Cheat_Sheet#Session_ID_Length"
            };

            this.ServerHeader = new List<string>()
            {
                "https://www.owasp.org/index.php/Fingerprint_Web_Server_(OTG-INFO-002)",
            };
        }

        public IReadOnlyList<string> SecureCookieAttribute { get; private set; }

        public IReadOnlyList<string> HttpOnlyCookie { get; private set; }
        public IReadOnlyList<string> CookiePrefixes { get; private set; }
        public IReadOnlyList<string> SessionIDNameFingerprinting { get; private set; }
        public IEnumerable<string> SessionIDLengthMustBeLong { get; internal set; }
        public IEnumerable<string> ServerHeader { get; internal set; }
    }
}
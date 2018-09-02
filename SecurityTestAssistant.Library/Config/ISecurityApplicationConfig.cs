namespace SecurityTestAssistant.Library.Config
{
    using SecurityTestAssistant.Library.Tests.Config;
    using System.Collections.Generic;

    public interface ISecurityApplicationConfig 
        : IHttpOnlyCookieTesterConfig, 
        IServerFingerPrintingByHttpHeaderTesterConfig,
        IServerFingerprintingByCookieNameTesterConfig,
        IResponseCookieNamePrefixTesterConfig,
        ISessionIdLengthTesterConfig,
        ISecureResponseCookieTesterConfig
    {
    }

    public interface ISecurityTesterConfigBase
    {
        ReferenceUrls References { get; }
    }

    public interface IServerFingerPrintingByHttpHeaderTesterConfig : ISecurityTesterConfigBase
    {
        IList<ServerHeaderValuePattern> KnownServerHeaderValues { get; }
    }

    public interface IServerFingerprintingByCookieNameTesterConfig : ISecurityTesterConfigBase
    {
        IList<StringPattern> KnownTechCookiePatterns { get; }
    }

    public interface ISessionIdLengthTesterConfig : ISecurityTesterConfigBase
    {
        IList<StringPattern> KnownTechCookiePatterns { get; }
    }

    public interface IResponseCookieNamePrefixTesterConfig : ISecurityTesterConfigBase
    {
    }

    public interface IHttpOnlyCookieTesterConfig : ISecurityTesterConfigBase
    {
    }

    public interface ISecureResponseCookieTesterConfig : ISecurityTesterConfigBase
    {
    }

    public class SecurityApplicationConfig : ISecurityApplicationConfig
    {

        public SecurityApplicationConfig()
        {
            KnownTechCookiePatterns = new List<StringPattern>
            {
                new StringPattern("PHPSESSID", PatternMatchType.StartsWith, "PHP", "Session ID cookie"),
                new StringPattern("JSESSIONID", PatternMatchType.StartsWith, "J2EE", "Session ID cookie"),
                new StringPattern("CFID", PatternMatchType.StartsWith, "ColdFusion", "Session ID cookie"),
                new StringPattern("CFTOKEN", PatternMatchType.StartsWith, "ColdFusion", "Session ID cookie"),
                new StringPattern("ASP.NET_SessionId", PatternMatchType.StartsWith, "ASP.NET", "Session ID cookie"),
                new StringPattern(".AspNetCore.Session", PatternMatchType.StartsWith, "ASP.NET Core", "Session ID cookie"),
                new StringPattern(".AspNetCore.Antiforgery.", PatternMatchType.StartsWith, "Asp.Net Core", "Anti-forgery cookie name"),
            };

            KnownServerHeaderValues = new List<ServerHeaderValuePattern>
            {
                new ServerHeaderValuePattern("IIS", "(?i)(?<= |^)(microsoft|IIS)(?= |$)", PatternMatchType.RegEx),
                new ServerHeaderValuePattern("Kestrel", "(?i)(?<= |^)(Kestrel)(?= |$)", PatternMatchType.RegEx),
                new ServerHeaderValuePattern("Apache", "(?i)(?<= |^)(apache)(?= |$)", PatternMatchType.RegEx),
                new ServerHeaderValuePattern("NginX", "(?i)(?<= |^)(nginx)(?= |$)", PatternMatchType.RegEx)
            };
        }

        public IList<ServerHeaderValuePattern> KnownServerHeaderValues { get; private set; }
        public ReferenceUrls References { get; set; } = new ReferenceUrls();
        public IList<StringPattern> KnownTechCookiePatterns { get; private set; }
    }
}

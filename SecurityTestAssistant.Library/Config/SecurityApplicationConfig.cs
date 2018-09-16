namespace SecurityTestAssistant.Library.Config
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SecurityTestAssistant.Library.Tests.Config;
    using System.Collections.Generic;
    using System.IO;

    public interface ISecurityApplicationConfig
        : IHttpOnlyCookieTesterConfig,
        IServerFingerPrintingByHttpHeaderTesterConfig,
        IServerFingerprintingByCookieNameTesterConfig,
        IResponseCookieNamePrefixTesterConfig,
        ISessionIdLengthTesterConfig,
        ISecureResponseCookieTesterConfig,
        ISecurityTesterConfigBase
    {
    }

    public interface ISecurityTesterConfigBase
    {
        TestReference References { get; }
    }

    public interface IServerFingerPrintingByHttpHeaderTesterConfig : ISecurityTesterConfigBase
    {
        IEnumerable<ServerHeaderValuePattern> KnownServerHeaderValues { get; }
    }

    public interface IServerFingerprintingByCookieNameTesterConfig : ISecurityTesterConfigBase
    {
        IEnumerable<TechnologyStringPattern> KnownTechCookiePatterns { get; }
    }

    public interface ISessionIdLengthTesterConfig : ISecurityTesterConfigBase
    {
        IEnumerable<TechnologyStringPattern> KnownTechCookiePatterns { get; }
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

        public SecurityApplicationConfig(
            string referenceUrlsConfigFilePath,
            string knownServerHeadersConfigFilePath,
            string knownCookiePatternsConfigFilePath)
        {

            var configFileContent = File.ReadAllText(referenceUrlsConfigFilePath);
            this.References = (TestReference)JsonConvert.DeserializeObject(configFileContent, typeof(TestReference));

            configFileContent = File.ReadAllText(knownServerHeadersConfigFilePath);
            this.KnownServerHeaderValues = ParseJsonArrayIntoObject<ServerHeaderValuePattern>(configFileContent);

            configFileContent = File.ReadAllText(knownCookiePatternsConfigFilePath);
            this.KnownTechCookiePatterns = ParseJsonArrayIntoObject<TechnologyStringPattern>(configFileContent);
        }

        private IEnumerable<T> ParseJsonArrayIntoObject<T>(string fileContent)
        {
            JArray jsonResponse = JArray.Parse(fileContent);
            IList<T> result = new List<T>();

            foreach (var item in jsonResponse)
            {
                JObject rItemValueJson = (JObject)item;
                result.Add(rItemValueJson.ToObject<T>());
            }

            return result;
        }

        public IEnumerable<ServerHeaderValuePattern> KnownServerHeaderValues { get; private set; }
        public TestReference References { get; set; }
        public IEnumerable<TechnologyStringPattern> KnownTechCookiePatterns { get; private set; }        
    }
}

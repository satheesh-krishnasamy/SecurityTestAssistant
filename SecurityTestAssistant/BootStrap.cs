

namespace SecurityTestAssistant
{
    using SecurityTestAssistant.Library.Config;
    using SecurityTestAssistant.Library.Logic;
    //using SecurityTestAssistant.Library.Logic.Mapper;
    using SecurityTestAssistant.Library.Net;
    using SecurityTestAssistant.Library.Testers;
    using SecurityTestAssistant.Library.Testers.Implementation;
    using SimpleInjector;
    using System.IO;
    using System.Reflection;
    using System.Threading;

    internal static class Bootstrap
    {
        public static Container container;


        private static SecurityApplicationConfig GetConfig()
        {
            var configFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ConfigurableItems");
            var referencesConfigFile = Path.Combine(configFolder, "ReferenceUrlsConfig.json");
            var headersConfigFile = Path.Combine(configFolder, "KnownServerHeaders.json");
            var cookiesConfigFile = Path.Combine(configFolder, "KnownTechCookiePatterns.json");

            var configInstance = new SecurityApplicationConfig(referencesConfigFile, headersConfigFile, cookiesConfigFile);

            return configInstance;
        }

        public static void Start()
        {
            container = new Container();


            //var libraryDaoAutoMapper = LibraryDaoAutoMapper.InitializeDaoCoreModelMappings();
            var cancellationTokenSource = new CancellationTokenSource();
            container.Register(() => cancellationTokenSource, Lifestyle.Singleton);

            var analysisResultHandler = new AnalysisResultHandler();
            var configInstance = GetConfig();

            // Register your types, for instance:
            // container.Register<AutoMapper.IMapper>(() => libraryDaoAutoMapper, Lifestyle.Singleton);
            container.RegisterInstance<IApplicationReportDataHandler>(analysisResultHandler);

            container.RegisterInstance<ISecurityApplicationConfig>(configInstance);
            container.RegisterInstance<IHttpOnlyCookieTesterConfig>(configInstance);
            container.RegisterInstance<IServerFingerPrintingByHttpHeaderTesterConfig>(configInstance);
            container.RegisterInstance<IServerFingerprintingByCookieNameTesterConfig>(configInstance);
            container.RegisterInstance<IResponseCookieNamePrefixTesterConfig>(configInstance);
            container.RegisterInstance<ISessionIdLengthTesterConfig>(configInstance);
            container.RegisterInstance<ISecureResponseCookieTesterConfig>(configInstance);

            container.Collection.Register<IApplicationDataConsumer>(
                typeof(CookieTester));

            container.RegisterSingleton<ISecurityTestHTTPTrafficListener>(() => new HTTPRequestResponseInterceptor());

            container.Collection.Register<IResponseAnalyser>(
                typeof(SecureResponseCookieTester).Assembly
                //,
                //typeof(HttpOnlyResponseCookieTester),
                //typeof(ResponseCookiePrefixTester),
                //typeof(SecureResponseCookieTester),
                //typeof(ServerFingerprintingByHeader),
                //typeof(ServerFingerprintingBySessionIDCookieNameTester),
                //typeof(SessionIdLengthTester)
                );

            container.Register<IHttpResponseProvider, HttpCaller>();

            // Optionally verify the container.
            container.Verify();
        }
    }
}



namespace SecurityTestAssistant
{
    using SecurityTestAssistant.Library.Config;
    using SecurityTestAssistant.Library.Logic;
    //using SecurityTestAssistant.Library.Logic.Mapper;
    using SecurityTestAssistant.Library.Net;
    using SecurityTestAssistant.Library.Testers;
    using SecurityTestAssistant.Library.Testers.Implementation;
    using SimpleInjector;
    using System.Threading;

    internal static class Bootstrap
    {
        public static Container container;

        public static void Start()
        {
            container = new Container();


            //var libraryDaoAutoMapper = LibraryDaoAutoMapper.InitializeDaoCoreModelMappings();
            var cancellationTokenSource = new CancellationTokenSource();
            container.Register(() => cancellationTokenSource, Lifestyle.Singleton);

            var analysisResultHandler = new AnalysisResultHandler();

            // Register your types, for instance:
            // container.Register<AutoMapper.IMapper>(() => libraryDaoAutoMapper, Lifestyle.Singleton);
            container.RegisterInstance<IApplicationReportDataHandler>(analysisResultHandler);
            container.RegisterSingleton<ISecurityApplicationConfig, SecurityApplicationConfig>();


            container.RegisterSingleton<IHttpOnlyCookieTesterConfig, SecurityApplicationConfig>();
            container.RegisterSingleton<IServerFingerPrintingByHttpHeaderTesterConfig, SecurityApplicationConfig>();
            container.RegisterSingleton<IServerFingerprintingByCookieNameTesterConfig, SecurityApplicationConfig>();
            container.RegisterSingleton<IResponseCookieNamePrefixTesterConfig, SecurityApplicationConfig>();
            container.RegisterSingleton<ISessionIdLengthTesterConfig, SecurityApplicationConfig>();
            container.RegisterSingleton<ISecureResponseCookieTesterConfig, SecurityApplicationConfig>();

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

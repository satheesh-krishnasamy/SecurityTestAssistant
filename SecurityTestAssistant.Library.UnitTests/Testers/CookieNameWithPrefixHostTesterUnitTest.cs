namespace SecurityTestAssistant.Library.UnitTests.Net
{
    using FakeItEasy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SecurityTestAssistant.Library.Config;
    using SecurityTestAssistant.Library.Logic;
    using SecurityTestAssistant.Library.Models.Events;
    using SecurityTestAssistant.Library.Net;
    using SecurityTestAssistant.Library.Testers;
    using SecurityTestAssistant.Library.Testers.Implementation;
    using System.Linq;


    [TestClass]
    public class CookieNameWithPrefixHostTesterUnitTest
    {
        private HttpResponseReceivedEventArgs2 GetHttpResponseReceivedEventArgs2(UrlScheme scheme)
        {
            var myUrl = "https://www.mywebsite.com/testpage";
            return A.Fake<HttpResponseReceivedEventArgs2>(
                x => x.WithArgumentsForConstructor(() => new HttpResponseReceivedEventArgs2(
                myUrl,
                scheme,
                "Get"
            )));
        }

        [TestMethod]
        public void CookieNameWithPrefixHostTester_Should_Report_If___host_Cookies_No_Path_And_Secure_Attributes_Via_Http()
        {
            // Prepare the test
            var config = A.Fake<IResponseCookieNamePrefixTesterConfig>();
            IResponseAnalyser httpOnlyTester = new CookieNameWithPrefixHostTester(config);
            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            httpOnlyTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Use HTTPs.
            var requestEvent = this.GetHttpResponseReceivedEventArgs2(UrlScheme.Https);

            // __host cookie name without secure and path
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__host-Cookie1=SomeValue;"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__host-Cookie2=SomeValue; Path=;"));


            // should get errors. One for missing secure and path
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__host-Cookie1", Value = "SomeValue", HttpOnly = false, IsSecure = false, Path = null });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__host-Cookie2", Value = "SomeValue", HttpOnly = false, IsSecure = false, Path = "" });

            // Invoke the method being tested
            httpOnlyTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(httpOnlyTester.Results);
            Assert.IsNotNull(httpOnlyTester.Results.Count() == 4);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustHaveHappened(Repeated.Exactly.Times(4));
            A.CallTo(resultHolder).MustHaveHappened();
        }

        [TestMethod]
        public void CookieNameWithPrefixHostTester_Should_Report_If___host_Cookies_No_Path_And_Secure_Attributes_Via_Https()
        {
            // Prepare the test
            var config = A.Fake<IResponseCookieNamePrefixTesterConfig>();
            IResponseAnalyser httpOnlyTester = new CookieNameWithPrefixHostTester(config);
            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            httpOnlyTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Use HTTP.. it is an security error when using __host
            var requestEvent = this.GetHttpResponseReceivedEventArgs2(UrlScheme.Http);

            // __host cookie name without secure and path
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__host-Cookie1=SomeValue;"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__host-Cookie2=SomeValue; Path=;"));


            // should get errors. One for missing secure and path
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__host-Cookie1", Value = "SomeValue", HttpOnly = false, IsSecure = false, Path = null });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__host-Cookie2", Value = "SomeValue", HttpOnly = false, IsSecure = false, Path = "" });

            // Invoke the method being tested
            httpOnlyTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(httpOnlyTester.Results);
            Assert.IsNotNull(httpOnlyTester.Results.Count() == 6);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustHaveHappened(Repeated.Exactly.Times(6));
            A.CallTo(resultHolder).MustHaveHappened();
        }

        [TestMethod]
        public void CookieNameWithPrefixHostTester_Should_Report_If___host_Cookies_No_Path_But_Secure_Attributes_Via_Https()
        {
            // Prepare the test
            var config = A.Fake<IResponseCookieNamePrefixTesterConfig>();
            IResponseAnalyser httpOnlyTester = new CookieNameWithPrefixHostTester(config);
            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            httpOnlyTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Use HTTPs.
            var requestEvent = this.GetHttpResponseReceivedEventArgs2(UrlScheme.Https);

            // __host cookie name without secure and path
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__host-Cookie1=SomeValue; secure;"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__host-Cookie2=SomeValue; secure;Path=;"));

            // should get errors. One for missing secure and path
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__host-Cookie1", Value = "SomeValue", HttpOnly = false, IsSecure = true, Path = null });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__host-Cookie2", Value = "SomeValue", HttpOnly = false, IsSecure = true, Path = "" });


            // Invoke the method being tested
            httpOnlyTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(httpOnlyTester.Results);
            Assert.IsNotNull(httpOnlyTester.Results.Count() == 2);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustHaveHappened(Repeated.Exactly.Twice);
            A.CallTo(resultHolder).MustHaveHappened();
        }

        [TestMethod]
        public void CookieNameWithPrefixHostTester_Should_Pass_If___host_Cookies_With_Path_And_Secure_Attributes_Via_Https()
        {
            // Prepare the test
            var config = A.Fake<IResponseCookieNamePrefixTesterConfig>();
            IResponseAnalyser httpOnlyTester = new CookieNameWithPrefixHostTester(config);
            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            httpOnlyTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Use HTTPs.
            var requestEvent = this.GetHttpResponseReceivedEventArgs2(UrlScheme.Https);

            // __host cookie name without secure and path
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__host-Cookie1=SomeValue; secure; Path=/;"));

            // should get errors. One for missing secure and path
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__host-Cookie1", Value = "SomeValue", HttpOnly = false, IsSecure = true, Path = "/" });


            // Invoke the method being tested
            httpOnlyTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(httpOnlyTester.Results);
            Assert.IsNotNull(httpOnlyTester.Results.Count() == 0);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustNotHaveHappened();
            A.CallTo(resultHolder).MustNotHaveHappened();
        }

        [TestMethod]
        public void CookieNameWithPrefixHostTester_Should_Report_If___host_Cookies_With_Path_And_Secure_Attributes_Via_Http()
        {
            // Prepare the test
            var config = A.Fake<IResponseCookieNamePrefixTesterConfig>();
            IResponseAnalyser httpOnlyTester = new CookieNameWithPrefixHostTester(config);
            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            httpOnlyTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Use HTTP. we should get ERROR because of this.
            var requestEvent = this.GetHttpResponseReceivedEventArgs2(UrlScheme.Http);

            // __host cookie name without secure and path
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__host-Cookie1=SomeValue; secure; Path=/;"));

            // should get errors. One for missing secure and path
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__host-Cookie1", Value = "SomeValue", HttpOnly = false, IsSecure = true, Path = "/" });


            // Invoke the method being tested
            httpOnlyTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(httpOnlyTester.Results);
            Assert.IsNotNull(httpOnlyTester.Results.Count() == 1);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(resultHolder).MustHaveHappened();
        }

        [TestMethod]
        public void CookieNameWithPrefixHostTester_Should_Report_If___host_Cookies_With_Path_But_Secure_Attributes_Via_Https()
        {
            // Prepare the test
            var config = A.Fake<IResponseCookieNamePrefixTesterConfig>();
            IResponseAnalyser httpOnlyTester = new CookieNameWithPrefixHostTester(config);
            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            httpOnlyTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Use HTTPs.
            var requestEvent = this.GetHttpResponseReceivedEventArgs2(UrlScheme.Https);

            // __host cookie name without secure and path
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__host-Cookie1=SomeValue; Path=/;"));

            // should get errors. One for missing secure and path
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__host-Cookie1", Value = "SomeValue", HttpOnly = false, IsSecure = false, Path = "/" });


            // Invoke the method being tested
            httpOnlyTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(httpOnlyTester.Results);
            Assert.IsNotNull(httpOnlyTester.Results.Count() == 1);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(resultHolder).MustHaveHappened();
        }
    }
}

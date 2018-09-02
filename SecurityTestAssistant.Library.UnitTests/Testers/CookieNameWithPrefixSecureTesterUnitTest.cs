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
    public class CookieNameWithPrefixSecureTesterUnitTest
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
        public void CookieNameWithPrefixSecureTester_Should_Report_If___secure_Cookies_Do_Not_Have_secure_Attribute()
        {
            // Prepare the test
            var config = A.Fake<IResponseCookieNamePrefixTesterConfig>();
            IResponseAnalyser httpOnlyTester = new CookieNameWithPrefixSecureTester(config);
            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            httpOnlyTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;


            var requestEvent = this.GetHttpResponseReceivedEventArgs2(UrlScheme.Https);
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__secure-Cookie1=SomeValue"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__secure-Cookie2=SomeValue2 secure"));
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__secure-Cookie1", Value = "SomeValue", HttpOnly = false, IsSecure = false });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__secure-Cookie2", Value = "SomeValue2", HttpOnly = false, IsSecure = true });


            // Invoke the method being tested
            httpOnlyTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(httpOnlyTester.Results);
            Assert.IsNotNull(httpOnlyTester.Results.Count() == 1);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(resultHolder).MustHaveHappened();
        }

        [TestMethod]
        public void CookieNameWithPrefixSecureTester_Should_NOT_Report_If_All____secure_Cookies_Have_secure_Attribute()
        {
            // Prepare the test
            var config = A.Fake<IResponseCookieNamePrefixTesterConfig>();
            IResponseAnalyser httpOnlyTester = new CookieNameWithPrefixSecureTester(config);
            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            httpOnlyTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;


            var requestEvent = this.GetHttpResponseReceivedEventArgs2(UrlScheme.Https);
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__secure-Cookie1=SomeValue secure"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__secure-Cookie2=SomeValue2 secure"));
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__secure-Cookie1", Value = "SomeValue", HttpOnly = false, IsSecure = true });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__secure-Cookie2", Value = "SomeValue2", HttpOnly = false, IsSecure = true });


            // Invoke the method being tested
            httpOnlyTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(httpOnlyTester.Results);
            Assert.IsNotNull(httpOnlyTester.Results.Count() == 0);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustNotHaveHappened();
            A.CallTo(resultHolder).MustNotHaveHappened();
        }

        [TestMethod]
        public void CookieNameWithPrefixSecureTester_Should_Report_If____secure_Cookies_Have_secure_Attribute_Accessed_Via_Http()
        {
            // Prepare the test
            var config = A.Fake<IResponseCookieNamePrefixTesterConfig>();
            IResponseAnalyser httpOnlyTester = new CookieNameWithPrefixSecureTester(config);
            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            httpOnlyTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Note: the __secure cookies are transferred via Http and not via HTTPS
            var requestEvent = this.GetHttpResponseReceivedEventArgs2(UrlScheme.Http);

            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__secure-Cookie1=SomeValue secure"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__secure-Cookie2=SomeValue2 secure"));
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__secure-Cookie1", Value = "SomeValue", HttpOnly = false, IsSecure = true });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__secure-Cookie2", Value = "SomeValue2", HttpOnly = false, IsSecure = true });


            // Invoke the method being tested
            httpOnlyTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(httpOnlyTester.Results);
            Assert.IsNotNull(httpOnlyTester.Results.Count() == 2);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustHaveHappened(Repeated.Exactly.Twice);
            A.CallTo(resultHolder).MustHaveHappened();
        }

        [TestMethod]
        public void CookieNameWithPrefixSecureTester_Should_Report_If____secure_Cookies_Does_Not_Have_secure_Attribute_And_Accessed_Via_Http()
        {
            // Prepare the test
            var config = A.Fake<IResponseCookieNamePrefixTesterConfig>();
            IResponseAnalyser httpOnlyTester = new CookieNameWithPrefixSecureTester(config);
            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            httpOnlyTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Note: the __secure cookies are transferred via Http and not via HTTPS
            var requestEvent = this.GetHttpResponseReceivedEventArgs2(UrlScheme.Http);

            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__secure-Cookie1=SomeValue")); // no secure and accessed via http.
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "__secure-Cookie2=SomeValue2 secure")); // secure but accessed via http 

            // no secure and accessed via http
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__secure-Cookie1", Value = "SomeValue", HttpOnly = false, IsSecure = false });
            // secure but accessed via http
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "__secure-Cookie2", Value = "SomeValue2", HttpOnly = false, IsSecure = true });


            // Invoke the method being tested
            httpOnlyTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(httpOnlyTester.Results);

            /* 1. Issue #1 The secure attribute is missing for one cookie.
             * 2. the secure attribute is there for another cookie, so there is no problem with secure attribute
             * 3. Issue #2 and 3: Both the __secure cookies are accessed via HTTP and not via HTTPS. So both should be reported.
             * */
            Assert.IsNotNull(httpOnlyTester.Results.Count() == 3);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustHaveHappened(Repeated.Exactly.Times(3));
            A.CallTo(resultHolder).MustHaveHappened();
        }

    }
}

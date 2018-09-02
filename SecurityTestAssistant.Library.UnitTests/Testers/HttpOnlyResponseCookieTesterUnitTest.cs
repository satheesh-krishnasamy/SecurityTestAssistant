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
    public class HttpOnlyResponseCookieTesterUnitTest
    {

        private HttpResponseReceivedEventArgs2 GetHttpResponseReceivedEventArgs2()
        {
            var myUrl = "https://www.mywebsite.com/testpage";
            return A.Fake<HttpResponseReceivedEventArgs2>(
                x => x.WithArgumentsForConstructor(() => new HttpResponseReceivedEventArgs2(
                myUrl,
                UrlScheme.Https,
                "Get"
            )));
        }

        [TestMethod]
        public void HttpOnlyResponseCookieTester_Should_Find_MissingHTTPAttribute_If_non_http_onlyCookie()
        {
            // Prepare the test
            var config = A.Fake<IHttpOnlyCookieTesterConfig>();
            IResponseAnalyser httpOnlyTester = new HttpOnlyResponseCookieTester(config);

            var requestEvent = this.GetHttpResponseReceivedEventArgs2();

            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "someCookie=SomeValue"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "someCookie2=SomeValue2 httponly"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "someCookie3=SomeValue3 httponly"));
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "someCookie", Value = "SomeValue", HttpOnly = false });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "someCookie2", Value = "SomeValue2", HttpOnly = false });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "someCookie3", Value = "SomeValue3", HttpOnly = true });

            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            httpOnlyTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Invoke the method being tested
            httpOnlyTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(httpOnlyTester.Results);
            Assert.IsNotNull(httpOnlyTester.Results.Count() == 2);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustHaveHappened(Repeated.Exactly.Twice);
            A.CallTo(resultHolder).MustHaveHappened();
        }

        [TestMethod]
        public void HttpOnlyResponseCookieTester_Should_NOT_Find_MissingHTTPAttribute_If_All_Cookies_have_HTTPOnly_attribute()
        {
            // Prepare the test
            var config = A.Fake<IHttpOnlyCookieTesterConfig>();
            IResponseAnalyser httpOnlyTester = new HttpOnlyResponseCookieTester(config);

            var requestEvent = this.GetHttpResponseReceivedEventArgs2();


            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "someCookie=SomeValue httponly"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "someCookie2=SomeValue2 httponly"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "someCookie3=SomeValue3 httponly"));
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "someCookie", Value = "SomeValue", HttpOnly = true });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "someCookie2", Value = "SomeValue2", HttpOnly = true });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "someCookie3", Value = "SomeValue3", HttpOnly = true });

            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            httpOnlyTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Invoke the method being tested
            httpOnlyTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(httpOnlyTester.Results);
            Assert.IsNotNull(httpOnlyTester.Results.Count() == 0);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustNotHaveHappened();
            A.CallTo(resultHolder).MustNotHaveHappened();
        }

    }
}

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
    public class SecureResponseCookieTesterUnitTest
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
        public void SecureResponseCookieTester_Should_Find_Insecure_Cookie()
        {
            // Prepare the test
            var config = A.Fake<ISecureResponseCookieTesterConfig>();
            IResponseAnalyser secureCookieTester = new SecureResponseCookieTester(config);
            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            secureCookieTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            var requestEvent = this.GetHttpResponseReceivedEventArgs2();
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie0", "someCookie=SomeValue"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie1", "someCookie=SomeValue"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie2", "someCookie2=SomeValue2 httponly secure"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie3", "someCookie3=SomeValue3 httponly secure"));
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "someCookie0", Value = "SomeValue", HttpOnly = true, IsSecure = false });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "someCookie1", Value = "SomeValue", HttpOnly = true, IsSecure = false });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "someCookie2", Value = "SomeValue2", HttpOnly = true, IsSecure = true });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "someCookie3", Value = "SomeValue3", HttpOnly = true, IsSecure = true });


            // Invoke the method being tested
            secureCookieTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(secureCookieTester.Results);
            Assert.IsNotNull(secureCookieTester.Results.Count() == 2);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustHaveHappened(Repeated.Exactly.Twice);
            A.CallTo(resultHolder).MustHaveHappened();
        }

        [TestMethod]
        public void SecureResponseCookieTester_Should_NOT_Report_If_All_Cookies_Have_Secure_And_HttpOnly_Attribute()
        {
            // Prepare the test
            var config = A.Fake<ISecureResponseCookieTesterConfig>();
            IResponseAnalyser secureCookieTester = new SecureResponseCookieTester(config);
            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            secureCookieTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            var requestEvent = this.GetHttpResponseReceivedEventArgs2();
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie0", "someCookie=SomeValue httponly secure"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie1", "someCookie=SomeValue httponly secure"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie2", "someCookie2=SomeValue2 httponly secure"));
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie3", "someCookie3=SomeValue3 httponly secure"));
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "someCookie0", Value = "SomeValue", HttpOnly = true, IsSecure = true });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "someCookie1", Value = "SomeValue", HttpOnly = true, IsSecure = true });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "someCookie2", Value = "SomeValue2", HttpOnly = true, IsSecure = true });
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "someCookie3", Value = "SomeValue3", HttpOnly = true, IsSecure = true });



            // Invoke the method being tested
            secureCookieTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(secureCookieTester.Results);
            Assert.IsNotNull(secureCookieTester.Results.Count() == 0);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustNotHaveHappened();
            A.CallTo(resultHolder).MustNotHaveHappened();
        }

        [TestMethod]
        public void SecureResponseCookieTester_Should_NOT_Report_If_ResponseEvent_Object_IsNull()
        {
            // Prepare the test
            var config = A.Fake<ISecureResponseCookieTesterConfig>();
            IResponseAnalyser secureCookieTester = new SecureResponseCookieTester(config);
            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            secureCookieTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Invoke the method being tested
            secureCookieTester.AnalyseHttpResponse(this, null);


            // Assert
            Assert.IsNotNull(secureCookieTester.Results);
            Assert.IsNotNull(secureCookieTester.Results.Count() == 0);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustNotHaveHappened();
            A.CallTo(resultHolder).MustNotHaveHappened();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityTestAssistant.Library.Config;
using SecurityTestAssistant.Library.Logic;
using SecurityTestAssistant.Library.Models;
using SecurityTestAssistant.Library.Models.Events;
using SecurityTestAssistant.Library.Net;
using SecurityTestAssistant.Library.Testers;
using SecurityTestAssistant.Library.Testers.Implementation;
using SecurityTestAssistant.Library.Tests.Config;

namespace SecurityTestAssistant.Library.UnitTests.Net
{
    [TestClass]
    public class ServerFingerprintingBySessionIDCookieNameTesterUnitTest
    {
        private const string myTargetUrl = "https://www.mywebsite.com/testpage";

        private HttpResponseReceivedEventArgs2 GetHttpResponseReceivedEventArgs2()
        {
            return A.Fake<HttpResponseReceivedEventArgs2>(
                x => x.WithArgumentsForConstructor(() => new HttpResponseReceivedEventArgs2(
                myTargetUrl,
                UrlScheme.Https,
                "Get"
            )));
        }

        [TestMethod]
        public void ServerFingerprintingByCookieNameTester_Should_Report_ServerFingerprinting_If_WellKnown_Cookies_Present()
        {
            // Prepare the test
            var config = A.Fake<IServerFingerprintingByCookieNameTesterConfig>();
            A.CallTo(() => config.KnownTechCookiePatterns).Returns(
                new List<StringPattern>
            {
                new StringPattern("ASP.NET_SessionId", PatternMatchType.StartsWith, "ASP.NET", "Session ID cookie"),
            });

            IResponseAnalyser serverFingerprintingByCookieTester = new ServerFingerprintingByCookieNameTester(config);

            var requestEvent = this.GetHttpResponseReceivedEventArgs2();

            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "ASP.NET_SessionId_some_other_random_value=abcd-efgh-ijkl-mnop-qrst httponly secure"));
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "ASP.NET_SessionId_some_other_random_value", Value = "SomeValue", HttpOnly = true, IsSecure = true });

            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            serverFingerprintingByCookieTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Invoke the method being tested
            serverFingerprintingByCookieTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(serverFingerprintingByCookieTester.Results);
            Assert.IsNotNull(serverFingerprintingByCookieTester.Results.Count() == 1);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustHaveHappened(Repeated.Exactly.Once);
        }


        [TestMethod]
        public void ServerFingerprintingByCookieNameTester_Should_NOT_Report_ServerFingerprinting_If_NoWellKnown_Cookies_Present()
        {
            // Prepare the test
            var config = A.Fake<IServerFingerprintingByCookieNameTesterConfig>();
            A.CallTo(() => config.KnownTechCookiePatterns).Returns(
                new List<StringPattern>
            {
                new StringPattern("ASP.NET_SessionId", PatternMatchType.StartsWith, "ASP.NET", "Session ID cookie"),
            });

            IResponseAnalyser serverFingerprintingByCookieTester = new ServerFingerprintingByCookieNameTester(config);

            var requestEvent = this.GetHttpResponseReceivedEventArgs2();

            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "SessionId_some_other_random_value=abcd-efgh-ijkl-mnop-qrst httponly secure"));
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "SessionId_some_other_random_value", Value = "SomeValue", HttpOnly = true, IsSecure = true });

            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            serverFingerprintingByCookieTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Invoke the method being tested
            serverFingerprintingByCookieTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(serverFingerprintingByCookieTester.Results);
            Assert.IsNotNull(serverFingerprintingByCookieTester.Results.Count() == 0);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustNotHaveHappened();
        }


        [TestMethod]
        public void ServerFingerprintingByCookieNameTester_Should_NOT_Report_ServerFingerprinting_If_No_SessionID_Cookie_Present()
        {
            // Prepare the test
            var config = A.Fake<IServerFingerprintingByCookieNameTesterConfig>();
            A.CallTo(() => config.KnownTechCookiePatterns).Returns(
                new List<StringPattern>
            {
                new StringPattern("ASP.NET_SessionId", PatternMatchType.StartsWith, "ASP.NET", "Session ID cookie"),
            });

            IResponseAnalyser serverFingerprintingByCookieTester = new ServerFingerprintingByCookieNameTester(config);

            var requestEvent = this.GetHttpResponseReceivedEventArgs2();

            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "somecookie=abcd-efgh-ijkl-mnop-qrst httponly secure"));
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "somecookie", Value = "SomeValue", HttpOnly = true, IsSecure = true });

            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            serverFingerprintingByCookieTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Invoke the method being tested
            serverFingerprintingByCookieTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(serverFingerprintingByCookieTester.Results);
            Assert.IsNotNull(serverFingerprintingByCookieTester.Results.Count() == 0);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustNotHaveHappened();
        }

        [TestMethod]
        public void ServerFingerprintingByCookieNameTester_Should_NOT_Report_ServerFingerprinting_If_No_Cookies_Present()
        {
            // Prepare the test
            var config = A.Fake<IServerFingerprintingByCookieNameTesterConfig>();
            A.CallTo(() => config.KnownTechCookiePatterns).Returns(
                new List<StringPattern>
            {
                new StringPattern("ASP.NET_SessionId", PatternMatchType.StartsWith, "ASP.NET", "Session ID cookie"),
            });

            IResponseAnalyser serverFingerprintingByCookieTester = new ServerFingerprintingByCookieNameTester(config);

            var requestEvent = this.GetHttpResponseReceivedEventArgs2();

            //requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "somecookie=abcd-efgh-ijkl-mnop-qrst httponly secure"));
            //requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "somecookie", Value = "SomeValue", HttpOnly = true, IsSecure = true });

            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            serverFingerprintingByCookieTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Invoke the method being tested
            serverFingerprintingByCookieTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(serverFingerprintingByCookieTester.Results);
            Assert.IsNotNull(serverFingerprintingByCookieTester.Results.Count() == 0);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustNotHaveHappened();
        }

        [TestMethod]
        public void ServerFingerprintingByCookieNameTester_Should_NOT_Report_ServerFingerprinting_If_Null_Response()
        {
            // Prepare the test
            var config = A.Fake<IServerFingerprintingByCookieNameTesterConfig>();
            A.CallTo(() => config.KnownTechCookiePatterns).Returns(
                new List<StringPattern>
            {
                new StringPattern("ASP.NET_SessionId", PatternMatchType.StartsWith, "ASP.NET", "Session ID cookie"),
            });

            IResponseAnalyser serverFingerprintingByCookieTester = new ServerFingerprintingByCookieNameTester(config);

            var requestEvent = (HttpResponseReceivedEventArgs2)null;

            //requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "somecookie=abcd-efgh-ijkl-mnop-qrst httponly secure"));
            //requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "somecookie", Value = "SomeValue", HttpOnly = true, IsSecure = true });

            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            serverFingerprintingByCookieTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Invoke the method being tested
            serverFingerprintingByCookieTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(serverFingerprintingByCookieTester.Results);
            Assert.IsNotNull(serverFingerprintingByCookieTester.Results.Count() == 0);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustNotHaveHappened();
        }
    }
}

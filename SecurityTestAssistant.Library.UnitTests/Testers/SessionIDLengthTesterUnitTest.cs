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
    public class SessionIdLengthTesterUnitTestUnitTest
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
        public void SessionIdLengthTester_Should_Report_Less_Length_SessionID_Cookie_Value()
        {
            // Prepare the test
            var config = A.Fake<ISessionIdLengthTesterConfig>();
            A.CallTo(() => config.KnownTechCookiePatterns).Returns(
                new List<TechnologyStringPattern>
            {
                new TechnologyStringPattern("ASP.NET_SessionId", PatternMatchType.StartsWith, "ASP.NET", "Session ID cookie"),
            });

            IResponseAnalyser serverFingerprintingByCookieTester = new SessionIdLengthTester(config);

            var requestEvent = this.GetHttpResponseReceivedEventArgs2();

            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "ASP.NET_SessionId_some_other_random_value=abcd; httponly; secure;"));
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "ASP.NET_SessionId_some_other_random_value", Value = "abcd", HttpOnly = true, IsSecure = true });

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
        public void SessionIdLengthTester_Should_Not_Report_If_The_Cookie_Is_Not_SessionID()
        {
            // Prepare the test
            var config = A.Fake<ISessionIdLengthTesterConfig>();
            A.CallTo(() => config.KnownTechCookiePatterns).Returns(
                new List<TechnologyStringPattern>
            {
                new TechnologyStringPattern("ASP.NET_SessionId", PatternMatchType.StartsWith, "ASP.NET", "Session ID cookie"),
            });

            IResponseAnalyser serverFingerprintingByCookieTester = new SessionIdLengthTester(config);

            var requestEvent = this.GetHttpResponseReceivedEventArgs2();

            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "SessionId_some_other_random_value=abcd; httponly; secure;"));
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
        public void SessionIdLengthTester_Should_NOT_Report_If_There_Is_No_Cookie_Present()
        {
            // Prepare the test
            var config = A.Fake<ISessionIdLengthTesterConfig>();
            A.CallTo(() => config.KnownTechCookiePatterns).Returns(
                new List<TechnologyStringPattern>
            {
                new TechnologyStringPattern("ASP.NET_SessionId", PatternMatchType.StartsWith, "ASP.NET", "Session ID cookie"),
            });

            IResponseAnalyser serverFingerprintingByCookieTester = new SessionIdLengthTester(config);

            var requestEvent = this.GetHttpResponseReceivedEventArgs2();

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
        public void SessionIdLengthTester_Should_NOT_Report_If__SessionID_Cookie_Values_Length_Equal_To_SuggestedLength()
        {
            // Prepare the test
            var config = A.Fake<ISessionIdLengthTesterConfig>();
            A.CallTo(() => config.KnownTechCookiePatterns).Returns(
                new List<TechnologyStringPattern>
            {
                new TechnologyStringPattern("ASP.NET_SessionId", PatternMatchType.StartsWith, "ASP.NET", "Session ID cookie"),
            });

            IResponseAnalyser serverFingerprintingByCookieTester = new SessionIdLengthTester(config);

            var requestEvent = this.GetHttpResponseReceivedEventArgs2();
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "ASP.NET_SessionId_some_other_random_value=1234567890123456; httponly; secure;"));
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "ASP.NET_SessionId_some_other_random_value", Value = "1234567890123456", HttpOnly = true, IsSecure = true });


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
        public void SessionIdLengthTester_Should_NOT_Report_If__SessionID_Cookie_Value_Is_Lengthier_Than_SuggestedLength()
        {
            // Prepare the test
            var config = A.Fake<ISessionIdLengthTesterConfig>();
            A.CallTo(() => config.KnownTechCookiePatterns).Returns(
                new List<TechnologyStringPattern>
            {
                new TechnologyStringPattern("ASP.NET_SessionId", PatternMatchType.StartsWith, "ASP.NET", "Session ID cookie"),
            });

            IResponseAnalyser serverFingerprintingByCookieTester = new SessionIdLengthTester(config);

            var requestEvent = this.GetHttpResponseReceivedEventArgs2();
            requestEvent.Response.Headers.Add(new HttpHeader("Set-Cookie", "ASP.NET_SessionId_some_other_random_value=1234567890123456; httponly; secure;"));
            requestEvent.Response.Cookies.Add(new HttpCookie() { Name = "ASP.NET_SessionId_some_other_random_value", Value = "1234567890123456", HttpOnly = true, IsSecure = true });


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

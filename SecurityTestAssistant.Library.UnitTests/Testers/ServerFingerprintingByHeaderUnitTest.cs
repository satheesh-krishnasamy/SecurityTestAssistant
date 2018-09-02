using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityTestAssistant.Library.Config;
using SecurityTestAssistant.Library.Logic;
using SecurityTestAssistant.Library.Models.Events;
using SecurityTestAssistant.Library.Net;
using SecurityTestAssistant.Library.Testers;
using SecurityTestAssistant.Library.Testers.Implementation;
using System.Collections.Generic;
using System.Linq;

namespace SecurityTestAssistant.Library.UnitTests.Net
{
    [TestClass]
    public class ServerFingerprintingByHeaderUnitTest
    {
        [TestMethod]
        public void ServerFingerprintingByHeader_Should_Warn_Server_Response_Header()
        {
            // Prepare the test
            var config = A.Fake<IServerFingerPrintingByHttpHeaderTesterConfig>();
            IResponseAnalyser serverFingerPrintingByHeaderTester = new ServerFingerprintingByHeader(config);

            A.CallTo(() => config.KnownServerHeaderValues).Returns(
                new List<ServerHeaderValuePattern>
                    {
                        new ServerHeaderValuePattern("IIS", "(?i)(?<= |^)(microsoft|IIS)(?= |$)", PatternMatchType.RegEx)
                    }
                );

            var myUrl = "https://www.mywebsite.com/testpage";
            var requestEvent = A.Fake<HttpResponseReceivedEventArgs2>(
                x => x.WithArgumentsForConstructor(() => new HttpResponseReceivedEventArgs2(
                myUrl,
                UrlScheme.Https,
                "Get"
            )));

            requestEvent.Response.Headers.Add(new HttpHeader("Server", "Microsoft IIS"));

            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            serverFingerPrintingByHeaderTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Invoke the method being tested
            serverFingerPrintingByHeaderTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(serverFingerPrintingByHeaderTester.Results);
            Assert.IsNotNull(serverFingerPrintingByHeaderTester.Results.Count() == 1);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(resultHolder).MustHaveHappened();
        }

        [TestMethod]
        public void ServerFingerprintingByHeader_Should_NOT_Warn_If_Server_Response_Header_Does_Not_Match_With_Well_Known_Value()
        {
            // Prepare the test
            var config = A.Fake<IServerFingerPrintingByHttpHeaderTesterConfig>();
            IResponseAnalyser serverFingerPrintingByHeaderTester = new ServerFingerprintingByHeader(config);

            A.CallTo(() => config.KnownServerHeaderValues).Returns(
                new List<ServerHeaderValuePattern>
                    {
                        new ServerHeaderValuePattern("IIS", "(?i)(?<= |^)(microsoft|IIS)(?= |$)", PatternMatchType.RegEx)
                    }
                );

            var myUrl = "https://www.mywebsite.com/testpage";
            var requestEvent = A.Fake<HttpResponseReceivedEventArgs2>(
                x => x.WithArgumentsForConstructor(() => new HttpResponseReceivedEventArgs2(
                myUrl,
                UrlScheme.Https,
                "Get"
            )));

            requestEvent.Response.Headers.Add(new HttpHeader("Server", "My customer server name"));

            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            serverFingerPrintingByHeaderTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Invoke the method being tested
            serverFingerPrintingByHeaderTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(serverFingerPrintingByHeaderTester.Results);
            Assert.IsNotNull(serverFingerPrintingByHeaderTester.Results.Count() == 0);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustNotHaveHappened();
            A.CallTo(resultHolder).MustNotHaveHappened();
        }

        [TestMethod]
        public void ServerFingerprintingByHeader_Should_NOT_Warn_If_Server_Response_Header_Is_Missing()
        {
            // Prepare the test
            var config = A.Fake<IServerFingerPrintingByHttpHeaderTesterConfig>();
            IResponseAnalyser serverFingerPrintingByHeaderTester = new ServerFingerprintingByHeader(config);

            A.CallTo(() => config.KnownServerHeaderValues).Returns(
                new List<ServerHeaderValuePattern>
                    {
                        new ServerHeaderValuePattern("IIS", "(?i)(?<= |^)(microsoft|IIS)(?= |$)", PatternMatchType.RegEx)
                    }
                );

            var myUrl = "https://www.mywebsite.com/testpage";
            var requestEvent = A.Fake<HttpResponseReceivedEventArgs2>(
                x => x.WithArgumentsForConstructor(() => new HttpResponseReceivedEventArgs2(
                myUrl,
                UrlScheme.Https,
                "Get"
            )));

            var resultHolder = A.Fake<IApplicationReportDataHandler>();
            serverFingerPrintingByHeaderTester.OnAnalysisResultPublished += resultHolder.HandleAnalysisResult;

            // Invoke the method being tested
            serverFingerPrintingByHeaderTester.AnalyseHttpResponse(this, requestEvent);


            // Assert
            Assert.IsNotNull(serverFingerPrintingByHeaderTester.Results);
            Assert.IsNotNull(serverFingerPrintingByHeaderTester.Results.Count() == 0);
            A.CallTo(() => resultHolder.HandleAnalysisResult(A<object>._, A<AnalysisCompletedEventAgrs>._)).MustNotHaveHappened();
            A.CallTo(resultHolder).MustNotHaveHappened();
        }

    }
}

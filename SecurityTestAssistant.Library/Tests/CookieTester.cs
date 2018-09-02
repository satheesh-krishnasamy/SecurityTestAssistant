namespace SecurityTestAssistant.Library.Logic
{
    using SecurityTestAssistant.Library.Config;
    using SecurityTestAssistant.Library.Models;
    using SecurityTestAssistant.Library.Models.Events;
    using SecurityTestAssistant.Library.Testers.EVents;
    using SecurityTestAssistant.Library.Tests.Config;
    using SecurityTestAssistant.Library.Utils;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class CookieTester : IApplicationDataConsumer
    {
        private readonly IList<TestPage> pages;
        public readonly CancellationToken cancellationToken;
        private readonly ISecurityApplicationConfig config;
        private readonly IList<Task> tasksPending = new List<Task>();

        public event HandleAnalysisResult HandleAnalysisResult;

        public CookieTester(CancellationTokenSource tokenSource, ISecurityApplicationConfig config)
        {
            pages = new List<TestPage>();
            this.cancellationToken = tokenSource.Token;
            this.config = config;

        }

        private void AnalysePage(TestPage page)
        {
            if (page == null)
                return;

            if (!this.cancellationToken.IsCancellationRequested)
            {
                if (page.Cookies != null || page.Cookies.Count > 0)
                {
                    var parallelOptions = new ParallelOptions() { CancellationToken = this.cancellationToken };
                    Parallel.ForEach(
                        page.Cookies,
                        parallelOptions,
                        (p) =>
                        {
                            if (!p.IsSecure)
                            {
                                if (this.HandleAnalysisResult != null)
                                {
                                    var result = new AnalysisResult(
                                        "Missing secure attribute: Cookie can be set with secure attribute as true, to send it via only HTTPS.",
                                        FindingType.Error,
                                        "Review and apply the secure attribute",
                                        "Missing Secure attribute",
                                        page.ToDictionary(),
                                        this.config.References.SecureCookieAttribute);

                                    this.HandleAnalysisResult(this, new AnalysisCompletedEventAgrs(result));
                                }
                            }

                            if (!p.HttpOnly)
                            {
                                var result = new AnalysisResult(
                                        "Missing HTTP only attribute: Cookie can be marked http only if it is not expected to be accessed from javascript/VB script.",
                                        p.IsSessionCookie ? FindingType.Error : FindingType.Warning,
                                        "Review and apply httponly attribute.",
                                        "Missing http only attribute",
                                        page.ToDictionary(),
                                        this.config.References.HttpOnlyCookie);

                                this.HandleAnalysisResult(this, new AnalysisCompletedEventAgrs(result));
                            }
                        });
                }

            }
        }

        private void ProcessDataAsync(TestPage page)
        {

        }

        public void WebPageLoadCompleted(object sender, WebPageCompletedEventAgrs ec)
        {
            this.AnalysePage(ec.Page);
        }
    }
}

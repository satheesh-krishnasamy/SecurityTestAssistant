using SecurityTestAssistant.Library.Models;
using SecurityTestAssistant.Library.Net;
using SecurityTestAssistant.Library.Testers.EVents;
using System.Collections.Generic;

namespace SecurityTestAssistant.Library.Testers
{
    public interface IResponseAnalyser
    {
        void AnalyseHttpResponse(object sender, HttpResponseReceivedEventArgs2 responseReceivedEvent);
        IEnumerable<AnalysisResult> Results { get; }
        event HandleAnalysisResult OnAnalysisResultPublished;
    }

    public interface IRequestAnalyser
    {
        void AnalyseHttpResponse(HttpRequest response);
        IEnumerable<AnalysisResult> Results { get; }
    }
}

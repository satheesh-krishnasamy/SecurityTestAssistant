namespace SecurityTestAssistant.Library.Logic
{
    using SecurityTestAssistant.Library.Models;
    using SecurityTestAssistant.Library.Models.Events;
    using System.Collections.Generic;

    public class AnalysisResultHandler : IApplicationReportDataHandler
    {
        private readonly IList<AnalysisResult> results;

        public AnalysisResultHandler()
        {
            this.results = new List<AnalysisResult>();
        }
        public IEnumerable<AnalysisResult> Results
        {
            get
            {
                return this.results;
            }
        }


        public void HandleAnalysisResult(object sender, AnalysisCompletedEventAgrs args)
        {
            this.results.Add(args.Result);
        }
    }
}

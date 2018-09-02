using SecurityTestAssistant.Library.Models.Report;
using System.Collections.Generic;

namespace SecurityTestAssistant.Library.Models
{
    public enum FindingType
    {
        None = 0,
        Error = 1,
        Warning = 2,
        Info = 3,
        Appreciation = 4
    }

    public class AnalysisResult
    {
        public AnalysisResult()
        {
            this.AdditionalProperties = new List<KeyValuePair<string, string>>();
        }

        public AnalysisResult(
            string findingMessage,
            FindingType findingType,
            string recommendation,
            string testType,
            IEnumerable<KeyValuePair<string, string>> additionalProps,
            IEnumerable<string> referenceUrl)
        {
            this.FindingMessage = findingMessage;
            this.Recommendation = recommendation;
            this.FindingType = findingType;
            this.AdditionalProperties = additionalProps;
            this.TestType = testType;
            this.ReferenceUrl = referenceUrl;
        }

        [ReportDisplay("Additional details")]
        public IEnumerable<KeyValuePair<string, string>> AdditionalProperties { get; private set; }

        [ReportDisplay("Finding type")]
        public FindingType FindingType { get; private set; }

        [ReportDisplay("Message")]
        public string FindingMessage { get; private set; }

        [ReportDisplay("Recommendation")]
        public string Recommendation { get; private set; }

        [ReportDisplay("Test type")]
        public string TestType { get; private set; }

        [ReportDisplay("Reference Url")]
        public IEnumerable<string> ReferenceUrl { get; private set; }
    }
}

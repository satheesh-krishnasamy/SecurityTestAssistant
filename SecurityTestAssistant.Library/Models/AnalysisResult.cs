using ReportGeneratorUtils;
using SecurityTestAssistant.Library.Extensions;
using System.Collections.Generic;

namespace SecurityTestAssistant.Library.Models
{
    public enum SeverityType
    {
        None = 0,
        Error = 1,
        Warning = 2,
        Info = 3,
        Appreciation = 4
    }

    public class GroupedAnalysisResult
    {
        public SeverityType Severity { get; set; }
        public IList<AnalysisResult> Results { get; set; }
    }

    public class AnalysisResult
    {
        public AnalysisResult()
        {
            this.AdditionalProperties = new List<KeyValuePair<string, string>>();
        }

        public AnalysisResult(
            string findingMessage,
            SeverityType severity,
            string recommendation,
            string testType,
            IEnumerable<KeyValuePair<string, string>> additionalProps,
            IEnumerable<string> referenceUrl)
        {
            this.FindingMessage = findingMessage;
            this.Recommendation = recommendation;
            this.Severity = severity;
            this.AdditionalProperties = additionalProps;
            this.TestType = testType;
            this.ReferenceUrl = referenceUrl;
        }

        public IEnumerable<KeyValuePair<string, string>> AdditionalProperties { get; private set; }

        [ReportDisplay("Severity")]
        public SeverityType Severity { get; private set; }

        [ReportDisplay("Message")]
        public string FindingMessage { get; private set; }

        [ReportDisplay("Recommendation")]
        public string Recommendation { get; private set; }

        [ReportDisplay("Test type")]
        public string TestType { get; private set; }

        public IEnumerable<string> ReferenceUrl { get; private set; }

        [ReportDisplay("Reference Urls")]
        public string ReferenceUrlsAsString => this.ReferenceUrl.ToString(" ; ");

        [ReportDisplay("Additional details")]
        public string AdditionalPropertiesAsString => this.AdditionalProperties.ToString(" : ", ",");

    }
}

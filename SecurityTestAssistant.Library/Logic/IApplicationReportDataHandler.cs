using SecurityTestAssistant.Library.Models;
using SecurityTestAssistant.Library.Models.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecurityTestAssistant.Library.Logic
{
    public interface IApplicationReportDataHandler
    {
        void HandleAnalysisResult(object sender, AnalysisCompletedEventAgrs args);
        IEnumerable<AnalysisResult> Results { get; }
    }
}
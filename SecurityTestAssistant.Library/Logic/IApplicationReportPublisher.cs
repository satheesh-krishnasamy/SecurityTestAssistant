using SecurityTestAssistant.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecurityTestAssistant.Library.Logic
{
    public interface IApplicationReportPublisher
    {
        Task PublishReport(IList<AnalysisResult> results);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTestAssistant.Library.Models.Events
{
    public class AnalysisCompletedEventAgrs : EventArgs
    {
        public AnalysisCompletedEventAgrs(AnalysisResult result)
        {
            this.Result = result;
        }

        public AnalysisResult Result { get; private set; }
    }
}

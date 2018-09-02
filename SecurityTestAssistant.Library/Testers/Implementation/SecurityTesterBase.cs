using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurityTestAssistant.Library.Models;
using SecurityTestAssistant.Library.Models.Events;
using SecurityTestAssistant.Library.Net;
using SecurityTestAssistant.Library.Testers.EVents;

namespace SecurityTestAssistant.Library.Testers.Implementation
{
    /// <summary>
    /// Base class for security test analysers
    /// </summary>
    /// <seealso cref="SecurityTestAssistant.Library.Testers.IResponseAnalyser" />
    public abstract class SecurityTesterBase : IResponseAnalyser
    {
        private readonly IList<AnalysisResult> results = new List<AnalysisResult>();

        public IEnumerable<AnalysisResult> Results
        {
            get
            {
                return this.results;
            }
        }

        /// <summary>
        /// Analyses the HTTP response. Refer the child class for the security test being performed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="responseReceivedEvent">The response received event.</param>
        public virtual void AnalyseHttpResponse(object sender, HttpResponseReceivedEventArgs2 responseReceivedEvent)
        {

        }

        protected void AddResult(AnalysisResult result)
        {
            this.results.Add(result);
            OnAnalysisResultPublished?.Invoke(this, new AnalysisCompletedEventAgrs(result));
        }

        public event HandleAnalysisResult OnAnalysisResultPublished;
    }
}

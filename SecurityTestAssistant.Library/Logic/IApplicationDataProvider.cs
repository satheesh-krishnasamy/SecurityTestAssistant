using SecurityTestAssistant.Library.Models;
using SecurityTestAssistant.Library.Testers.EVents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTestAssistant.Library.Logic
{
    public delegate void WebPageLoadCompleted(object sender, WebPageCompletedEventAgrs e);
   


    public class WebPageCompletedEventAgrs : EventArgs
    {
        public WebPageCompletedEventAgrs(TestPage page)
        {
            this.Page = page;
        }

        public TestPage Page { get; private set; }
    }




    public interface IApplicationDataProvider
    {
        event WebPageLoadCompleted WebPageLoadCompleted;
    }

    public interface IApplicationDataConsumer
    {
        void WebPageLoadCompleted(object sender, WebPageCompletedEventAgrs ec);
        event HandleAnalysisResult HandleAnalysisResult;
    }
}

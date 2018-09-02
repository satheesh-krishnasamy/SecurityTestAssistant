namespace SecurityTestAssistant
{
    using SecurityTestAssistant.Library.Logic;
    using SecurityTestAssistant.Library.Net;
    using SecurityTestAssistant.Library.Testers;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Forms;

    static class Program
    {
        private static IEnumerable<IApplicationDataConsumer> analysers;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Bootstrap.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            analysers = Bootstrap.container.GetAllInstances<IApplicationDataConsumer>();
            var reportDataAccumulator = Bootstrap.container.GetInstance<IApplicationReportDataHandler>();
            var httpResponseAnalysers = Bootstrap.container.GetAllInstances<IResponseAnalyser>();

            var appForm = new Main(
                Bootstrap.container.GetInstance<CancellationTokenSource>(),
                Bootstrap.container.GetInstance<ISecurityTestHTTPTrafficListener>(),
               httpResponseAnalysers,
               reportDataAccumulator);

            var dataProvider = ((IApplicationDataProvider)appForm);
            foreach (var anlyser in analysers)
            {
                dataProvider.WebPageLoadCompleted += anlyser.WebPageLoadCompleted;
                anlyser.HandleAnalysisResult += reportDataAccumulator.HandleAnalysisResult;
            }

            Application.Run(appForm);


        }
    }
}

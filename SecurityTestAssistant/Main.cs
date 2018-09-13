namespace SecurityTestAssistant
{
    using ReportGeneratorUtils;
    using ReportGeneratorUtils.Utils;
    using SecurityTestAssistant.Library.Logic;
    using SecurityTestAssistant.Library.Models;
    using SecurityTestAssistant.Library.Net;
    using SecurityTestAssistant.Library.Testers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Windows.Forms;

    public partial class Main : Form, IApplicationDataProvider
    {

        private const char BULLET_CHAR = '\u2022';

        private readonly CancellationTokenSource tokenSource;
        private readonly ISecurityTestHTTPTrafficListener httpTrafficListener;
        private readonly IEnumerable<IResponseAnalyser> httpResponseAnalysers;
        private bool httpListernerRunning;

        public readonly IApplicationReportDataHandler reportDataCollector;

        private readonly char ENTER_KEY = Convert.ToChar(Keys.Enter);

        public event WebPageLoadCompleted WebPageLoadCompleted;
        public bool IsLoginPage { get; set; }
        public bool IsAuthenticatedPage { get; set; }


        public Main(
            CancellationTokenSource tokenSource,
            ISecurityTestHTTPTrafficListener httpTrafficListener,
            IEnumerable<IResponseAnalyser> httpResponseAnalysers,
            IApplicationReportDataHandler reportDataCollector)
        {
            InitializeComponent();
            this.tokenSource = tokenSource;
            this.httpTrafficListener = httpTrafficListener;
            this.httpResponseAnalysers = httpResponseAnalysers;
            this.httpListernerRunning = false;
            this.reportDataCollector = reportDataCollector;

            this.AttachHttpEventsToAnalyser();
        }

        private IList<AnalysisResult> results = new List<AnalysisResult>();

        private void Analyser_HandleAnalysisResult(object sender, Library.Models.Events.AnalysisCompletedEventAgrs result)
        {
            results.Add(result.Result);
            resultBindingSource.DataSource = results;
            resultsGrid.DataSource = resultBindingSource;
        }

        private void AttachHttpEventsToAnalyser()
        {

            resultBindingSource.DataSource = results;
            resultsGrid.DataSource = resultBindingSource;

            foreach (var analyser in this.httpResponseAnalysers)
            {
                this.httpTrafficListener.OnHttpResponseReceived += analyser.AnalyseHttpResponse;
                analyser.OnAnalysisResultPublished += this.reportDataCollector.HandleAnalysisResult;
                analyser.OnAnalysisResultPublished += Analyser_HandleAnalysisResult;
            }
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(webBrowser1.Url);
            var cookies = new CookieContainer();
            req.CookieContainer = cookies;

            var clist = cookies.GetCookies(webBrowser1.Document.Url);
            var pageCookies = new List<PageCookie>();
            foreach (Cookie cki in clist)
            {
                var pcki = new PageCookie()
                {
                    HttpOnly = cki.HttpOnly,
                    IsSecure = cki.Secure,
                    IsSessionCookie = cki.Discard,
                    LifeTime = cki.Expires,
                    Name = cki.Name
                };

                pageCookies.Add(pcki);
            }

            if (this.WebPageLoadCompleted != null)
            {
                var loadedPage = new TestPage(
                    webBrowser1.Url.ToString(),
                    webBrowser1.Url.ToString(),
                    webBrowser1.Document.Title,
                    this.IsLoginPage,
                    this.IsAuthenticatedPage,
                    pageCookies
                    );


                var eventArgs = new WebPageCompletedEventAgrs(loadedPage);
                this.WebPageLoadCompleted(this, eventArgs);
            }
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            this.NavigateToCurrentUrl();
        }

        private void NavigateToCurrentUrl()
        {
            var url = txtUrl.Text.Trim();
            Uri validUrl = null;
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out validUrl))
            {
                webBrowser1.Url = validUrl;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
            webBrowser1.Stop();
        }

        private void BtnLoginPage_Click(object sender, EventArgs e)
        {
            this.IsLoginPage = true;
        }

        private void BtnAuthPage_Click(object sender, EventArgs e)
        {
            this.IsAuthenticatedPage = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.tokenSource.Cancel();
            if (this.httpListernerRunning)
                this.httpTrafficListener.StopListening();
        }

        private void BtnShowResults_Click(object sender, EventArgs e)
        {
            DisplayResults(chkGroupResults.Checked);
        }

        private void WebBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            var method = e.Url;
        }

        private void TxtUrl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ENTER_KEY)
            {
                this.NavigateToCurrentUrl();
            }
        }

        private void BtnHttpListerner_Click(object sender, EventArgs e)
        {
            StopTestingClicked();
        }

        private void StopTestingClicked()
        {
            if (this.httpListernerRunning)
            {
                this.httpTrafficListener.StopListening();
                this.httpListernerRunning = false;
                this.Text = "Web App Security analysis assistant - HTTP listener stopped";
                btnHttpListerner.Text = "Start analysis";
                txtUrlToAnalyse.Enabled = true;
                btnStopAnalysis.Enabled = false;
                if (this.results.Count > 0)
                {
                    this.tabControlMainWindow.SelectedTab = tabFinish;
                }
            }
            else
            {
                Uri url = null;
                if (Uri.TryCreate(txtUrlToAnalyse.Text, UriKind.Absolute, out url))
                {
                    this.httpTrafficListener.DoNotListenAllDomains();
                    this.httpTrafficListener.ListenDomain(url.Host);
                    this.httpTrafficListener.StartListening();
                    this.httpListernerRunning = true;
                    this.Text = "Web App Security analysis assistant - HTTP listener started";
                    btnHttpListerner.Text = "Stop analysis";
                    txtUrlToAnalyse.Enabled = false;
                    btnStopAnalysis.Enabled = true;
                    new CookieCleanup().ShowDialog(this);

                    txtUrl.Text = txtUrlToAnalyse.Text;
                    this.NavigateToCurrentUrl();
                    this.tabControlMainWindow.SelectedTab = tabBrowse;
                }
                else
                {
                    MessageBox.Show("Please provide a  valid URL");
                    if (txtUrlToAnalyse.CanFocus)
                    {
                        txtUrlToAnalyse.SelectAll();
                        txtUrlToAnalyse.Focus();
                    }
                }
            }
        }

        private void DisplayResults(bool shouldGroup)
        {
            resultBindingSource.DataSource = this.GetResults(shouldGroup);
        }

        private IList<AnalysisResult> GetResults(bool shouldGroup)
        {
            if (shouldGroup)
            {
                return reportDataCollector.Results
                    .GroupBy(r => new { r.TestType, r.Severity, r.FindingMessage, r.Recommendation })
                    .Select(a => a.FirstOrDefault())
                    .ToList();
            }
            else
            {
                return reportDataCollector.Results.ToList();
            }
        }

        private IList<AnalysisResult> FilterBySeverity(IList<AnalysisResult> results, SeverityType severityType)
        {
            var filteredResult = results.Where(p => p.Severity == severityType);
            if (filteredResult == null)
            {
                return null;
            }
            else
            {
                return filteredResult.ToList();
            }
        }

        private void ExportReport()
        {
            var resultsCollection = this.GetResults(chkGroupResults.Checked);
            IReportBuilder reportGenerator = new HtmlReportBuilder();

            var errors = this.FilterBySeverity(resultsCollection, SeverityType.Error);
            if (errors.Count > 0)
            {
                reportGenerator.AppendReportSection(
                  ReportSectionDisplayType.Table,
                  errors,
                  "Severity: Error",
                  string.Empty);
            }

            var warnings = this.FilterBySeverity(resultsCollection, SeverityType.Warning);
            if (warnings.Count > 0)
            {
                reportGenerator.AppendReportSection(
                  ReportSectionDisplayType.Table,
                  warnings,
                  "Severity: warning",
                  string.Empty);
            }

            var infos = this.FilterBySeverity(resultsCollection, SeverityType.Info);
            if (infos.Count > 0)
            {
                reportGenerator.AppendReportSection(
                  ReportSectionDisplayType.Table,
                  infos,
                  "Severity: information",
                  string.Empty);
            }

            var appreciations = this.FilterBySeverity(resultsCollection, SeverityType.Appreciation);
            if (appreciations.Count > 0)
            {
                reportGenerator.AppendReportSection(
                  ReportSectionDisplayType.Table,
                  appreciations,
                  "Other good things",
                  string.Empty);
            }

            var htmlReport = reportGenerator.Build(
                "Analysis result",
                string.Empty,
                "Copyright © MyCompany");

            // save the file
            var reportFilePath = $"Reports\\HTMLReport_{DateTime.Now.ToString("yyyyMMddhhmmss")}.html";
            IReportSaver saver = new ReportFileSaver();
            saver.SaveReport(Path.Combine(Environment.CurrentDirectory, reportFilePath), htmlReport, true);
            openFolderInWindowsExplorer(reportFilePath, "");
        }

        private void openFolderInWindowsExplorer(string path, string args)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(args))
                    args = string.Empty;

                System.Diagnostics.Process.Start(path, args);
            }
            catch (Exception excep)
            {
                MessageBox.Show("There was an error in opening your report. " + excep.Message);
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            DisplayResults(this.chkGroupResults.Checked);
        }

        private void TextBox2_MouseEnter(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrlToAnalyse.Text))
            {
                if (Clipboard.ContainsText())
                {
                    var text = Clipboard.GetText();
                    if (Uri.IsWellFormedUriString(text, UriKind.Absolute))
                    {
                        txtUrlToAnalyse.Text = Clipboard.GetText();
                    }
                }
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                txtMainNotice.Text = AppCommonResource.MainNoticeSection
                .Replace("[Bullet]", BULLET_CHAR.ToString());
            }
            catch
            { /**/
                txtMainNotice.Text = "Unable to load the instructions.";
            }
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.ToggleMoreInfo(chkMoreInfo.Checked);
        }

        public class AdditionalInfo
        {
            public string Item { get; set; }
            public string Info { get; set; }
        }

        private void ToggleMoreInfo(bool showMoreInfo)
        {
            splitContainer3.Panel2Collapsed = !showMoreInfo;

            if (showMoreInfo)
            {
                if (resultsGrid.CurrentRow != null && resultBindingSource.Count >= resultsGrid.Rows.Count)
                {
                    var analysisResult = (AnalysisResult)resultsGrid.CurrentRow.DataBoundItem;
                    List<AdditionalInfo> additionalInfo = new List<AdditionalInfo>();

                    if (analysisResult.ReferenceUrl != null && analysisResult.ReferenceUrl.Count() > 0)
                    {
                        var links = from row in analysisResult.ReferenceUrl
                                    select new AdditionalInfo() { Item = "Link", Info = row };
                        additionalInfo.AddRange(links);
                    }
                    if (analysisResult.AdditionalProperties != null && analysisResult.AdditionalProperties.Count() > 0)
                    {

                        var otherInfo = from row in analysisResult.AdditionalProperties
                                        select new AdditionalInfo() { Item = row.Key, Info = row.Value };
                        additionalInfo.AddRange(otherInfo);

                    }

                    gridViewAdditionalInfo.DataSource = additionalInfo.ToArray();
                    gridViewAdditionalInfo.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    gridViewAdditionalInfo.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void ResultsGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.ToggleMoreInfo(chkMoreInfo.Checked);
        }

        private void BtnStopAnalysis_Click(object sender, EventArgs e)
        {
            if (this.httpListernerRunning)
            {
                StopTestingClicked();
            }
        }

        private void btnExportReport_Click(object sender, EventArgs e)
        {
            this.ExportReport();
        }
    }
}

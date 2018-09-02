namespace SecurityTestAssistant
{
    using SecurityTestAssistant.Library.Logic;
    using SecurityTestAssistant.Library.Models;
    using SecurityTestAssistant.Library.Net;
    using SecurityTestAssistant.Library.Testers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Windows.Forms;

    public partial class Main : Form, IApplicationDataProvider
    {

        private const char BULLET_CHAR = '\u2022';
        private string instructions =
            "Important note and disclaimer" +
                $"{Environment.NewLine + "\t" + BULLET_CHAR} This tools decrypts your HTTPs traffic and perform common security analysis on the URLs/pages belong to the above entered domain/host." +
                $"{Environment.NewLine + "\t" + BULLET_CHAR} This decrypted content will NEVER BE SAVED by this tool. However it is recommended that try testing it with your test environment URLs and credentials." +
                $"{Environment.NewLine + "\t" + BULLET_CHAR} This tool acts as a proxy to your HTTP requests" +
                $"{Environment.NewLine + "\t" + BULLET_CHAR}  This tool changes the default proxy of your machine and reverts it back once the tests are over. You can click the Stop button or close the window." +
                $"{Environment.NewLine + "\t" + BULLET_CHAR} Disable HSTS for this URL to let this tool capture the HTTPs traffic. If it is enabled then it cannot decrypt the traffic. " +
                $"{Environment.NewLine + "\t" + BULLET_CHAR} Use the browser in the next tab or any other browser. " +
                $"{Environment.NewLine + "\t" + BULLET_CHAR} [In future] Some tests cannot be performed if other browsers are used to navigate your target URL. " +
                $"{Environment.NewLine + "\t" + BULLET_CHAR} Clear the cookies in the browser before start testing. " +
                $"{Environment.NewLine + "\t\t" + BULLET_CHAR} This is required to ensure the persisted cookies are not sent by the server." +
                $"{Environment.NewLine + "\t\t" + BULLET_CHAR} Once cleared, the server can send cookies." +
                $"{Environment.NewLine + "\t\t" + BULLET_CHAR} Only Cookies sent from server can be analysed.";

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
                    if(txtUrlToAnalyse.CanFocus)
                    {
                        txtUrlToAnalyse.SelectAll();
                        txtUrlToAnalyse.Focus();
                    }
                }
            }
        }

        private void DisplayResults(bool shouldGroup)
        {
            if (shouldGroup)
            {
                var tmpResults = reportDataCollector.Results
                    .GroupBy(r => new { r.TestType, r.FindingType, r.FindingMessage, r.Recommendation })
                    .Select(a => a.FirstOrDefault())
                    .ToList();
                resultBindingSource.DataSource = tmpResults;
                //resultsGrid.DataSource = resultBindingSource;
            }
            else
            {
                resultBindingSource.DataSource = reportDataCollector.Results;
                //resultsGrid.DataSource = resultBindingSource;
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
            lblInstructions.Text = instructions;
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
    }
}

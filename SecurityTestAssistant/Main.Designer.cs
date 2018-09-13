namespace SecurityTestAssistant
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControlMainWindow = new System.Windows.Forms.TabControl();
            this.tabDefine = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpMainWindowNoticeSection = new System.Windows.Forms.GroupBox();
            this.txtMainNotice = new System.Windows.Forms.TextBox();
            this.btnHttpListerner = new System.Windows.Forms.Button();
            this.lblEnterUrlLabel = new System.Windows.Forms.Label();
            this.txtUrlToAnalyse = new System.Windows.Forms.TextBox();
            this.tabBrowse = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.tabFinish = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnExportReport = new System.Windows.Forms.Button();
            this.btnStopAnalysis = new System.Windows.Forms.Button();
            this.chkMoreInfo = new System.Windows.Forms.CheckBox();
            this.chkGroupResults = new System.Windows.Forms.CheckBox();
            this.btnShowResults = new System.Windows.Forms.Button();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.grpBoxResults = new System.Windows.Forms.GroupBox();
            this.resultsGrid = new System.Windows.Forms.DataGridView();
            this.grpBoxMoreInfo = new System.Windows.Forms.GroupBox();
            this.gridViewAdditionalInfo = new System.Windows.Forms.DataGridView();
            this.hostsToListenBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recommendationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resultBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabControlMainWindow.SuspendLayout();
            this.tabDefine.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpMainWindowNoticeSection.SuspendLayout();
            this.tabBrowse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabFinish.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.grpBoxResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsGrid)).BeginInit();
            this.grpBoxMoreInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAdditionalInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hostsToListenBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMainWindow
            // 
            this.tabControlMainWindow.Controls.Add(this.tabDefine);
            this.tabControlMainWindow.Controls.Add(this.tabBrowse);
            this.tabControlMainWindow.Controls.Add(this.tabFinish);
            this.tabControlMainWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMainWindow.Location = new System.Drawing.Point(0, 0);
            this.tabControlMainWindow.Name = "tabControlMainWindow";
            this.tabControlMainWindow.SelectedIndex = 0;
            this.tabControlMainWindow.Size = new System.Drawing.Size(889, 419);
            this.tabControlMainWindow.TabIndex = 9;
            // 
            // tabDefine
            // 
            this.tabDefine.Controls.Add(this.groupBox1);
            this.tabDefine.Location = new System.Drawing.Point(4, 22);
            this.tabDefine.Name = "tabDefine";
            this.tabDefine.Padding = new System.Windows.Forms.Padding(3);
            this.tabDefine.Size = new System.Drawing.Size(881, 393);
            this.tabDefine.TabIndex = 1;
            this.tabDefine.Text = "Define";
            this.tabDefine.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.grpMainWindowNoticeSection);
            this.groupBox1.Controls.Add(this.btnHttpListerner);
            this.groupBox1.Controls.Add(this.lblEnterUrlLabel);
            this.groupBox1.Controls.Add(this.txtUrlToAnalyse);
            this.groupBox1.Location = new System.Drawing.Point(8, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(865, 345);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hosts to listen and test";
            // 
            // grpMainWindowNoticeSection
            // 
            this.grpMainWindowNoticeSection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMainWindowNoticeSection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpMainWindowNoticeSection.Controls.Add(this.txtMainNotice);
            this.grpMainWindowNoticeSection.Location = new System.Drawing.Point(6, 74);
            this.grpMainWindowNoticeSection.Name = "grpMainWindowNoticeSection";
            this.grpMainWindowNoticeSection.Size = new System.Drawing.Size(853, 265);
            this.grpMainWindowNoticeSection.TabIndex = 6;
            this.grpMainWindowNoticeSection.TabStop = false;
            this.grpMainWindowNoticeSection.Text = "Useful notes";
            // 
            // txtMainNotice
            // 
            this.txtMainNotice.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMainNotice.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMainNotice.Location = new System.Drawing.Point(6, 19);
            this.txtMainNotice.Multiline = true;
            this.txtMainNotice.Name = "txtMainNotice";
            this.txtMainNotice.ReadOnly = true;
            this.txtMainNotice.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMainNotice.Size = new System.Drawing.Size(841, 240);
            this.txtMainNotice.TabIndex = 6;
            this.txtMainNotice.Text = "Instructions and notifications";
            // 
            // btnHttpListerner
            // 
            this.btnHttpListerner.AutoSize = true;
            this.btnHttpListerner.Location = new System.Drawing.Point(634, 21);
            this.btnHttpListerner.Name = "btnHttpListerner";
            this.btnHttpListerner.Size = new System.Drawing.Size(96, 23);
            this.btnHttpListerner.TabIndex = 4;
            this.btnHttpListerner.Text = "Start analysis";
            this.btnHttpListerner.UseVisualStyleBackColor = true;
            this.btnHttpListerner.Click += new System.EventHandler(this.BtnHttpListerner_Click);
            // 
            // lblEnterUrlLabel
            // 
            this.lblEnterUrlLabel.AutoSize = true;
            this.lblEnterUrlLabel.Location = new System.Drawing.Point(17, 26);
            this.lblEnterUrlLabel.Name = "lblEnterUrlLabel";
            this.lblEnterUrlLabel.Size = new System.Drawing.Size(60, 13);
            this.lblEnterUrlLabel.TabIndex = 2;
            this.lblEnterUrlLabel.Text = "Http(s) Url: ";
            // 
            // txtUrlToAnalyse
            // 
            this.txtUrlToAnalyse.Location = new System.Drawing.Point(83, 24);
            this.txtUrlToAnalyse.Name = "txtUrlToAnalyse";
            this.txtUrlToAnalyse.Size = new System.Drawing.Size(545, 20);
            this.txtUrlToAnalyse.TabIndex = 3;
            this.txtUrlToAnalyse.MouseEnter += new System.EventHandler(this.TextBox2_MouseEnter);
            // 
            // tabBrowse
            // 
            this.tabBrowse.Controls.Add(this.splitContainer1);
            this.tabBrowse.Location = new System.Drawing.Point(4, 22);
            this.tabBrowse.Name = "tabBrowse";
            this.tabBrowse.Padding = new System.Windows.Forms.Padding(3);
            this.tabBrowse.Size = new System.Drawing.Size(881, 393);
            this.tabBrowse.TabIndex = 0;
            this.tabBrowse.Text = "Browse";
            this.tabBrowse.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnCancel);
            this.splitContainer1.Panel1.Controls.Add(this.btnGo);
            this.splitContainer1.Panel1.Controls.Add(this.txtUrl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.webBrowser1);
            this.splitContainer1.Size = new System.Drawing.Size(875, 387);
            this.splitContainer1.SplitterDistance = 26;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.Location = new System.Drawing.Point(594, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnGo
            // 
            this.btnGo.AutoSize = true;
            this.btnGo.Location = new System.Drawing.Point(555, 0);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(33, 23);
            this.btnGo.TabIndex = 8;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.BtnGo_Click);
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(5, 3);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(544, 20);
            this.txtUrl.TabIndex = 7;
            this.txtUrl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtUrl_KeyPress);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(875, 357);
            this.webBrowser1.TabIndex = 8;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WebBrowser1_DocumentCompleted);
            this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.WebBrowser1_Navigating);
            // 
            // tabFinish
            // 
            this.tabFinish.Controls.Add(this.splitContainer2);
            this.tabFinish.Location = new System.Drawing.Point(4, 22);
            this.tabFinish.Name = "tabFinish";
            this.tabFinish.Size = new System.Drawing.Size(881, 393);
            this.tabFinish.TabIndex = 2;
            this.tabFinish.Text = "Finish";
            this.tabFinish.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btnExportReport);
            this.splitContainer2.Panel1.Controls.Add(this.btnStopAnalysis);
            this.splitContainer2.Panel1.Controls.Add(this.chkMoreInfo);
            this.splitContainer2.Panel1.Controls.Add(this.chkGroupResults);
            this.splitContainer2.Panel1.Controls.Add(this.btnShowResults);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(881, 393);
            this.splitContainer2.SplitterDistance = 36;
            this.splitContainer2.TabIndex = 0;
            // 
            // btnExportReport
            // 
            this.btnExportReport.AutoSize = true;
            this.btnExportReport.Location = new System.Drawing.Point(193, 5);
            this.btnExportReport.Name = "btnExportReport";
            this.btnExportReport.Size = new System.Drawing.Size(124, 23);
            this.btnExportReport.TabIndex = 14;
            this.btnExportReport.Text = "Export as HTML report";
            this.btnExportReport.UseVisualStyleBackColor = true;
            this.btnExportReport.Click += new System.EventHandler(this.btnExportReport_Click);
            // 
            // btnStopAnalysis
            // 
            this.btnStopAnalysis.Location = new System.Drawing.Point(98, 5);
            this.btnStopAnalysis.Name = "btnStopAnalysis";
            this.btnStopAnalysis.Size = new System.Drawing.Size(88, 23);
            this.btnStopAnalysis.TabIndex = 11;
            this.btnStopAnalysis.Text = "Stop analysis";
            this.btnStopAnalysis.UseVisualStyleBackColor = true;
            this.btnStopAnalysis.Click += new System.EventHandler(this.BtnStopAnalysis_Click);
            // 
            // chkMoreInfo
            // 
            this.chkMoreInfo.AutoSize = true;
            this.chkMoreInfo.Checked = true;
            this.chkMoreInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMoreInfo.Location = new System.Drawing.Point(467, 9);
            this.chkMoreInfo.Name = "chkMoreInfo";
            this.chkMoreInfo.Size = new System.Drawing.Size(155, 17);
            this.chkMoreInfo.TabIndex = 13;
            this.chkMoreInfo.Text = "Show additional information";
            this.chkMoreInfo.UseVisualStyleBackColor = true;
            this.chkMoreInfo.CheckedChanged += new System.EventHandler(this.CheckBox2_CheckedChanged);
            // 
            // chkGroupResults
            // 
            this.chkGroupResults.AutoSize = true;
            this.chkGroupResults.Checked = true;
            this.chkGroupResults.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGroupResults.Location = new System.Drawing.Point(373, 9);
            this.chkGroupResults.Name = "chkGroupResults";
            this.chkGroupResults.Size = new System.Drawing.Size(88, 17);
            this.chkGroupResults.TabIndex = 12;
            this.chkGroupResults.Text = "Group results";
            this.chkGroupResults.UseVisualStyleBackColor = true;
            this.chkGroupResults.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // btnShowResults
            // 
            this.btnShowResults.AutoSize = true;
            this.btnShowResults.Location = new System.Drawing.Point(8, 5);
            this.btnShowResults.Name = "btnShowResults";
            this.btnShowResults.Size = new System.Drawing.Size(87, 23);
            this.btnShowResults.TabIndex = 10;
            this.btnShowResults.Text = "Refresh results";
            this.btnShowResults.UseVisualStyleBackColor = true;
            this.btnShowResults.Click += new System.EventHandler(this.BtnShowResults_Click);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.grpBoxResults);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.grpBoxMoreInfo);
            this.splitContainer3.Size = new System.Drawing.Size(881, 353);
            this.splitContainer3.SplitterDistance = 710;
            this.splitContainer3.TabIndex = 1;
            // 
            // grpBoxResults
            // 
            this.grpBoxResults.Controls.Add(this.resultsGrid);
            this.grpBoxResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBoxResults.Location = new System.Drawing.Point(0, 0);
            this.grpBoxResults.Name = "grpBoxResults";
            this.grpBoxResults.Size = new System.Drawing.Size(710, 353);
            this.grpBoxResults.TabIndex = 1;
            this.grpBoxResults.TabStop = false;
            this.grpBoxResults.Text = "Security analysis result";
            // 
            // resultsGrid
            // 
            this.resultsGrid.AllowUserToAddRows = false;
            this.resultsGrid.AllowUserToDeleteRows = false;
            this.resultsGrid.AllowUserToOrderColumns = true;
            this.resultsGrid.AutoGenerateColumns = false;
            this.resultsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn3,
            this.recommendationDataGridViewTextBoxColumn});
            this.resultsGrid.DataSource = this.resultBindingSource;
            this.resultsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultsGrid.Location = new System.Drawing.Point(3, 16);
            this.resultsGrid.Name = "resultsGrid";
            this.resultsGrid.ReadOnly = true;
            this.resultsGrid.Size = new System.Drawing.Size(704, 334);
            this.resultsGrid.TabIndex = 14;
            this.resultsGrid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.ResultsGrid_RowEnter);
            // 
            // grpBoxMoreInfo
            // 
            this.grpBoxMoreInfo.Controls.Add(this.gridViewAdditionalInfo);
            this.grpBoxMoreInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBoxMoreInfo.Location = new System.Drawing.Point(0, 0);
            this.grpBoxMoreInfo.Name = "grpBoxMoreInfo";
            this.grpBoxMoreInfo.Size = new System.Drawing.Size(167, 353);
            this.grpBoxMoreInfo.TabIndex = 0;
            this.grpBoxMoreInfo.TabStop = false;
            this.grpBoxMoreInfo.Text = "More information";
            // 
            // gridViewAdditionalInfo
            // 
            this.gridViewAdditionalInfo.AllowUserToAddRows = false;
            this.gridViewAdditionalInfo.AllowUserToDeleteRows = false;
            this.gridViewAdditionalInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewAdditionalInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridViewAdditionalInfo.Location = new System.Drawing.Point(3, 16);
            this.gridViewAdditionalInfo.Name = "gridViewAdditionalInfo";
            this.gridViewAdditionalInfo.ReadOnly = true;
            this.gridViewAdditionalInfo.Size = new System.Drawing.Size(161, 334);
            this.gridViewAdditionalInfo.TabIndex = 15;
            // 
            // hostsToListenBindingSource
            // 
            this.hostsToListenBindingSource.AllowNew = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "TestType";
            this.dataGridViewTextBoxColumn4.HeaderText = "Test Type";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 80;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "FindingMessage";
            this.dataGridViewTextBoxColumn3.HeaderText = "Message";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // recommendationDataGridViewTextBoxColumn
            // 
            this.recommendationDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.recommendationDataGridViewTextBoxColumn.DataPropertyName = "Recommendation";
            this.recommendationDataGridViewTextBoxColumn.HeaderText = "Recommendation";
            this.recommendationDataGridViewTextBoxColumn.Name = "recommendationDataGridViewTextBoxColumn";
            this.recommendationDataGridViewTextBoxColumn.ReadOnly = true;
            this.recommendationDataGridViewTextBoxColumn.Width = 115;
            // 
            // resultBindingSource
            // 
            this.resultBindingSource.DataSource = typeof(SecurityTestAssistant.Library.Models.AnalysisResult);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 419);
            this.Controls.Add(this.tabControlMainWindow);
            this.Name = "Main";
            this.Text = "Web ApplicationSecurity Test Assistant";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.tabControlMainWindow.ResumeLayout(false);
            this.tabDefine.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpMainWindowNoticeSection.ResumeLayout(false);
            this.grpMainWindowNoticeSection.PerformLayout();
            this.tabBrowse.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabFinish.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.grpBoxResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultsGrid)).EndInit();
            this.grpBoxMoreInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAdditionalInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hostsToListenBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMainWindow;
        private System.Windows.Forms.TabPage tabBrowse;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.TabPage tabDefine;
        private System.Windows.Forms.TabPage tabFinish;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btnShowResults;
        private System.Windows.Forms.DataGridView resultsGrid;
        private System.Windows.Forms.BindingSource resultBindingSource;

        private System.Windows.Forms.CheckBox chkGroupResults;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtUrlToAnalyse;
        private System.Windows.Forms.Label lblEnterUrlLabel;
        private System.Windows.Forms.Button btnHttpListerner;
        private System.Windows.Forms.BindingSource hostsToListenBindingSource;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox grpBoxMoreInfo;
        private System.Windows.Forms.CheckBox chkMoreInfo;
        private System.Windows.Forms.DataGridView gridViewAdditionalInfo;
        private System.Windows.Forms.GroupBox grpBoxResults;
        private System.Windows.Forms.Button btnStopAnalysis;
        private System.Windows.Forms.Button btnExportReport;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn recommendationDataGridViewTextBoxColumn;
        private System.Windows.Forms.GroupBox grpMainWindowNoticeSection;
        private System.Windows.Forms.TextBox txtMainNotice;
    }
}


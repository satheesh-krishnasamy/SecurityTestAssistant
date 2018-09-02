using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecurityTestAssistant
{
    public partial class CookieCleanup : Form
    {
        public CookieCleanup()
        {
            InitializeComponent();
        }

        private void ClearFolder(DirectoryInfo folder)
        {
            foreach (FileInfo file in folder.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo subfolder in folder.GetDirectories())
            {
                ClearFolder(subfolder);
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFolder(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache)));
            }
            catch
            {
                MessageBox.Show(this,
                    "I am unable to clean the cookies/cache files. Please go to internet explorer and clean yourself.",
                    "Failed to clean",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CookieCleanup_Load(object sender, EventArgs e)
        {
            btnCancel.Focus();
        }
    }
}

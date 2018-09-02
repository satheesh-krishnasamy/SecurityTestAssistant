using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTestAssistant.Library.Models
{
    public class TestPage
    {
        public TestPage(
            string pageName,
            string url,
            string title,
            bool isLoginPage,
            bool isAuthenicatedPage,
            IList<PageCookie> cookies)
        {
            this.Page = pageName;
            this.FullUrl = url;
            this.Title = title;
            this.IsAuthenticatedPage = isAuthenicatedPage;
            this.IsLoginPage = IsLoginPage;
            this.Cookies = cookies ?? new List<PageCookie>();
        }

        public string Page { get; set; }
        public string FullUrl { get; set; }
        public string Title { get; set; }
        public bool IsLoginPage { get; set; }
        public bool IsAuthenticatedPage { get; set; }
        public IList<PageCookie> Cookies { get; set; }
    }
}

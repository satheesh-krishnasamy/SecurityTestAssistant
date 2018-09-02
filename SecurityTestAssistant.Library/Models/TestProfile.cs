using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTestAssistant.Library.Models
{
    public class TestProfile
    {
        public TestProfile(string profileName)
        {
            this.ProfileName = profileName;
            this.TestPages = new List<TestPage>();
        }

        public string ProfileName { get; set; }
        public string Domain { get; set; }
        public IList<TestPage> TestPages { get; set; }
    }
}

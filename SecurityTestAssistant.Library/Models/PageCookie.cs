using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTestAssistant.Library.Models
{
    public class PageCookie
    {
        public string Name { get; set; }
        public bool HttpOnly { get; set; }
        public bool IsSessionCookie { get; set; }
        public bool IsSecure { get; set; }
        public DateTime LifeTime { get; set; }
    }
}

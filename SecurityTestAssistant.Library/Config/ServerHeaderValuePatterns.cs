using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTestAssistant.Library.Config
{
    public class ServerHeaderValuePattern: PatternBase
    {
        public ServerHeaderValuePattern(
            string serverOrTechnologyName, 
            string value,
            PatternMatchType pattern): base(value, pattern, "HTTP Header Server")
        {
            this.ServerOrTechnologyName = serverOrTechnologyName;
        }

        public string ServerOrTechnologyName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTestAssistant.Library.Models.Rules
{
    public class RuleResult
    {
        public IDictionary<string, string> RuleNames { get; set; }
    }

    public class Rule
    {
        public string Name { get; set; }
        public FindingType FindingType { get; set; }
        public string GetAnalysisResult(string
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTestAssistant.Library.Config
{
    public enum PatternMatchType
    {
        StartsWith,
        RegEx,
        Equals,
        PresentsAnywhere
    }

    public abstract class PatternBase
    {
        public PatternBase(string patternText, PatternMatchType patternType, string sourceType)
        {
            this.PatternText = patternText;
            this.PatternType = patternType;
            this.SourceType = sourceType;
        }

        public string SourceType { get; set; }
        public string PatternText { get; set; }
        public PatternMatchType PatternType { get; set; }
    }

    public class StringPattern : PatternBase
    {
        public string Technology { get; set; }

        public StringPattern(string name, PatternMatchType sessionIdPattern, string technology, string datasource) :
            base(name, sessionIdPattern, datasource)
        {
            this.Technology = technology;
        }
    }
}

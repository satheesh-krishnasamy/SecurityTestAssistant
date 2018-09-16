namespace SecurityTestAssistant.Library.Utils
{
    using SecurityTestAssistant.Library.Config;
    using System.Collections.Generic;
    using System.Linq;

    internal class PatternMatchResult
    {
        public bool MatchFound { get; internal set; }
        public TechnologyStringPattern MatchingPattern { get; internal set; }
    }


    internal static class PatternMatchUtil
    {
        public static PatternMatchResult CheckPatternMatch(string inputToMatch, IEnumerable<TechnologyStringPattern> patterns)
        {

            PatternMatchResult result = new PatternMatchResult();

            if (!string.IsNullOrWhiteSpace(inputToMatch) && patterns != null && patterns.Count() > 0)
            {
                foreach (var currPattern in patterns)
                {
                    if (PatternComparator.AreMacthesFound(inputToMatch, currPattern))
                    {
                        result.MatchingPattern = currPattern;
                        result.MatchFound = true;
                        return result;
                    }
                }
            }

            result.MatchFound = false;
            return result;
        }

    }
}

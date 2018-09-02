namespace SecurityTestAssistant.Library.Utils
{
    using SecurityTestAssistant.Library.Config;
    using System.Collections.Generic;

    public class PatternMatchResult
    {
        public bool MatchFound { get; internal set; }
        public StringPattern MatchingPattern { get; internal set; }
    }


    public static class PatternMatchUtil
    {
        public static PatternMatchResult CheckPatternMatch(string inputToMatch, IList<StringPattern> patterns)
        {

            PatternMatchResult result = new PatternMatchResult();

            if (!string.IsNullOrWhiteSpace(inputToMatch) && patterns != null && patterns.Count > 0)
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

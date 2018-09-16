namespace SecurityTestAssistant.Library.Config
{
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
}

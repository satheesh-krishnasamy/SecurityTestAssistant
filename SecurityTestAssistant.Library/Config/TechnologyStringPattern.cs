namespace SecurityTestAssistant.Library.Config
{
    public class TechnologyStringPattern : PatternBase
    {
        public string Technology { get; set; }

        public TechnologyStringPattern(string name, PatternMatchType sessionIdPattern, string technology, string datasource) :
            base(name, sessionIdPattern, datasource)
        {
            this.Technology = technology;
        }
    }
}

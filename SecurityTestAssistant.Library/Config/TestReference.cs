namespace SecurityTestAssistant.Library.Tests.Config
{
    using System.Collections.Generic;

    public class TestReference
    {
        public IDictionary<string, IEnumerable<string>> Urls { get; internal set; }

        public TestReference()
        {
            this.Urls = new Dictionary<string, IEnumerable<string>>();
        }
    }
}
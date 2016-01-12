using System.Collections.Generic;

namespace Mascetti
{
    public class LocalizedValuesContext
    {
        public LocalizedValuesContext()
        {
        }

        public LocalizedValuesContext(Dictionary<string, string> matchRules, Dictionary<string,List<LocalizedStringDefinition>> values)
        {
            MatchRules = matchRules;
            Values = values;
        }

        public Dictionary<string, string> MatchRules { get; set; }
        public Dictionary<string, List<LocalizedStringDefinition>> Values { get; set; }
    }
}
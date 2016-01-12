using System.Collections.Generic;

namespace Mascetti
{
    public class LanguageDefinition
    {
        public LanguageDefinition()
        {
        }

        public LanguageDefinition(Dictionary<string, List<LocalizedValueDefinition>> values, List<LocalizedValuesContext> contexts)
        {
            Values = values;
            Contexts = contexts;
        }

        public Dictionary<string, List<LocalizedValueDefinition>> Values { get; set; }
        public List<LocalizedValuesContext> Contexts { get; set; }
    }
}
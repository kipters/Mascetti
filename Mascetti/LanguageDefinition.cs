using System.Collections.Generic;

namespace Mascetti
{
    public class LanguageDefinition
    {
        public Dictionary<string, List<LocalizedStringDefinition>> Values { get; set; }
        public List<LocalizedValuesContext> Contexts { get; set; }
        public string Culture { get; set; }
    }
}
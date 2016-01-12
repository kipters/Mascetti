using System;
using System.Text;

namespace Mascetti
{
    public class LocalizedStringProvider
    {
        private LanguageDefinition _definition;

        public LocalizedStringProvider()
        {
        }

        public LocalizedStringProvider(LanguageDefinition languageDefinition)
        {
            _definition = languageDefinition;
        }
        
        public void Add(LanguageDefinition languageDefinition)
        {
            if (_definition == null)
            {
                _definition = languageDefinition;
                return;
            }

            _definition.Contexts.AddRange(languageDefinition.Contexts);
            foreach (var value in languageDefinition.Values)
                _definition.Values[value.Key] = value.Value;
        }

        public LocalizedStringBuilder Localize(string key) => new LocalizedStringBuilder(_definition, key);
        public LocalizedStringBuilder Localize(string key, int amount) => new LocalizedStringBuilder(_definition, key, amount);
    }
}

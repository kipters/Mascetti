using System;
using System.Text;

namespace Mascetti
{
    public class LocalizedSentenceFactory
    {
        private LanguageDefinition _definition;

        public LocalizedSentenceFactory()
        {
        }

        public LocalizedSentenceFactory(LanguageDefinition languageDefinition)
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

        public LocalizedSentenceBuilder Localize(string key) => new LocalizedSentenceBuilder(_definition, key);
        public LocalizedSentenceBuilder Localize(string key, int amount) => new LocalizedSentenceBuilder(_definition, key, amount);
    }
}

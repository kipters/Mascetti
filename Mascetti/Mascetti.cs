using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mascetti
{
    public class Mascetti
    {
        private LanguageDefinition _definition;

        public void Load(LanguageDefinition languageDefinition)
        {
            _definition = languageDefinition;
        }

        public void Add(LanguageDefinition languageDefinition)
        {
            // TODO
        }

        public LocalizedSentenceBuilder Localize(string key)
        {
            return new LocalizedSentenceBuilder(_definition, key);
        }
    }

    public class LocalizedSentenceBuilder
    {
        private readonly LanguageDefinition _definition;
        private readonly string _key;
        private Dictionary<string, string> _contexts;
        private int _amount;
        private object[] _parameters;

        internal LocalizedSentenceBuilder(LanguageDefinition definition, string key)
        {
            _definition = definition;
            _key = key;
            _contexts = new Dictionary<string, string>();
            _amount = 1;
        }

        public LocalizedSentenceBuilder WithContext(string context, string value)
        {
            _contexts[context] = value;
            return this;
        }

        public LocalizedSentenceBuilder WithContexts(Dictionary<string, string> contexts)
        {
            foreach (var context in contexts)
                _contexts[context.Key] = context.Value;

            return this;
        }

        public LocalizedSentenceBuilder Singular()
        {
            _amount = 1;
            return this;
        }

        public LocalizedSentenceBuilder Plural(int amount)
        {
            _amount = amount;
            return this;
        }

        public LocalizedSentenceBuilder WithParameters(params object[] parameters)
        {
            _parameters = parameters;
            return this;
        }

        public override string ToString()
        {
            var values = _contexts.Count == 0
                ? _definition.Values
                : _definition.Contexts
                    .FirstOrDefault(c =>
                        _contexts.All(r =>
                            _contexts.ContainsKey(r.Key) && _contexts[r.Key] == r.Value))
                    .Values;

            if (values == null)
                throw new KeyNotFoundException("I couldn't find any matching context");

            if (!values.ContainsKey(_key))
                throw new KeyNotFoundException("Key not found in this context");

            var valueDefinition = values[_key];

            if (valueDefinition.Count == 1)
                return string.Format(valueDefinition[0].FormatString, _parameters);


        }
    }

    public class LanguageDefinition
    {
        public Dictionary<string, List<LocalizedValueDefinition>> Values { get; set; }
        public List<LocalizedValuesContext> Contexts { get; set; }
    }

    public class LocalizedValuesContext
    {
        public Dictionary<string, string> MatchRules { get; set; }
        public Dictionary<string, List<LocalizedValueDefinition>> Values { get; set; }
    }

    public class LocalizedValueDefinition
    {
        public int? First { get; set; }
        public int? Last { get; set; }
        public string FormatString { get; set; }
    }
}

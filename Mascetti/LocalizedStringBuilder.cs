using System.Collections.Generic;
using System.Linq;

namespace Mascetti
{
    public class LocalizedStringBuilder
    {
        private readonly LanguageDefinition _definition;
        private readonly string _key;
        private readonly Dictionary<string, string> _contexts;
        private int _amount;
        private List<object> _parameters;

        internal LocalizedStringBuilder(LanguageDefinition definition, string key)
        {
            _definition = definition;
            _key = key;
            _contexts = new Dictionary<string, string>();
            _parameters = new List<object>();
        }

        internal LocalizedStringBuilder(LanguageDefinition definition, string key, int amount)
            : this(definition, key)
        {
            _amount = amount;
            _parameters.Add(amount);
        }

        public LocalizedStringBuilder WithContext(string context, string value)
        {
            _contexts[context] = value;
            return this;
        }

        public LocalizedStringBuilder WithContexts(Dictionary<string, string> contexts)
        {
            foreach (var context in contexts)
                _contexts[context.Key] = context.Value;

            return this;
        }

        public LocalizedStringBuilder Singular()
        {
            _amount = 1;
            return this;
        }

        public LocalizedStringBuilder Plural(int amount)
        {
            _amount = amount;
            return this;
        }

        public LocalizedStringBuilder WithParameters(params object[] parameters)
        {
            _parameters.AddRange(parameters);
            return this;
        }

        public static implicit operator string(LocalizedStringBuilder builder)
        {
            return builder.ToString();
        }

        public override string ToString()
        {
            var values = _contexts.Count == 0
                ? _definition.Values
                : _definition.Contexts
                    .FirstOrDefault(c =>
                        _contexts.All(r =>
                            c.MatchRules.ContainsKey(r.Key) && c.MatchRules[r.Key] == r.Value))
                    ?.Values;

            if (values == null)
                throw new KeyNotFoundException("I couldn't find any matching context");

            if (!values.ContainsKey(_key))
                throw new KeyNotFoundException("Key not found in this context");

            var valueDefinition = values[_key];

            string result;

            if (valueDefinition.Count == 1)
            {
                result = string.Format(valueDefinition[0].FormatString, _parameters.ToArray());
                return result;
            }

            var formatString = valueDefinition.FirstOrDefault(d =>
                (d.First == null || d.First <= _amount) &&
                (d.Last == null || d.Last >= _amount));

            if (formatString == null)
                throw new KeyNotFoundException("I couldn't find any matching pluralization");

            result = string.Format(formatString.FormatString, _parameters.ToArray());

            return result;
        }
    }
}
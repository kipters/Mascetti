using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mascetti.Json
{
    public class InlineDefinitionJsonConverter : JsonConverter
    {
        private readonly StringDefinitionJsonConverter _itemConverter;

        public InlineDefinitionJsonConverter(bool serializeAsArrays = false)
        {
            _itemConverter = new StringDefinitionJsonConverter(serializeAsArrays);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var definitions = value as List<LocalizedStringDefinition>;

            if (definitions == null || definitions.Count == 0)
            {
                writer.WriteNull();
                return;
            }

            if (definitions.Count == 1 && definitions[0].First == null && definitions[0].Last == null)
            {
                writer.WriteValue(definitions[0].FormatString);
                return;
            }

            var rawValue = JsonConvert.SerializeObject(definitions, serializer.Formatting, _itemConverter);
            writer.WriteRawValue(rawValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                var values = serializer.Deserialize<LocalizedStringDefinition[]>(reader);
                return new List<LocalizedStringDefinition>(values);
            }

            if (reader.TokenType != JsonToken.String)
                throw new FormatException($"Expected a string or an object, got a {reader.TokenType} instead");

            var value = (string) reader.Value;
            return new List<LocalizedStringDefinition> {new LocalizedStringDefinition(value)};
        }

        public override bool CanConvert(Type objectType) => objectType == typeof (List<LocalizedStringDefinition>);
    }
}

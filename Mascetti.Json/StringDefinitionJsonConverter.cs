using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mascetti.Json
{
    public class StringDefinitionJsonConverter : JsonConverter
    {
        private readonly bool _useArrays;

        public StringDefinitionJsonConverter(bool useArrays = false)
        {
            _useArrays = useArrays;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var def = value as LocalizedStringDefinition;
            if (def == null)
                return;

            if (def.First == null && def.Last == null)
                writer.WriteValue(def.FormatString);

            if (_useArrays)
            {
                writer.WriteStartArray();
                writer.WriteValue(def.First);
                writer.WriteValue(def.Last);
                writer.WriteValue(def.FormatString);
                writer.WriteEndArray();
                return;
            }

            var rawValue = JsonConvert.SerializeObject(def, serializer.Formatting);
            writer.WriteRawValue(rawValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    var jObject = JObject.Load(reader);
                    return jObject.ToObject<LocalizedStringDefinition>();
                case JsonToken.StartArray:
                {
                    var jArray = JArray.Load(reader);
                    if (jArray.Count != 3)
                        throw new FormatException($"A {nameof(LocalizedStringDefinition)} in array form should have three items");

                    if (jArray[0].Type != JTokenType.Integer && jArray[0].Type != JTokenType.Null)
                        throw new FormatException($"In a {nameof(LocalizedStringDefinition)} in array form the first item must be of type Integer or null");

                    if (jArray[1].Type != JTokenType.Integer && jArray[1].Type != JTokenType.Null)
                        throw new FormatException($"In a {nameof(LocalizedStringDefinition)} in array form the second item must be of type Integer or null");

                    if (jArray[2].Type != JTokenType.String && jArray[2].Type != JTokenType.Null)
                        throw new FormatException($"In a {nameof(LocalizedStringDefinition)} in array form the third item must be of type string or null");

                    var def = new LocalizedStringDefinition
                    {
                        First = jArray[0].ToObject<int?>(),
                        Last = jArray[1].ToObject<int?>(),
                        FormatString = jArray[2].ToObject<string>()
                    };

                    return def;
                }

                default:
                    return null;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            var canConvert = objectType == typeof (LocalizedStringDefinition);
            return canConvert;
        }
    }
}
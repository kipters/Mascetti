using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Mascetti.Json
{
    public class Helper
    {
        public static LanguageDefinition Deserialize(string json)
        {
            //return JsonConvert.DeserializeObject<LanguageDefinition>(json, new LanguageDefinitionJsonConverter());
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            settings.Converters.Add(new InlineDefinitionJsonConverter());
            settings.Converters.Add(new StringDefinitionJsonConverter());
            return JsonConvert.DeserializeObject<LanguageDefinition>(json, settings);
        }

        public static string Serialize(LanguageDefinition definition, bool serializeAsArrays = false)
        {
            return Serialize(definition, Formatting.Indented, serializeAsArrays);
        }

        public static string Serialize(LanguageDefinition definition, Formatting formatting, bool serializeAsArrays = false)
        {
            //var json = JsonConvert.SerializeObject(definition, formatting, new InlineDefinitionJsonConverter(), new StringDefinitionJsonConverter(serializeAsArrays));
            var settings = new JsonSerializerSettings();// {TypeNameHandling = TypeNameHandling.All};
            settings.Converters.Add(new InlineDefinitionJsonConverter(serializeAsArrays));
            settings.Converters.Add(new StringDefinitionJsonConverter(serializeAsArrays));
            var json = JsonConvert.SerializeObject(definition, formatting, settings);
            return json;
        }
    }
}

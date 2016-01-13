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
            return JsonConvert.DeserializeObject<LanguageDefinition>(json, settings);
        }

        public static string Serialize(LanguageDefinition definition)
        {
            return Serialize(definition, Formatting.Indented);
        }

        public static string Serialize(LanguageDefinition definition, Formatting formatting)
        {
            var json = JsonConvert.SerializeObject(definition, formatting, new InlineDefinitionJsonConverter());
            return json;
        }
    }
}

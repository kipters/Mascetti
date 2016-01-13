using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Mascetti;
using Mascetti.Json;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

            //HardcodedSample();
            JsonSample();
        }

        private static void JsonSample()
        {
            var json = File.ReadAllText("sample.json");
            var ld = Helper.Deserialize(json);

            var newJson = Helper.Serialize(ld);
            Console.WriteLine(newJson);
            Console.ReadLine();
        }

        private static void HardcodedSample()
        {
            var commentValues = new List<LocalizedStringDefinition>
            {
                new LocalizedStringDefinition("WAT", null, -1),
                new LocalizedStringDefinition("No comments", 0, 0),
                new LocalizedStringDefinition("One comment", 1, 1),
                new LocalizedStringDefinition("{0} comments", 2)
            };

            var priceValues = new List<LocalizedStringDefinition>
            {
                new LocalizedStringDefinition("Free", null, 0),
                new LocalizedStringDefinition("Price: {0:C}", 1)
            };

            var values = new Dictionary<string, List<LocalizedStringDefinition>>
            {
                {"comments", commentValues},
                {"price", priceValues}
            };

            var maleProfileValues = new List<LocalizedStringDefinition>
            {
                new LocalizedStringDefinition("{0} updated his profile")
            };

            var femaleProfileValues = new List<LocalizedStringDefinition>
            {
                new LocalizedStringDefinition("{0} updated her profile")
            };

            var maleContext = new LocalizedValuesContext(new Dictionary<string, string> {{"gender", "male"}},
                new Dictionary<string, List<LocalizedStringDefinition>> {{"profileUpdate", maleProfileValues}});

            var femaleContext = new LocalizedValuesContext(new Dictionary<string, string> {{"gender", "female"}},
                new Dictionary<string, List<LocalizedStringDefinition>> {{"profileUpdate", femaleProfileValues}});

            var definition = new LanguageDefinition
            {
                Values = values,
                Contexts = new List<LocalizedValuesContext> {maleContext, femaleContext}
            };

            var m = new LocalizedStringProvider(definition);

            Console.WriteLine(m.Localize("comments").Plural(-1).WithParameters(-1).ToString());
            Console.WriteLine(m.Localize("comments").Plural(0).WithParameters(0).ToString());
            Console.WriteLine(m.Localize("comments").Plural(1).WithParameters(1).ToString());
            Console.WriteLine(m.Localize("comments").Plural(2).WithParameters(2).ToString());

            Console.WriteLine(m.Localize("profileUpdate").WithContext("gender", "male").WithParameters("Phil").ToString());
            Console.WriteLine(m.Localize("profileUpdate").WithContext("gender", "female").WithParameters("Lana"));

            string s = m.Localize("price", 0).WithParameters(0);
            string x = m.Localize("price", 10);

            Console.WriteLine($"{s}\n{x}");

            Console.ReadLine();
        }
    }
}

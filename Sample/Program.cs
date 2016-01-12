using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mascetti;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

            var commentValues = new List<LocalizedValueDefinition>
            {
                new LocalizedValueDefinition("WAT", null, -1),
                new LocalizedValueDefinition("No comments", 0, 0),
                new LocalizedValueDefinition("One comment", 1, 1),
                new LocalizedValueDefinition("{0} comments", 2)
            };

            var priceValues = new List<LocalizedValueDefinition>
            {
                new LocalizedValueDefinition("Free", null, 0),
                new LocalizedValueDefinition("Price: {0:C}", 1)
            };

            var values = new Dictionary<string, List<LocalizedValueDefinition>>
            {
                {"comments", commentValues},
                {"price", priceValues}
            };

            var maleProfileValues = new List<LocalizedValueDefinition>
            {
                new LocalizedValueDefinition("{0} updated his profile")
            };

            var femaleProfileValues = new List<LocalizedValueDefinition>
            {
                new LocalizedValueDefinition("{0} updated her profile")
            };

            var maleContext = new LocalizedValuesContext(new Dictionary<string, string> {{"gender", "male"}},
                new Dictionary<string, List<LocalizedValueDefinition>> {{"profileUpdate", maleProfileValues}});

            var femaleContext = new LocalizedValuesContext(new Dictionary<string, string> {{"gender", "female"}},
                new Dictionary<string, List<LocalizedValueDefinition>> {{"profileUpdate", femaleProfileValues}});

            var definition = new LanguageDefinition
            {
                Values = values,
                Contexts = new List<LocalizedValuesContext> {maleContext, femaleContext}
            };

            var m = new LocalizedSentenceFactory(definition);

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

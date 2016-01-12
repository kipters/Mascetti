using System;
using System.Collections.Generic;
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
            var m = new Mascetti.Mascetti();
            m.Load(null);

            m.Localize("aaa")
                .Singular()
                .Plural(4)
                .WithContext("gender", "male")
                .WithParameters(1, 2, "ciao")
                .ToString();
        }
    }
}

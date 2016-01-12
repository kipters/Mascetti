namespace Mascetti
{
    public class LocalizedValueDefinition
    {
        public LocalizedValueDefinition()
        {
        }

        public LocalizedValueDefinition(string formatString, int? first = null, int? last = null)
        {
            First = first;
            Last = last;
            FormatString = formatString;
        }

        public int? First { get; set; }
        public int? Last { get; set; }
        public string FormatString { get; set; }
    }
}
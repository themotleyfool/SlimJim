using System.Xml.Linq;

namespace SlimJim.Infrastructure
{
    public static class XElementExtensions
    {
        public static string ValueOrDefault(this XElement element)
        {
            return element != null ? element.Value : "";
        }
    }
}
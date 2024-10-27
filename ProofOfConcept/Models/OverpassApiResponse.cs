using System.Xml.Linq;

namespace ProofOfConcept.Models
{
    public class OverpassApiResponse
    {
        // List of Elements can be null if the search area is too small, but not in our case
        // because of the range restriction in the front.
        public List<Element> Elements { get; set; }
    }
    public class Element
    {
        public long Id { get; set; }
        public string? Type { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public Center Center { get; set; }
        public Dictionary<string, string> Tags { get; set; }
    }
    public class Center
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}

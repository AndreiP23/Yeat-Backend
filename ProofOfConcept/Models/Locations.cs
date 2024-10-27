namespace ProofOfConcept.Models
{
    public class Locations
    {
        public long Id { get; set; }
        public string? Type { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public Dictionary<string, string>? Tags { get; set; }
    }
}

namespace ProofOfConcept.Models
{
    public class SerpSearchLocation
    {
        public List<Place> Places { get; set; }
    }
    public class Place
    {
        public string title { get; set; }
        public string place_id { get; set; }
        public string data_id { get; set; }
        // Add other relevant properties depending on the API response
    }
}

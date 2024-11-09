namespace ProofOfConcept.Models
{
    public class SerpSearchResponse
    {
        public List<Photos>? Photos { get; set; }
    }
    public class Photos
    {
        // Add more fields id needed
        public string thumbnail { get; set; } 
        public string image { get; set; } 
        public string photo_meta_serpapi_link { get; set; }
    }
}

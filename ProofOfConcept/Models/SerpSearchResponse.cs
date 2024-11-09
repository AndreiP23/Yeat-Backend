namespace ProofOfConcept.Models
{
    public class SerpSearchResponse
    {
        public List<Photo>? Photos { get; set; }
    }
    public class Photo
    {
        // Add more fields id needed
        // Dont know if Url or Title can be null yet
        public string Url { get; set; } 
        public string Title { get; set; }
    }
}

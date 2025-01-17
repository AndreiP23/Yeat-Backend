namespace ProofOfConcept.Models
{
    public class SerpSearchModel
    {
        public List<Place>? place_results { get; set; } // din cauza serp ca nu raspunde cum trb
        public List<Place>? local_results { get; set; }

        public List<Place> GetResults()
        {
            return place_results?.Any() == true ? place_results : local_results;
        }
    }

    public class DayProgram
    {
        public string? day { get; set; }
        public string? hours { get; set; }
    }

    public class ServiceOptions
    {
        public bool dine_in { get; set; }
        public bool takeout { get; set; }
        public bool delivery { get; set; }
    }
}

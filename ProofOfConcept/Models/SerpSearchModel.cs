namespace ProofOfConcept.Models
{
    public class SerpSearchModel
    {
        public Place place_results { get; set; }
    }

    public class GpsCoordinates
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class DayProgram
    {
        public string day { get; set; }
        public string hours { get; set; }
    }

    public class ServiceOptions
    {
        public bool dine_in { get; set; }
        public bool takeout { get; set; }
        public bool delivery { get; set; }
    }
}

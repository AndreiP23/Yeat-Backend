namespace ProofOfConcept.Models
{
    public class SerpSearchModel
    {
        public int position { get; set; }
        public string title { get; set; }
        public string place_id { get; set; }
        public string data_id { get; set; }
        public string data_cid { get; set; }
        public string reviews_link { get; set; }
        public string photos_link { get; set; }
        public GpsCoordinates gps_coordinates { get; set; }
        public string place_id_search { get; set; }
        public string provider_id { get; set; }
        public double rating { get; set; }
        public int reviews { get; set; }
        public string price { get; set; }
        public string type { get; set; }
        public List<string> types { get; set; }
        public string type_id { get; set; }
        public List<string> type_ids { get; set; }
        public string address { get; set; }
        public string open_state { get; set; }
        public string hours { get; set; }
        public OperatingHours operating_hours { get; set; }
        public string phone { get; set; }
        public string website { get; set; }
        public ServiceOptions service_options { get; set; }
        public string order_online { get; set; }
        public string thumbnail { get; set; }
    }

    public class GpsCoordinates
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class OperatingHours
    {
        public string friday { get; set; }
        public string saturday { get; set; }
        public string sunday { get; set; }
        public string monday { get; set; }
        public string tuesday { get; set; }
        public string wednesday { get; set; }
        public string thursday { get; set; }
    }

    public class ServiceOptions
    {
        public bool dine_in { get; set; }
        public bool takeout { get; set; }
        public bool delivery { get; set; }
    }
}

namespace ProofOfConcept.Models
{
    public class SerpSearchLocation
    {
        //public string title { get; set; }
        //public string place_id { get; set; }
        //public string data_id { get; set; }
        public Place place_results { get; set; }
    }
    public class Place
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
        public string[] types { get; set; }
        public string[] type_ids { get; set; }
        public string address { get; set; }
        public string open_state { get; set; }
        public DayProgram[] hours { get; set; }
        //public OperatingHours operating_hours { get; set; }
        public string phone { get; set; }
        public string website { get; set; }
        public ServiceOptions service_options { get; set; }
        public string order_online { get; set; }
        public string thumbnail { get; set; }
        // Add other relevant properties depending on the API response
    }
}

namespace ProofOfConcept.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AtThisLocation
    {
        public int position { get; set; }
        public string title { get; set; }
        public string data_id { get; set; }
        public string data_cid { get; set; }
        public string reviews_link { get; set; }
        public string photos_link { get; set; }
        public GpsCoordinates gps_coordinates { get; set; }
        public string place_id_search { get; set; }
        public double rating { get; set; }
        public string price { get; set; }
        public string thumbnail { get; set; }
    }

    public class GpsCoordinates
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Image
    {
        public string title { get; set; }
        public string thumbnail { get; set; }
    }

    public class PlaceResults
    {
        public string title { get; set; }
        public string place_id { get; set; }
        public string data_id { get; set; }
        public string data_cid { get; set; }
        public string reviews_link { get; set; }
        public string photos_link { get; set; }
        public GpsCoordinates gps_coordinates { get; set; }
        public string place_id_search { get; set; }
        public string provider_id { get; set; }
        public string thumbnail { get; set; }
        public string address { get; set; }
        public Weather weather { get; set; }
        public List<Image> images { get; set; }
        public List<AtThisLocation> at_this_location { get; set; }
        public string web_results_link { get; set; }
        public string serpapi_web_results_link { get; set; }
    }
    public class LocalResults
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
        public bool unclaimed_listing { get; set; }
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
        public string description { get; set; }
        public ServiceOptions service_options { get; set; }
        public string thumbnail { get; set; }
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

    public class Root
    {
        public SearchMetadata search_metadata { get; set; }
        public SearchParameters search_parameters { get; set; }
        public SearchInformation search_information { get; set; }
        public PlaceResults place_results { get; set; }
        public LocalResults[] local_results { get; set; }
    }

    public class SearchInformation
    {
        public string local_results_state { get; set; }
        public string query_displayed { get; set; }
    }

    public class SearchMetadata
    {
        public string id { get; set; }
        public string status { get; set; }
        public string json_endpoint { get; set; }
        public string created_at { get; set; }
        public string processed_at { get; set; }
        public string google_maps_url { get; set; }
        public string raw_html_file { get; set; }
        public double total_time_taken { get; set; }
    }

    public class SearchParameters
    {
        public string engine { get; set; }
        public string type { get; set; }
        public string q { get; set; }
        public string ll { get; set; }
        public string google_domain { get; set; }
        public string hl { get; set; }
    }

    public class Weather
    {
        public string celsius { get; set; }
        public string fahrenheit { get; set; }
        public string conditions { get; set; }
    }
}

using Microsoft.AspNetCore.DataProtection.KeyManagement;
using ProofOfConcept.Models;
using ProofOfConcept.Refit;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProofOfConcept.Services
{
    public interface ISerpService
    {
        Task<List<Photos>> GetMenuPhotosForLocationAsync(string dataId);
        Task<SerpSearchModel> SearchForPlaceAsync(double logitude, double latitude, string name);
        Task<List<MenuFile>> DownloadPhotosLocally(List<Photos> photosGroup);
    }
    public class SerpService : ISerpService
    {
        private readonly ISerpApi _serpApi;
        private readonly IConfiguration _configuration;

        public SerpService(ISerpApi serpApi, IConfiguration configuration)
        {
            _serpApi = serpApi;
            _configuration = configuration;
        }

        public async Task<List<Photos>> GetMenuPhotosForLocationAsync(string dataId)
        {
            try
            {
                var engine = "google_maps_photos";
                var type = "search";
                //var data_id = "0x40b1ff40d8cd3897:0xeaa7b2d9fa6fc399"; this is PizzaHut Bucuresti Universitate for testing;

                var srpApiKey = _configuration["serp-api-key"];

                if (srpApiKey is null)
                    return new List<Photos>();

                var response = await _serpApi.GetLocationMenuPhotosAsync(engine, type, dataId, apiKey: srpApiKey);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.Photos;
                    return jsonResponse;
                }
                else
                {
                    Console.WriteLine($"Error: {response.Error?.Message}");
                    return new List<Photos>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<Photos>();
            }
        }
        public async Task<SerpSearchModel> SearchForPlaceAsync(double logitude, double latitude, string name)
        {
            var engine = "google_maps";
            var type = "search";

            var location = $"@{latitude},{logitude},10z"; // Latitude, Longitude, Zoom
            var srpApiKey = _configuration["serp-api-key"];

            if (srpApiKey is null)
                return new SerpSearchModel();

            var response = await _serpApi.SearchLocationAsync(engine, location, type, srpApiKey, name);

            if (response.IsSuccessStatusCode)
            {
                //place_results??
                var jsonResponse = response.Content;

                var jsonToObj = JsonSerializer.Deserialize<SerpSearchModel>(jsonResponse);
                return jsonToObj;
            }
            else
            {
                Console.WriteLine($"Error: {response.RequestMessage}");
                return new SerpSearchModel();
            }
        }

        public async Task<List<MenuFile>> DownloadPhotosLocally(List<Photos> photosGroup)
        {
            var results = new List<MenuFile>();

            foreach (var photo in photosGroup)
            {
                var filePath = Path.GetTempFileName();
                filePath = Path.ChangeExtension(filePath, "jpg");
                try
                {
                    using (var webClient = new WebClient())
                    {
                        await webClient.DownloadFileTaskAsync(new Uri(photo.image), filePath);
                    }

                    var content = await File.ReadAllBytesAsync(filePath);

                    results.Add(new MenuFile()
                    {
                        Content = content,
                        Name = Path.GetFileName(filePath),
                    });
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    File.Delete(filePath);
                }
            }

            return results;
        }
    }
}

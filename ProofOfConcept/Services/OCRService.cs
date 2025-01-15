using ProofOfConcept.Models;
using ProofOfConcept.Refit;
using Refit;
using System.Text.Json;

namespace ProofOfConcept.Services
{
    public interface IOCRService
    {
        Task<string> GetTextFromPhotosAsync(string dataId, List<string> filePath);
    }
    public class OCRService : IOCRService
    {
        private readonly IOCR _ocrApi;
        public OCRService(IOCR ocrApi)
        {
            _ocrApi = ocrApi;
        }

        public async Task<string> GetTextFromPhotosAsync(string dataId, List<string> filePath)
        {
            string finalResponse = "";
            try
            {
                foreach (var file in filePath)
                {

                    var fileStream = File.OpenRead(file);
                    var filePart = new StreamPart(fileStream, Path.GetFileName(file), contentType: "image/jpg");

                    var response = await _ocrApi.GetTextFromMenuPhotosAsync(dataId, filePart);

                    fileStream.Close();
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = response.Content;

                        var jsonToObj = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonResponse);

                        jsonToObj.TryGetValue("menuFile", out var resu);
                        finalResponse += " " + resu[0];
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.Error?.Message}");
                    }
                }

                return finalResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return string.Empty;
            }
        }
    }
}

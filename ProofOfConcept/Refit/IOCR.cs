using Microsoft.AspNetCore.Mvc;
using ProofOfConcept.Models;
using Refit;

namespace ProofOfConcept.Refit
{
    public interface IOCR
    {
        [Multipart]
        [Post("/ocr-images")]
        Task<ApiResponse<string>> GetTextFromMenuPhotosAsync(string PlaceId,StreamPart menuFile);
    }
}

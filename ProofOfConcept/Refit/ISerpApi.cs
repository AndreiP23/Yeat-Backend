﻿using ProofOfConcept.Models;
using Refit;

namespace ProofOfConcept.Refit
{
    public interface ISerpApi
    {
        [Get("/search")]
        Task<ApiResponse<SerpSearchResponse>> GetLocationMenuPhotosAsync(
          [AliasAs("q")] string locationQuery,             // Location search query this can also be the name of the restaurant
          [AliasAs("category_id")] string category = "CgIYIQ",  // Static "menu" category
          [AliasAs("h1")] string h1 = "ro",  // Country
          [AliasAs("latitude")] double? latitude = null,   // Latitude for location
          [AliasAs("longitude")] double? longitude = null, // Longitude for location
          [AliasAs("api_key")] string apiKey = "<API_KEY>" // API key for authentication
          );

        [Get("/search")]
        Task<HttpResponseMessage> SearchLocationAsync(
        [AliasAs("engine")] string engine,           // Search engine (e.g., google_maps)
        [AliasAs("ll")] string location,             // Latitude and longitude (e.g., "@44.4346528,26.0984804,10z")
        [AliasAs("type")] string type,               // Search type (e.g., search)
        [AliasAs("api_key")] string apiKey,          // Your API key
        [AliasAs("q")] string query                  // Search query (e.g., "Pizza Hut Bucuresti Universitate")
    );
    }
}
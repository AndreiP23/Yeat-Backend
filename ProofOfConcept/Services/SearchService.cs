using FuzzySharp;
using Microsoft.IdentityModel.Tokens;
using ProofOfConcept.Models;
using System.Reflection.Metadata.Ecma335;

namespace ProofOfConcept.Services
{
    public enum BrachSerpResponse
    {
        Place,
        Local
    }
    public interface ISearchService
    {
        Task<List<Locations?>> ProcessLocation(string selectedAmenity, double latitudeUser, double logitudeUser, string userInput);
    }
    public class SearchService : ISearchService
    {
        private readonly ILocationsService _locationsService;
        private readonly ISerpService _serpService;
        private readonly IOCRService _ocrService;
        public SearchService(ILocationsService locationsService, ISerpService serpService, IOCRService ocrService)
        {
            _locationsService = locationsService;
            _serpService = serpService;
            _ocrService = ocrService;
        }

        public abstract class LocationHandler
        {
            protected LocationHandler _nextHandler;

            public LocationHandler SetNext(LocationHandler nextHandler)
            {
                _nextHandler = nextHandler;
                return _nextHandler;
            }

            public abstract Task<bool> HandleAsync(Locations location, string userInput, ISerpService serpService, IOCRService ocrService);
        }

        public class SerpSearchHandler : LocationHandler
        {
            public override async Task<bool> HandleAsync(Locations location, string userInput, ISerpService serpService, IOCRService ocrService)
            {
                var searchResult = await serpService.SearchForPlaceAsync(location.Lon, location.Lat, location.Tags["name"]);

                if(searchResult == null)
                {
                    return false;
                }
                else
                {
                    BrachSerpResponse action = BrachSerpResponse.Place;
                    if (searchResult.place_results == null && searchResult.local_results == null)
                    {
                        return false;
                    }
                    else
                    {
                        
                        switch (searchResult.place_results)
                        {
                            case null:
                                     var isValid = await CheckLocalResults(searchResult.local_results.FirstOrDefault());
                                if (isValid)
                                        action = BrachSerpResponse.Local;
                                break;
                            default:
                                    if (searchResult.place_results.data_id == null)
                                        return false; 
                                    else
                                        action = BrachSerpResponse.Place;
                                break;
                        };

                        if(action == BrachSerpResponse.Place)
                            location.DataId = searchResult.place_results.data_id; // Adaugăm `data_id` pentru procesare ulterioară
                        else
                            location.DataId = searchResult.local_results.First().data_id;

                        return _nextHandler == null || await _nextHandler.HandleAsync(location, userInput, serpService, ocrService);

                    }
                }

                
            }
        }

        public static async Task<bool> CheckLocalResults(LocalResults model)
        {
            switch (model)
            {
                case null:
                    return false;
                default:
                    if (model.data_id == null)
                        return false;
                    break;
            };
            return true;
        }
        public class MenuValidationHandler : LocationHandler
        {
            public override async Task<bool> HandleAsync(Locations location, string userInput, ISerpService serpService, IOCRService ocrService)
            {
                if (location.DataId == null)
                {
                    return false;
                }

                var photosGroup = await serpService.GetMenuPhotosForLocationAsync(location.DataId);

                if (photosGroup == null)
                {
                    return false;
                }

                if (photosGroup.Count == 0)
                {
                    return false;
                }

                var menuFiles = await serpService.DownloadPhotosLocally(photosGroup.Take(3).ToList()); // take 5 is only for development and demo

                var test = location.Tags["name"];
                var resultedText = await ocrService.GetTextFromPhotosAsync(location.DataId, menuFiles.Select(el => el.FilePath).ToList());


                var wordList = resultedText.Split(" ");

                foreach(var word in wordList)
                {
                    if (Fuzz.Ratio(word.ToLower(),userInput.ToLower()) > 75)
                    {
                        return true; // Conținutul meniului conține input-ul utilizatorului
                    }
                }
                return false;
            }
        }

        public async Task<List<Locations?>> ProcessLocation(string selectedAmenity, double latitudeUser, double logitudeUser, string userInput)
        {
            var locations = (await _locationsService.GetAllFoodPlaces(selectedAmenity, latitudeUser, logitudeUser)).Where(e => e.Tags.ContainsKey("name")).Take(10).ToList();

            // Construim lanțul de procesare
            var handlerChain = new SerpSearchHandler();
            handlerChain.SetNext(new MenuValidationHandler());
            var validLocations = new List<Locations>();

            foreach (var location in locations)
            {
                foreach (var word in location.Tags.Values)
                {
                    if (Fuzz.Ratio(word, userInput) > 75)
                    {
                        validLocations.Add(location);
                    }
                }
            }
            // Filtrăm locațiile folosind lanțul
            foreach (var location in locations)
            {
                if (await handlerChain.HandleAsync(location, userInput, _serpService, _ocrService))
                {
                    validLocations.Add(location);
                }
            }

            return validLocations;
        }
    }
}

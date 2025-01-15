using FuzzySharp;
using ProofOfConcept.Models;
using System.Reflection.Metadata.Ecma335;

namespace ProofOfConcept.Services
{
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
                    if (searchResult.place_results == null)
                    {
                        return false;
                        
                    }
                    else
                    {
                        if (searchResult.place_results.data_id == null)
                        {
                            return false; // Locația nu este validă
                        }
                    }
                }

                location.DataId = searchResult.place_results.data_id; // Adaugăm `data_id` pentru procesare ulterioară

                return _nextHandler == null || await _nextHandler.HandleAsync(location, userInput, serpService, ocrService);
            }
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

                if (photosGroup.Count == 0)
                {
                    return false;
                }

                var menuFiles = await serpService.DownloadPhotosLocally(photosGroup.Take(5).ToList()); // take 5 is only for development and demo


                var resultedText = await ocrService.GetTextFromPhotosAsync(location.DataId, menuFiles.Select(el => el.FilePath).ToList());

                if (resultedText.Contains(userInput))
                {
                    return true; // Conținutul meniului conține input-ul utilizatorului
                }

                return false;
            }
        }

        public async Task<List<Locations?>> ProcessLocation(string selectedAmenity, double latitudeUser, double logitudeUser, string userInput)
        {
            var locations = (await _locationsService.GetAllFoodPlaces(selectedAmenity, latitudeUser, logitudeUser)).Take(5).ToList();

            // Construim lanțul de procesare
            var handlerChain = new SerpSearchHandler();
            handlerChain.SetNext(new MenuValidationHandler());

            // Filtrăm locațiile folosind lanțul
            var validLocations = new List<Locations>();
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

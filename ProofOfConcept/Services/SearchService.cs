using FuzzySharp;
using ProofOfConcept.Models;

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
        public SearchService(ILocationsService locationsService, ISerpService serpService)
        {
            _locationsService = locationsService;
            _serpService = serpService;
        }

        public abstract class LocationHandler
        {
            protected LocationHandler _nextHandler;

            public LocationHandler SetNext(LocationHandler nextHandler)
            {
                _nextHandler = nextHandler;
                return _nextHandler;
            }

            public abstract Task<bool> HandleAsync(Locations location, string userInput, ISerpService serpService);
        }

        public class SerpSearchHandler : LocationHandler
        {
            public override async Task<bool> HandleAsync(Locations location, string userInput, ISerpService serpService)
            {
                var searchResult = await serpService.SearchForPlaceAsync(location.Lon, location.Lat, location.Tags["name"]);

                if (searchResult.place_results.data_id == null)
                {
                    return false; // Locația nu este validă
                }

                location.DataId = searchResult.place_results.data_id; // Adaugăm `data_id` pentru procesare ulterioară

                return _nextHandler == null || await _nextHandler.HandleAsync(location, userInput, serpService);
            }
        }
        public class MenuValidationHandler : LocationHandler
        {
            public override async Task<bool> HandleAsync(Locations location, string userInput, ISerpService serpService)
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

                var menuFiles = await serpService.DownloadPhotosLocally(photosGroup);

                foreach (var menuFile in menuFiles)
                {
                    // Aici ar fi un apel OCR (deocamdată simulăm cu un string temporar)
                    var resultedText = "temp";

                    if (Fuzz.Ratio(resultedText, userInput) >= 90)
                    {
                        return true; // Conținutul meniului conține input-ul utilizatorului
                    }
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
                if (await handlerChain.HandleAsync(location, userInput, _serpService))
                {
                    validLocations.Add(location);
                }
            }

            return validLocations;
        }
    }
}

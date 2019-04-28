using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OsloBysykkel.Models;

namespace OsloBysykkel
{
    public class ApiCaller
    {
        Dictionary<int, Station> _cachedStations = new Dictionary<int, Station>();

        public async Task<AvailabilityResult> GetStationsAvailabilities()
        {
            var client = GetClient();
            var httpResult = await client.GetAsync("https://oslobysykkel.no/api/v1/stations/availability");
            var stringContent = await httpResult.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AvailabilityResult>(stringContent);
        }

        //public async Task<Station> GetFullStation(int stationId)
        //{
        //    if (!_cachedStations.ContainsKey(stationId))
        //    {
        //        // get station from API
        //        _cachedStations.Add(stationId, );
        //    }

        //    return _cachedStations.ContainsKey(stationId) 
        //        ? _cachedStations[stationId]
        //        : null;
        //}

        public async Task<StationDescriptionsResult> GetStationsDescriptions()
        {
            var client = GetClient();
            var httpResult = await client.GetAsync("https://oslobysykkel.no/api/v1/stations");
            var stringContent = await httpResult.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StationDescriptionsResult>(stringContent);
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Client-Identifier", "631b838e6bd221dc9f1c04cfad7e5a04");
            return client;
        }
    }
}
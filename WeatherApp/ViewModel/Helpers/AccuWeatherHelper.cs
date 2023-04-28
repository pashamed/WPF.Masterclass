using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.Model;

namespace WeatherApp.ViewModel.Helpers
{
    public class AccuWeatherHelper
    {
        public const string BASE_URL = "http://dataservice.accuweather.com/";
        public const string AUTOCOMPLETE_ENDPOINT = "locations/v1/cities/autocomplete?apikey={0}&q={1}";
        public const string API_KEY = "4ITQCG3tQkMQfAuzkuXSZCrSec5g6pZA";
        public const string CURRENT_CONDITIONS_ENDPOINT = "currentconditions/v1/{0}?apikey={1}";

        public static async Task<List<City>> GetCities(string query)
        {
            var cities = new List<City>();
            
            string url = BASE_URL + string.Format(AUTOCOMPLETE_ENDPOINT, API_KEY,query);
            using(HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var responseStream = await response.Content.ReadAsStreamAsync();
                cities = JsonSerializer.Deserialize<List<City>>(responseStream);
            }

            return cities;           
        }

        public static async Task<CurrentConditions> GetCurrentConditionsAsync(string cityKey)
        {
            var conditions = new CurrentConditions();

            string url = BASE_URL + string.Format(CURRENT_CONDITIONS_ENDPOINT, cityKey, API_KEY);
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var responseStream = await response.Content.ReadAsStreamAsync();
                conditions = JsonSerializer.Deserialize<List<CurrentConditions>>(responseStream).FirstOrDefault() ;
            }

            return conditions;
        }
    }
}

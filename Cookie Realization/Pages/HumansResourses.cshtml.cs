using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_API;

namespace Cookie_Realization.Pages
{
    [Authorize(Policy = "HrManagerOnly")]
    public class Index1Model : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        [BindProperty]
        public List<WeatherForecast> weatherForecastItems { get; set; }

        public Index1Model(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task OnGet()
        {
            var httpClient = httpClientFactory.CreateClient("OurWebAPI");
            weatherForecastItems = await httpClient.GetFromJsonAsync<List<WeatherForecast>>("api/WeatherForecast") ?? new List<WeatherForecast>(); ;
        }
    }
}

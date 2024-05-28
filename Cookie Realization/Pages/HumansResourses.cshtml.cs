using Cookie_Realization.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Web_API;
using Web_API.Controllers;

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
            JwtToken token = new JwtToken();
            var strTokenObj = HttpContext.Session.GetString("access_token");
            if (string.IsNullOrEmpty(strTokenObj))
            {
                token = await Authenticate();
            }
            else
            {
                token = JsonConvert.DeserializeObject<JwtToken>(strTokenObj) ?? new JwtToken();
            }
            if (token == null
                || string.IsNullOrWhiteSpace(token.AccessToken)
                || token.ExpiresAt <= DateTime.UtcNow)
            {
                token = await  Authenticate();
            }


            var httpClient = httpClientFactory.CreateClient("OurWebAPI");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken ?? string.Empty);
            weatherForecastItems = await httpClient.GetFromJsonAsync<List<WeatherForecast>>("api/WeatherForecast") ?? new List<WeatherForecast>();
        }

        private async Task<JwtToken> Authenticate()
        {
            var httpClient = httpClientFactory.CreateClient("OurWebAPI");
            var res = await httpClient.PostAsJsonAsync("https://localhost:7027/api/auth", new Credential { UserName = "admin", Password = "password" });
            res.EnsureSuccessStatusCode();
            string strJwt = await res.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("access_token", strJwt);
            return JsonConvert.DeserializeObject<JwtToken>(strJwt) ?? new JwtToken();
        }
    }
}

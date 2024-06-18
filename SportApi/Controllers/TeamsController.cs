using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SportApi.Models;

namespace SportApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamsController: ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl;
        private readonly string _apiKey;

        public TeamsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _apiBaseUrl = configuration["ApiFootballSettings:BaseUrl"];
            _apiKey = configuration["ApiFootballSettings:ApiKey"];
        }
        [HttpGet]
        public async Task<IActionResult> GetTeams(string team_id = null, string league_id = null)
        {
            var action = "get_teams";
            var query = $"action={action}&APIkey={_apiKey}";

            if (!string.IsNullOrEmpty(team_id))
            {
                query += $"&team_id={team_id}";
            }
            else if (!string.IsNullOrEmpty(league_id))
            {
                query += $"&league_id={league_id}";
            }
            else
            {
                return BadRequest("É necessário fornecer team_id ou league_id.");
            }

            var requestUrl = $"{_apiBaseUrl}?{query}";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Não foi possível obter os dados da equipe.");
            }

            var content = await response.Content.ReadAsStringAsync();
            var teams = JsonConvert.DeserializeObject<List<TeamModel>>(content);

            return Ok(teams);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SportApi.Data;
using SportApi.Models;

namespace SportApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchResultsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MatchResultsController> _logger;


        public MatchResultsController(IHttpClientFactory httpClientFactory, ApplicationDbContext context, IConfiguration configuration, ILogger<MatchResultsController> logger) // Adicione o nome 'logger' ao parâmetro
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> GetAndSaveMatchResult(string from, string to, string? leagueId)
        {
            var apiKey = _configuration.GetValue<string>("ApiFootballSettings:ApiKey");
            var baseUrl = _configuration.GetValue<string>("ApiFootballSettings:BaseUrl");
            var action = "get_events";

            var requestUrl = $"{baseUrl}?action={action}&from={from}&to={to}&league_id={leagueId}&APIkey={apiKey}";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"API Error: {response.StatusCode}. Details: {errorContent}");
                return StatusCode((int)response.StatusCode, $"API Error: {response.StatusCode}. Details: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            try
            {
                var matchResults = JsonConvert.DeserializeObject<List<MatchResult>>(responseContent, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Populate
                });

                foreach (var match in matchResults)
                {
                    _context.MatchResults.Add(match);
                }
                await _context.SaveChangesAsync();
                return Ok("Resultados salvos com sucesso.");
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError(ex, "Erro ao desserializar a resposta da API.");
                return StatusCode(500, "Erro interno ao processar a resposta da API.");
            }
            
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SportApi.Data;
using SportApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public MatchResultsController(IHttpClientFactory httpClientFactory, ApplicationDbContext context, IConfiguration configuration, ILogger<MatchResultsController> logger)
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

        // Get paginated match results
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchResult>>> GetMatchResults([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Page and pageSize must be greater than zero.");
            }

            var matchResults = await _context.MatchResults
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalRecords = await _context.MatchResults.CountAsync();

            var response = new
            {
                TotalRecords = totalRecords,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                Data = matchResults
            };

            return Ok(response);
        }


        // Get a single match result by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<MatchResult>> GetMatchResult(int id)
        {
            var matchResult = await _context.MatchResults.FindAsync(id);

            if (matchResult == null)
            {
                return NotFound();
            }

            return matchResult;
        }

        // Create a new match result
        [HttpPost("create")]
        public async Task<ActionResult<MatchResult>> CreateMatchResult(MatchResult matchResult)
        {
            _context.MatchResults.Add(matchResult);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMatchResult), new { id = matchResult.Id }, matchResult);
        }

        // Update an existing match result
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatchResult(int id, MatchResult matchResult)
        {
            if (id != matchResult.Id)
            {
                return BadRequest();
            }

            _context.Entry(matchResult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchResultExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Delete a match result
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatchResult(int id)
        {
            var matchResult = await _context.MatchResults.FindAsync(id);
            if (matchResult == null)
            {
                return NotFound();
            }

            _context.MatchResults.Remove(matchResult);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MatchResultExists(int id)
        {
            return _context.MatchResults.Any(e => e.Id == id);
        }
    }
}

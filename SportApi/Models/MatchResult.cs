using Newtonsoft.Json;

namespace SportApi.Models
{
    public class MatchResult
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("match_id")]
        public string MatchId { get; set; } = string.Empty;

        [JsonProperty("match_hometeam_name")]
        public string HomeTeam { get; set; } = string.Empty;

        [JsonProperty("match_awayteam_name")]
        public string AwayTeam { get; set; } = string.Empty;

        [JsonProperty("match_hometeam_score")]
        public int HomeScore { get; set; }

        [JsonProperty("match_awayteam_score")]
        public int AwayScore { get; set; }
        
    }

}

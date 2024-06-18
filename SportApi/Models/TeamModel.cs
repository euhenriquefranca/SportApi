using System.Numerics;
using System;
using Newtonsoft.Json;

namespace SportApi.Models
{
    public class TeamModel
    {
        [JsonProperty("team_key")]
        public string TeamKey { get; set; } = string.Empty;

        [JsonProperty("team_name")]
        public string TeamName { get; set; } = string.Empty;

        [JsonProperty("team_country")]
        public string TeamCountry { get; set; } = string.Empty;

        [JsonProperty("team_founded")]
        public string TeamFounded { get; set; } = string.Empty;

        [JsonProperty("team_badge")]
        public string TeamBadge { get; set; } = string.Empty;

        [JsonProperty("venue")]
        public VenueModel Venue { get; set; }

        [JsonProperty("players")]
        public List<PlayerModel> Players { get; set; }

        [JsonProperty("coaches")]
        public List<CoachModel> Coaches { get; set; }
    }
}

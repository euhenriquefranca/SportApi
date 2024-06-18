using Newtonsoft.Json;

namespace SportApi.Models
{
    public class PlayerModel
    {
        [JsonProperty("player_key")]
        public long PlayerKey { get; set; }

        [JsonProperty("player_name")]
        public string PlayerName { get; set; } = string.Empty;

        
        
        [JsonProperty("player_number")]
        public string PlayerNumber { get; set; } = string.Empty;

        [JsonProperty("player_country")]
        public string PlayerCountry { get; set; } = string.Empty;

      
    }
}

using Newtonsoft.Json;

namespace SportApi.Models
{
    public class VenueModel
    {
        [JsonProperty("venue_name")]
        public string VenueName { get; set; } = string.Empty;

        [JsonProperty("venue_address")]
        public string VenueAddress { get; set; } = string.Empty;

        [JsonProperty("venue_city")]
        public string VenueCity { get; set; } = string.Empty;

        [JsonProperty("venue_capacity")]
        public string VenueCapacity { get; set; } = string.Empty;

        [JsonProperty("venue_surface")]
        public string VenueSurface { get; set; } = string.Empty;
    }
}

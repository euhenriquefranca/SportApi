using Newtonsoft.Json;

namespace SportApi.Models
{
    public class CoachModel
    {
        [JsonProperty("coach_name")]
        public string CoachName { get; set; } = string.Empty;

    }
}

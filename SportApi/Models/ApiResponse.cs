namespace SportApi.Models
{
    public class ApiResponse
    {
        public List<MatchResult> Matches { get; set; }
        public string Error { get; set; }
    }
}

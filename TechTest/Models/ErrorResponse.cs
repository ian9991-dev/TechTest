using System.Text.Json.Serialization;

namespace TechTest.Models
{
    public class ErrorResponse
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}

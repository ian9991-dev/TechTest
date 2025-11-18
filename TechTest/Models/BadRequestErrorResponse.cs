using System.Text.Json.Serialization;

namespace TechTest.Models
{
    public class BadRequestErrorResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
       
        [JsonPropertyName("details")]
        public Detail[] Details { get; set; }
    }

    public class Detail
    {
        [JsonPropertyName("field")]
        public string Field { get; set; }
        
        [JsonPropertyName("message")]
        public string Message { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

}

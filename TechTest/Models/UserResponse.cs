using System.Text.Json.Serialization;

namespace TechTest.Models
{
    public class UserResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
        
        [JsonPropertyName("address")]
        public Address? Address { get; set; }
        
        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("createdTimestamp")]
        public DateTime CreatedTimestamp { get; set; }
        [JsonPropertyName("updatedTimestamp")]
        public DateTime UpdatedTimestamp { get; set; }
    }

    public class Address
    {
        [JsonPropertyName("line1")]
        public string? Line1 { get; set; }
        
        [JsonPropertyName("line2")]
        public string? Line2 { get; set; }
        
        [JsonPropertyName("line3")]
        public string? Line3 { get; set; }
        
        [JsonPropertyName("town")]
        public string? Town { get; set; }

        [JsonPropertyName("county")]
        public string? County { get; set; }
        
        [JsonPropertyName("postcode")]
        public string? Postcode { get; set; }
    }

}

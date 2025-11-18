using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TechTest.Models
{
   
    public class TransactionResponse
    {
        [JsonPropertyName("id")]
        [RegularExpression(@"^tan-[A-Za-z0-9]$", ErrorMessage = "Invalid transaction id format. Expected pattern: ^tan-[A-Za-z0-9]$")]
        public string Id { get; set; }

        [JsonPropertyName("amount")]
        [Range(0, 10000, ErrorMessage = "Amount must be between 0 and 10000.")]
        public double Amount { get; set; }

        [JsonPropertyName("currency")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Currency Currency { get; set; }

        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionType Type { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("userId")]
        [RegularExpression(@"^usr-[A-Za-z0-9]+$", ErrorMessage = "Invalid user id format. Expected pattern: ^usr-[A-Za-z0-9]+$")]
        public string UserId { get; set; }

        [JsonPropertyName("createdTimestamp")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedTimestamp { get; set; }
    }

}

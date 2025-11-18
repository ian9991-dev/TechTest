using System.Text.Json.Serialization;

namespace TechTest.Models
{
    public class BankAccountResponse
    {
        [JsonPropertyName("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("sortCode")]
        public string SortCode { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("accountType")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AccountType AccountType { get; set; }

        [JsonPropertyName("balance")]
        public int Balance { get; set; }

        [JsonPropertyName("currency")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Currency Currency { get; set; }

        [JsonPropertyName("createdTimestamp")]
        public DateTime CreatedTimestamp { get; set; }

        [JsonPropertyName("updatedTimestamp")]
        public DateTime UpdatedTimestamp { get; set; }
    }
}

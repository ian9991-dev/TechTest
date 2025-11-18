using System.Text.Json.Serialization;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System;
 
namespace TechTest.Models
{
    public class CreateTransactionRequest
    {
        [JsonPropertyName("amount")]
        [Range(0, 10000, ErrorMessage = "Amount must be between 0 and 10000.")]
        public double Amount { get; set; }
        
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionType? Type { get; set; }
        
        [JsonPropertyName("reference")]
        public string? Reference { get; set; }
    }

}

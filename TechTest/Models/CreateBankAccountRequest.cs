
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace TechTest.Models
{
 
    public class CreateBankAccountRequest
    {
        [JsonPropertyName("name")]
        [Required(ErrorMessage = "name is required.")]
        public string Name { get; set; }
       
        [JsonPropertyName("accountType")]
        [Required(ErrorMessage = "accountType is required.")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AccountType AccountType { get; set; }
    }
}

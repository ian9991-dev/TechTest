using System.Text.Json.Serialization;

namespace TechTest.Models
{
    public class ListBankAccountsResponse
    {
        [JsonPropertyName("accounts")]
        public List<BankAccountResponse> Accounts { get; set; }
    }
}

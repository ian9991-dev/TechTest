using System.Text.Json.Serialization;
using TechTest.Models;

namespace TechTest.Data.Models
{
   
    public class AccountDetails
    {        
        public string AccountNumber { get; set; }
        public string SortCode { get; set; }
        public string Name { get; set; }
        public AccountType AccountType { get; set; }
        public double Balance { get; set; }
        public Currency Currency { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime UpdatedTimestamp { get; set; }
        public string UserId { get; set; }

        public IList<TransactionRecord> Transactions { get; set; } = [];
    }
}

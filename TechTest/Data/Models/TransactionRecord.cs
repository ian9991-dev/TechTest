using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using TechTest.Models;

namespace TechTest.Data.Models
{
    public class TransactionRecord
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public string? Currency { get; set; }
        public TransactionType Type { get; set; }
        public string? Reference { get; set; }
        public DateTime CreatedTimestamp { get; set; }
    }
}

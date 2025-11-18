using Microsoft.AspNetCore.Components.Web;
using TechTest.Data.Models;

namespace TechTest.Data
{
    /// <summary>
    /// Very simple in-memory data access layer for demonstration purposes.
    /// Would be replaced with a real database access layer in a production application.
    /// </summary>
    public class DataAccessLayer : IDataAccessLayer
    {
        private readonly IDictionary<string, AccountDetails> _accounts = new Dictionary<string, AccountDetails>();
        private readonly IDictionary<string, UserDetails> _users = new Dictionary<string, UserDetails>();

        public AccountDetails SaveAccountDetails(AccountDetails accountDetails)
        {
            _accounts[accountDetails.AccountNumber] = accountDetails;
            return accountDetails;
        }

        public bool CheckUserEmailExists(string email)
            => _users.Values.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        public AccountDetails[] GetAllAccountDetails(string userId)
            =>  [.. _accounts.Values.Where(acc=>acc.UserId == userId)];

        public AccountDetails? GetAccount(string accountNumber)
            =>  _accounts.TryGetValue(accountNumber, out var accountDetails) ? accountDetails : null;

        public void DeleteAccount(string accountnumber)
            => _accounts.Remove(accountnumber);

        public UserDetails CreateUser(UserDetails userDetails)
        {
            _users.Add(userDetails.Id, userDetails);
            return userDetails;
        }

        public UserDetails? GetUser(string id)
            => _users.TryGetValue(id, out var userDetails) ? userDetails : null;

        public bool DeleteUser(string userId)
            => _users.Remove(userId);


        public void RecordTransaction(TransactionRecord record, string accountNumber)
            => _accounts[accountNumber].Transactions.Add(record);

        public TransactionRecord GetTransaction(string transactionId, string accountNumber)
            => GetAccount(accountNumber)!.Transactions.FirstOrDefault(t => t.Id == transactionId)!;

        public TransactionRecord[] GetTransactions(string accountNumber)
            => [.. GetAccount(accountNumber)!.Transactions];

       
    }
}

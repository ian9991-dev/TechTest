using TechTest.Data.Models;

namespace TechTest.Data
{
    public class DataAccessLayer : IDataAccessLayer
    {
        private readonly IDictionary<string, AccountDetails> _accounts = new Dictionary<string, AccountDetails>();
        private readonly IDictionary<string, UserDetails> _users = new Dictionary<string, UserDetails>();

        public AccountDetails SaveAccountDetails(AccountDetails accountDetails)
        {
            _accounts[accountDetails.AccountNumber] = accountDetails;
            return accountDetails;
        }

        public AccountDetails[] GetAllAccountDetails(string userId)
        {
            return [.. _accounts.Values.Where(acc=>acc.UserId == userId)];
        }

        public AccountDetails? GetAccount(string accountNumber)
        {
            return _accounts.TryGetValue(accountNumber, out var accountDetails) ? accountDetails : null;
        }

        public void DeleteAccount(string accountnumber)
        {
            _accounts.Remove(accountnumber);
        }

        public UserDetails CreateUser(UserDetails userDetails)
        {
            _users.Add(userDetails.Id, userDetails);
            return userDetails;
        }

        public UserDetails? GetUser(string id)
        {
            return _users.TryGetValue(id, out var userDetails) ? userDetails : null;
        }

        public bool DeleteUser(string userId)
        {
            return _users.Remove(userId);
        }


        public void RecordTransaction(TransactionRecord record, string accountNumber)
        {
            _accounts[accountNumber].Transactions.Add(record);
        }

        public TransactionRecord GetTransaction(string transactionId, string accountNumber)
            => GetAccount(accountNumber)!.Transactions.FirstOrDefault(t => t.Id == transactionId)!;

        public TransactionRecord[] GetTransactions(string accountNumber)
            => [.. GetAccount(accountNumber)!.Transactions];

       
    }
}

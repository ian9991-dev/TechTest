using TechTest.Data.Models;

namespace TechTest.Data
{
    public interface IDataAccessLayer
    { 
        AccountDetails SaveAccountDetails(AccountDetails accountDetails);
        bool CheckUserEmailExists(string email);
        AccountDetails[] GetAllAccountDetails(string userId);
        AccountDetails? GetAccount(string accountNumber);
        UserDetails CreateUser(UserDetails userDetails);
        UserDetails? GetUser(string id);
        bool DeleteUser(string userId);
        void RecordTransaction(TransactionRecord record, string accountNumber);
        TransactionRecord? GetTransaction(string transactionId, string accountNumber);
        TransactionRecord[] GetTransactions(string accountNumber);
    }
}
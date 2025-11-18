using TechTest.Data.Models;
using TechTest.Models;

namespace TechTest.Services
{
    public interface IAccountService
    {
        BankAccountResponse CreateAccount(string name, AccountType accountType, string userId);
        AccountDetails[] GetAccounts(string userId);
        AccountDetails? GetAccount(string accountNumber, string userId);

        TransactionResponse CreateTransaction(string accountNumber, string userId, CreateTransactionRequest createTransactionRequest);
        TransactionResponse GetTransaction(string accountNumber, string userId, string transactionId);
        ListTransactionsResponse GetAllTransaction(string accountNumber, string userId);
    }
}
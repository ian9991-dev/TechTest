using Mapster;
using TechTest.Data;
using TechTest.Data.Models;
using TechTest.Models;

namespace TechTest.Services
{
    public class AccountService(IDataAccessLayer dataAccessLayer, 
                                ICodesGenerator codesGenerator) : IAccountService
    {
        public BankAccountResponse CreateAccount(string name, AccountType accountType, string userId)
        {
            var dateTime = DateTime.UtcNow;
            var accountDetails = new AccountDetails
            {
                AccountNumber = CreateAccountNumber(),
                SortCode = codesGenerator.GenerateSortCode(),
                Name = name,
                AccountType = accountType,
                Balance = 0,
                Currency = Currency.GBP,
                CreatedTimestamp = dateTime,
                UpdatedTimestamp = dateTime,
                UserId = userId
            };

            var details = dataAccessLayer.SaveAccountDetails(accountDetails);
            return details.Adapt<BankAccountResponse>();
        }

        private string CreateAccountNumber()
        {
            do
            {
                string accountNumber = codesGenerator.GenerateAccountNumber();
                if (dataAccessLayer.GetAccount(accountNumber) == null)
                {
                    return accountNumber;
                }
            } 
            while (true);
        }

        public AccountDetails[] GetAccounts(string userId)
            => dataAccessLayer.GetAllAccountDetails(userId);

        public AccountDetails? GetAccount(string accountNumber, string userId) 
            => GetAccountDetails(accountNumber, userId);

        public TransactionResponse CreateTransaction(string accountNumber, string userId, CreateTransactionRequest createTransactionRequest)
        {
            AccountDetails? account = GetAccountDetails(accountNumber, userId);

            var transaction = createTransactionRequest.Adapt<TransactionRecord>();

            transaction.Id = codesGenerator.GenerateTransactionId();
            transaction.CreatedTimestamp = DateTime.UtcNow;
            if (transaction.Type == TransactionType.deposit)
            {
                account.Balance += transaction.Amount;
            }
            else if (transaction.Type == TransactionType.withdrawal)
            {
                var newBalance = account.Balance - transaction.Amount;
                if (newBalance < 0)
                {
                    throw new InsufficientFundsException();
                }
                account.Balance = newBalance;
            }

            dataAccessLayer.RecordTransaction(transaction, accountNumber);
            var response = transaction.Adapt<TransactionResponse>();
            response.UserId = userId;
            return response;
        }

        private AccountDetails GetAccountDetails(string accountNumber, string userId)
        {
            var account = dataAccessLayer.GetAccount(accountNumber) ?? throw new AccountNotFoundException();
            if (account.UserId != userId)
            {
                throw new UserNotAuthorised();
            }
            return account;
        }

        public TransactionResponse GetTransaction(string accountNumber, string userId, string transactionId)
        {
            _ = GetAccountDetails(accountNumber, userId);
            var transaction = dataAccessLayer.GetTransaction(transactionId, accountNumber) ?? throw new TransactionNotFoundException();
            var response = transaction.Adapt<TransactionResponse>();
            response.UserId = userId;
            return response;
        }

        public ListTransactionsResponse GetAllTransaction(string accountNumber, string userId)
        {
            _ = GetAccountDetails(accountNumber, userId);
            var transactions = dataAccessLayer.GetTransactions(accountNumber);
            
            return new ListTransactionsResponse()
            {
                Transactions = [.. transactions.Select(t =>
                {
                    var transaction = t.Adapt<TransactionResponse>();
                    transaction.UserId = userId;
                    return transaction;
                })]
            };
        }
    }
}

using Mapster;
using System.Security.Cryptography;
using TechTest.Data;
using TechTest.Data.Models;
using TechTest.Models;

namespace TechTest.Services
{
    public class AccountService(IDataAccessLayer dataAccessLayer) : IAccountService
    {
        private readonly static Random _random = new();
        public BankAccountResponse CreateAccount(string name, AccountType accountType, string userId)
        {
            var dateTime = DateTime.UtcNow;
            var accountDetails = new AccountDetails
            {
                AccountNumber = GenerateAccountNumber(),
                SortCode = GenerateSortCode(),
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

        public AccountDetails[] GetAccounts(string userId)
            => dataAccessLayer.GetAllAccountDetails(userId);

        public AccountDetails? GetAccount(string accountNumber, string userId) 
            => GetAccountDetails(accountNumber, userId);

        public TransactionResponse CreateTransaction(string accountNumber, string userId, CreateTransactionRequest createTransactionRequest)
        {
            AccountDetails? account = GetAccountDetails(accountNumber, userId);

            var transaction = createTransactionRequest.Adapt<TransactionRecord>();

            transaction.Id = GenerateTransactionId();
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

        private static String GenerateSortCode()
            => $"{Generate2DigigtPart()}-{Generate2DigigtPart()}-{Generate2DigigtPart()}";
        private static string Generate2DigigtPart()
        {
            var random = _random.Next(0, 99);
            return (random < 10) ? "0" + random.ToString() : random.ToString();
        }

        private static string GenerateAccountNumber() 
            => new Random().Next(10000000, 99999999).ToString();


        private static string GenerateTransactionId(int suffixLength = 6)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var result = new char[suffixLength];
            var bytes = new byte[suffixLength];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            for (int i = 0; i < suffixLength; i++)
            {
                var idx = bytes[i] % alphabet.Length;
                result[i] = alphabet[idx];
            }

            return "tan-" + new string(result);
        }

        
    }
}

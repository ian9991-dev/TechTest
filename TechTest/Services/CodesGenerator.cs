using System;
using System.Security.Cryptography;
using System.Text;

namespace TechTest.Services
{
    public class CodesGenerator : ICodesGenerator
    {
        private readonly static Random _random = new();

        public String GenerateSortCode()
            => $"{Generate2DigigtPart()}-{Generate2DigigtPart()}-{Generate2DigigtPart()}";
        public string Generate2DigigtPart()
        {
            var random = _random.Next(0, 99);
            return (random < 10) ? "0" + random.ToString() : random.ToString();
        }

        public string GenerateAccountNumber()
            => new Random().Next(10000000, 99999999).ToString();

        public string GenerateTransactionId(int length = 6)
            => GenerateId("tan-", length);

        public string GenerateUserId(int idLength = 12)
            => GenerateId("usr-", idLength);

        private static string GenerateId(string prefix,int legnth)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var result = new StringBuilder(legnth);
            var bytes = new byte[legnth];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            for (var i = 0; i < legnth; i++)
            {
                result.Append(alphabet[bytes[i] % alphabet.Length]);
            }
            return prefix + result.ToString();
        }
    }
}

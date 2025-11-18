using Mapster;
using System.Security.Cryptography;
using System.Text;
using TechTest.Data;
using TechTest.Data.Models;
using TechTest.Models;

namespace TechTest.Services
{
    public interface IUserService
    {
        UserResponse CreateUser(CreateUserRequest createUser);
        UserResponse GetUser(string userId, string id);
        bool DeleteUser(string userId, string requestedUserId);
    }

    public class UserService(IDataAccessLayer dataAccessLayer, IAccountService accountService) : IUserService
    {
        public UserResponse CreateUser(CreateUserRequest createUserRequest)
        {
            var userDetails = createUserRequest.Adapt<UserDetails>();
            userDetails.Id = GenerateAccountNumber();
            var dateTime = DateTime.UtcNow;
            userDetails.CreatedTimestamp = dateTime;
            userDetails.UpdatedTimestamp = dateTime;
            var response = dataAccessLayer.CreateUser(userDetails);
            return response.Adapt<UserResponse>();
        }

        public UserResponse GetUser(string userId,string id)
        {
            var requestedUser = GetUserDetails(id);
            if (requestedUser.Id != userId)
            {
                throw new UserNotAuthorised();
            }
            return requestedUser.Adapt<UserResponse>();
        }

        private UserDetails GetUserDetails(string id)
        {
            var userDetails = dataAccessLayer.GetUser(id);
            if (userDetails == null)
            {
                throw new UserNotFoundException();
            }
            return userDetails;
        }

        public bool DeleteUser(string userId, string requestedUserId)
        {
            var requestedUser = GetUserDetails(requestedUserId);
            
            if(requestedUser.Id != userId)
            {
                throw new UserNotAuthorised();
            }

            if (accountService.GetAccounts(userId).Any())
            {
                throw new UserCannotBeDeletedException();
            }
            return dataAccessLayer.DeleteUser(userId);
        }
         
        public string GenerateAccountNumber()
        {
            return "usr-lKRJZw1NUYUE";

            const int idLength = 12; // length of the random alphanumeric portion
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var result = new StringBuilder(idLength);
            var bytes = new byte[idLength];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            for (int i = 0; i < idLength; i++)
            {
                var idx = bytes[i] % alphabet.Length;
                result.Append(alphabet[idx]);
            }

            return "usr-" + result.ToString();
        }
    }
}

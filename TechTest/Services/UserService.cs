using Mapster;
using Microsoft.CodeAnalysis;
using TechTest.Data;
using TechTest.Data.Models;
using TechTest.Models;

namespace TechTest.Services
{
    public class UserService(IDataAccessLayer dataAccessLayer, 
                             IAccountService accountService, 
                             ICodesGenerator codesGenerator) : IUserService
    {
        public UserResponse CreateUser(CreateUserRequest createUserRequest)
        {
            var userDetails = createUserRequest.Adapt<UserDetails>();
            userDetails.Id = GenerateUserId();
            var dateTime = DateTime.UtcNow;
            userDetails.CreatedTimestamp = dateTime;
            userDetails.UpdatedTimestamp = dateTime;
            var response = dataAccessLayer.CreateUser(userDetails);
            return response.Adapt<UserResponse>();
        }

        public string GenerateUserId()
        {
            do
            {
                string userId = codesGenerator.GenerateUserId();
                if (dataAccessLayer.GetUser(userId) == null)
                {
                    return userId;
                }
            }
            while (true);
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
            => dataAccessLayer.GetUser(id) ?? throw new UserNotFoundException();

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
    }
}

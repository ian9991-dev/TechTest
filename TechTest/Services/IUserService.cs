using TechTest.Models;

namespace TechTest.Services
{
    public interface IUserService
    {
        UserResponse CreateUser(CreateUserRequest createUser);
        UserResponse GetUser(string userId, string id);
        bool DeleteUser(string userId, string requestedUserId);
    }
}

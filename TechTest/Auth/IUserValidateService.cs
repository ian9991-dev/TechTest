namespace TechTest.Auth
{
    public interface IUserValidateService
    {
        void ValidateUser(string username, string password);
    }
}
namespace TechTest.Services
{
    public interface ICodesGenerator
    {
        string Generate2DigigtPart();
        string GenerateAccountNumber(); 
        string GenerateSortCode();
        string GenerateTransactionId(int suffixLength = 6);
        string GenerateUserId(int idLength = 12);
    }
}

namespace TodoWithDatabase.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string userName, string role, bool testing);
    }
}

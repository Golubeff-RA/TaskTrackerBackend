// Services/Interfaces/IPasswordHasher.cs
namespace YourApp.Services.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
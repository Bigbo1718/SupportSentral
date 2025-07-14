using SupportSentralFrontEnd.Models;

namespace SupportSentralFrontEnd.Interfaces;

public interface IUserClient
{
    Task<User?> GetUserFromEmail(string email);
    Task<List<User>?> GetAllUserAsync();
}
using SupportSentralFrontEnd.Models;

namespace SupportSentralFrontEnd.Interfaces;

public interface IUserClient
{
    Task<User?> GetUserFromEmail(string email);
    Task<User?> GetUserFromId(Guid? Id);
    Task<List<User>?> GetAllUserAsync();
}
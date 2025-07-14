using SupportSentral.Api.Contracts;

namespace SupportSentral.Api.Repositories;

public interface IUserRepository
{
    public Task<List<UserContract>> GetAllAsync();
    public Task<UserContract?> GetByEmailIdAsync(string email);
    public Task<UserContract?> CreateUserAsync(UserContract user);
    public Task<bool> UpdateUser(Guid id, UpdateUserContract user);
}
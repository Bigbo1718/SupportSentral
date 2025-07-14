using SupportSentralFrontEnd.Interfaces;
using SupportSentralFrontEnd.Models;

namespace SupportSentralFrontEnd.Clients;

public class UserClient(HttpClient client) : IUserClient
{
    public async Task<User?> GetUserFromEmail(string email)
    {
        return await client.GetFromJsonAsync<User>($"user/{email}");
        
    }

    public async Task<List<User>?> GetAllUserAsync()
    {
        return await client.GetFromJsonAsync<List<User>>($"user");
    }
}
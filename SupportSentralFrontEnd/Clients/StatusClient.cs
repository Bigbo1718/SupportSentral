using SupportSentralFrontEnd.Models;

namespace SupportSentralFrontEnd.Clients;

public class StatusClient(HttpClient client)
{
    public async Task<List<Status>?> GetStatuses()
    {
        return await client.GetFromJsonAsync<List<Status>>("status");
    }
    public async Task<Status?> GetStatusFromId(int id)
    {
        var statusResponse =  await client.GetFromJsonAsync<Status>($"status/{id}");
        return statusResponse;
    }
}
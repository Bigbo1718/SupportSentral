using SupportSentralFrontEnd.Models;

namespace SupportSentralFrontEnd.Interfaces;

    public interface IStatusClient
    {
        Task<List<Status>?> GetStatuses();
        Task<Status?> GetStatusFromId(int? id);
    }

using SupportSentral.Api.Contracts;
using SupportSentral.Api.Entities;

namespace SupportSentral.Api.Repositories;

public interface ITicketRepository
{
    public Task<List<TicketContract>> GetAllAsync();
    public Task<TicketDetailsContract?> GetByIdAsync(Guid id);
    public Task<TicketDetailsContract?> CreateTicketAsync(CreateTicketContract ticket);
    public Task<bool>UpdateTicketDetailsContract(UpdateTicketDetailsContract ticket, Guid id);
}
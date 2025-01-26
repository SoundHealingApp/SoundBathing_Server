using SoundHealing.Core.Models;

namespace SoundHealing.Core.Interfaces;

public interface IQuoteRepository
{
    public Task AddAsync(Quote quote, CancellationToken cancellationToken);
    
    public Task DeleteAsync(Quote quote, CancellationToken cancellationToken);
    
    public Task<Quote?> GetByIdAsync(Guid quoteId, CancellationToken cancellationToken);

    public Task<List<Quote>> GetAllAsync(CancellationToken cancellationToken);
    
    public Task<Quote?> GetRandomAsync(CancellationToken cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
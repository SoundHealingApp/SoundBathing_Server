using Microsoft.EntityFrameworkCore;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Repositories;

public class QuoteRepository(UserDbContext dbContext) : IQuoteRepository
{
    public async Task AddAsync(Quote quote, CancellationToken cancellationToken)
    {
        await dbContext.Quotes.AddAsync(quote, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Quote quote, CancellationToken cancellationToken)
    {
        dbContext.Quotes.Remove(quote);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Quote?> GetByIdAsync(Guid quoteId, CancellationToken cancellationToken)
    {
        return await dbContext.Quotes.FirstOrDefaultAsync(x => x.Id == quoteId, cancellationToken);
    }
    
    public async Task<List<Quote>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Quotes.ToListAsync(cancellationToken);
    }

    public async Task<Quote?> GetRandomAsync(CancellationToken cancellationToken)
    {
        var totalQuotes = await dbContext.Quotes.CountAsync(cancellationToken);
        
        if (totalQuotes == 0)
            return null;

        var randomIndex = new Random().Next(0, totalQuotes);
        
        var randomQuote = await dbContext.Quotes
            .Skip(randomIndex)
            .Take(1)
            .FirstOrDefaultAsync(cancellationToken);
        
        return randomQuote;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => dbContext.SaveChangesAsync(cancellationToken);
}
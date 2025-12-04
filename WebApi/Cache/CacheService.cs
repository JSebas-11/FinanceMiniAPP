using Microsoft.Extensions.Caching.Memory;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Cache;

internal class CacheService : ICacheService {
    //------------------------INITIALIZATION------------------------
    private readonly IMemoryCache _cache;
    private readonly ITickerRepository _tickerRepository;

    public CacheService(IMemoryCache memoryCache, ITickerRepository tickerRepository) {
        _cache = memoryCache;
        _tickerRepository = tickerRepository;
    }

    //------------------------METHODS------------------------
    public async Task<Ticker?> GetTickerAsync(string symbol) {
        Ticker? ticker;
        
        if (!_cache.TryGetValue(symbol, out ticker)) {
            ticker = await _tickerRepository.GetTickerAsync(symbol);

            if (ticker is null) return null; //No cachear en caso de no tener el ticker en la DB

            _cache.Set(symbol, ticker);
        }

        return ticker;
    }

    public void ClearTickerCache (string symbol) => _cache.Remove(symbol.ToUpperInvariant());
}
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Cache;

internal class CacheService : ICacheService {
    //------------------------INITIALIZATION------------------------
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _opts;
    private readonly ITickerRepository _tickerRepository;

    public CacheService(IDistributedCache distCache, CacheSettings settings, ITickerRepository tickerRepository) {
        _cache = distCache;
        _opts = new DistributedCacheEntryOptions() {
            AbsoluteExpirationRelativeToNow = settings.Expiration
        };
        _tickerRepository = tickerRepository;
    }

    //------------------------METHODS------------------------
    public async Task<Ticker?> GetTickerAsync(string symbol) {
        var json = _cache.GetString(symbol);
        
        if (string.IsNullOrWhiteSpace(json)) {
            var ticker = await _tickerRepository.GetTickerAsync(symbol);
            if (ticker is null) return null; //No cachear en caso de no tener el ticker en la DB

            SetTickerCache(ticker);
            return ticker;
        }
        
        return JsonSerializer.Deserialize<Ticker>(json);
    }

    public void SetTickerCache(Ticker ticker) {
        string symbol = ticker.Symbol;
        ClearTickerCache(symbol);

        var json = JsonSerializer.Serialize(ticker);
        _cache.SetString(symbol, json, _opts);
    }

    public void ClearTickerCache (string symbol) => _cache.Remove(symbol.ToUpperInvariant());
}
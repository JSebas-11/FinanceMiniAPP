using WebApi.Models;

namespace WebApi.Cache;

internal interface ICacheService {
    //------------------------TICKERS------------------------
    Task<Ticker?> GetTickerAsync(string symbol);
    void ClearTickerCache(string symbol);
}
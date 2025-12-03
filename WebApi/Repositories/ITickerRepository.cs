using WebApi.Models;

namespace WebApi.Repositories;

internal interface ITickerRepository {
    public Task<bool> ExistsTickerAsync(string symbol);
    public Task<Ticker?> GetTickerAsync(string symbol);
}
using WebApi.Common;
using WebApi.Models;

namespace WebApi.Repositories;

internal interface ITickerRepository {
    public Task<bool> ExistsTickerAsync(string symbol);
    public Task<Ticker?> GetTickerAsync(string symbol);
    public Task<Result> InsertTickerAsync(Ticker ticker);
    public Task<Result> UpdateTickerAsync(Ticker ticker);
    public Task<Result> DeleteTickerAsync(string symbol);
}
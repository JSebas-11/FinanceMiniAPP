using Shares.Results;
using WebApi.Models;

namespace WebApi.Services;

internal interface ITickerService {
    Task<GenericResult<Ticker>> GetTickerAsync(string symbol);
    Task<Result> CatchUpTickerAsync(string symbol);
}
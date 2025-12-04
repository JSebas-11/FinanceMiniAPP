using WebApi.Common;
using WebApi.Models;

namespace WebApi.Services;

internal interface ITickerService {
    Task<GenericResult<Ticker>> GetTickerAsync(string symbol);
}
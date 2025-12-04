using WebApi.Common;
using WebApi.Models;

namespace WebApi.Services;

internal class TickerService : ITickerService {
    Task<GenericResult<Ticker>> ITickerService.GetTickerAsync(string symbol) {
        throw new NotImplementedException();
    }
}
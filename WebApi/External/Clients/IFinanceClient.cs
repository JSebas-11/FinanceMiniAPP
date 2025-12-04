using WebApi.Common;
using WebApi.DTOs;

namespace WebApi.External.Clients;

internal interface IFinanceApiClient {
    Task<GenericResult<TickerDto>> GetTickerAsync(string symbol);
}
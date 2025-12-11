using Shares.DTOs;
using Shares.Results;

namespace WebApi.External.Clients;

internal interface IFinanceApiClient {
    Task<GenericResult<TickerDto>> GetTickerAsync(string symbol);
}
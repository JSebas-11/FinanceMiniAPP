using Shares.DTOs;
using Shares.Results;

namespace WebApi.External.Clients;

internal interface IArtificialIntelligenceClient {
    Task<GenericResult<string>> GenerateSummarizeAsync(TickerDto ticker);
}
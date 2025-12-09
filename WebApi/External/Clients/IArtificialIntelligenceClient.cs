using WebApi.Common;
using WebApi.DTOs;

namespace WebApi.External.Clients;

internal interface IArtificialIntelligenceClient {
    Task<GenericResult<string>> GenerateSummarizeAsync(TickerDto ticker);
}
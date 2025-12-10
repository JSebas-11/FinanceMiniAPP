using WebApi.Cache;
using WebApi.Common;
using WebApi.External.Clients;
using WebApi.Mapping;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services;

internal class TickerService : ITickerService {
    //------------------------INITIALIZATION------------------------
    private readonly ITickerRepository _tickerRepository;
    private readonly ICacheService _cacheService;
    private readonly IFinanceApiClient _apiClient;
    private readonly IArtificialIntelligenceClient _aiClient;

    public TickerService(
        ITickerRepository tickerRepository, ICacheService cacheService, 
        IFinanceApiClient apiClient, IArtificialIntelligenceClient aiClient
    ) {
        _tickerRepository = tickerRepository;
        _cacheService = cacheService;
        _apiClient = apiClient;
        _aiClient = aiClient;
    }

    //------------------------METHODS------------------------
    public async Task<GenericResult<Ticker>> GetTickerAsync(string symbol) {
        symbol = symbol.Trim().ToUpperInvariant();

        try {
            //Intentar obtenerlo de cache o de la DB
            Ticker? ticker = await _cacheService.GetTickerAsync(symbol);
            if (ticker is not null)
                return GenericResult<Ticker>.Ok("Ticker retrieved from cache", ticker);

            //En este caso no se encuentra en cache, asi que lo pedimos a la API
            var clientResult = await _apiClient.GetTickerAsync(symbol);
            if (!clientResult.Success) 
                return GenericResult<Ticker>.CopyWithNewValue(clientResult, default(Ticker));

            if (clientResult.Value is null) 
                return GenericResult<Ticker>.Fail("Ticker could not be got from API", InternalApiErrors.InternalOperationError);

            //Construir el objeto ticker apartir del DTO, si este no es nulo
            ticker = await TickerDtoMapper.ToModelAsync(clientResult.Value, _aiClient);

            //Guardar en cache y en DB
            var dbInsertion = await _tickerRepository.InsertTickerAsync(ticker);
            if (!dbInsertion.Success) return GenericResult<Ticker>.Fail(dbInsertion.Description, InternalApiErrors.InternalOperationError);

            _cacheService.SetTickerCache(ticker);

            return GenericResult<Ticker>.CopyWithNewValue(clientResult, ticker);
        }
        catch (Exception ex) { 
            return GenericResult<Ticker>.Fail("There has been an error getting ticker", InternalApiErrors.InternalOperationError, ex.Message); 
        }
    }
    public async Task<Result> CatchUpTickerAsync(string symbol) {
        symbol = symbol.Trim().ToUpperInvariant();

        //Verificar que si exista en DB
        if (!await _tickerRepository.ExistsTickerAsync(symbol)) 
            return Result.Fail($"There is no ticker ({symbol}) in DB to update", InternalApiErrors.NotFound);

        //Obtener ticker de API con nuevos datos o retornar el error en caso de haber ocurrido
        var result = await _apiClient.GetTickerAsync(symbol);
        if (!result.Success) return result;
        if (result.Value is null) return Result.Fail("There has been an error getting ticker from API", InternalApiErrors.ExternalApiError);

        //Creacion del Ticker apartir del DTO
        Ticker? ticker;
        try { ticker = await TickerDtoMapper.ToModelAsync(result.Value, _aiClient); }
        catch (Exception) { return Result.Fail("There has been an error creating ticker", InternalApiErrors.CastingError); }

        //Actualizar ticker en DB
        var updateResult = await _tickerRepository.UpdateTickerAsync(ticker);
        if (!updateResult.Success) return updateResult;

        _cacheService.SetTickerCache(ticker);
        return Result.Ok($"Ticker ({symbol}) updated successfully");
    }
}
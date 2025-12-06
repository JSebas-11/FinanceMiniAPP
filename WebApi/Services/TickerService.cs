using WebApi.Builders;
using WebApi.Cache;
using WebApi.Common;
using WebApi.DTOs;
using WebApi.External.Clients;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services;

internal class TickerService : ITickerService {
    //------------------------INITIALIZATION------------------------
    private readonly ITickerRepository _tickerRepository;
    private readonly ICacheService _cacheService;
    private readonly IFinanceApiClient _apiClient;

    public TickerService(ITickerRepository tickerRepository, ICacheService cacheService, IFinanceApiClient apiClient) {
        _tickerRepository = tickerRepository;
        _cacheService = cacheService;
        _apiClient = apiClient;
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
                return GenericResult<Ticker>.CopyWithNewValue(clientResult, default(Ticker)); ;

            if (clientResult.Value is null) 
                return GenericResult<Ticker>.Fail("Ticker could not be got from API", InternalApiErrors.InternalOperationError);

            //Construir el objeto ticker apartir del DTO, si este no es nulo
            ticker = CreateTicker(clientResult.Value);

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

    //------------------------innerMeths------------------------
    private Ticker CreateTicker(TickerDto tickerDto) {
        return new TickerBuilder()
            .WithBasicInfo(tickerDto.Symbol, tickerDto.ShortName, tickerDto.LongName, tickerDto.QuoteType)
            .WithLogisticInfo(tickerDto.Currency, tickerDto.ExchangeName, tickerDto.Region)
            .With52WeeksInfo(tickerDto.FiftyTwoWeekHigh, tickerDto.FiftyTwoWeekLow)
            .WithMarketInfo(tickerDto.MarketPrice, tickerDto.RegularMarketOpen, tickerDto.RegularMarketClose, 
                            tickerDto.RegularMarketVolume, tickerDto.MarketCap, tickerDto.MarketState)
            .WithFundamentals(tickerDto.EpsTtm, tickerDto.EpsForward, tickerDto.ForwardPE, tickerDto.Price2Book, 
                            tickerDto.BookValue, tickerDto.SharesOutstanding)
            .Build();
    }
}
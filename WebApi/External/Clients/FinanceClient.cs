using Shares.DTOs;
using Shares.Enums;
using Shares.Results;
using WebApi.Mapping;
using YahooFinanceApi;

namespace WebApi.External.Clients;

internal class FinanceClient : IFinanceApiClient {
    //------------------------INITIALIZATION------------------------
    public FinanceClient() {
        Yahoo.IgnoreEmptyRows = true;
    }

    //------------------------METHODS------------------------
    public async Task<GenericResult<TickerDto>> GetTickerAsync(string symbol) {
        try {
            // Info recollection
            var data = await Yahoo.Symbols(symbol)
                // Basic
                .Fields(Field.Symbol, Field.ShortName, Field.LongName, Field.QuoteType, 
                        Field.Currency, Field.FullExchangeName)
                // Finance
                .Fields(Field.RegularMarketPrice, Field.RegularMarketOpen, Field.RegularMarketPreviousClose, 
                        Field.MarketCap, Field.MarketState)
                // Ranges
                .Fields(Field.FiftyTwoWeekHigh, Field.FiftyTwoWeekLow)
                // Fundamentals
                .Fields(Field.EpsTrailingTwelveMonths, Field.EpsForward, Field.ForwardPE, Field.PriceToBook, 
                        Field.BookValue, Field.SharesOutstanding)
                .QueryAsync();

            if (!data.ContainsKey(symbol)) return GenericResult<TickerDto>.Fail("Ticker not found", InternalApiErrors.NotFound);

            var dto = TickerDtoMapper.ToDto(data[symbol]);
            
            return GenericResult<TickerDto>.Ok("DTO created", dto);
        }
        catch (Exception ex) { 
            return GenericResult<TickerDto>.Fail("Error getting ticker from API", InternalApiErrors.InternalOperationError, ex.Message); 
        }
    }
}
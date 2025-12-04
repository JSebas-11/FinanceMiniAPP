using WebApi.Common;
using WebApi.DTOs;
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
            // Info recollection (PE Ratio y EPS no estan disponibles en la API y deberan ser calculados)
            var data = await Yahoo.Symbols(symbol)
                // Basic
                .Fields(Field.Symbol, Field.ShortName, Field.LongName, Field.QuoteType, 
                        Field.Currency, Field.Exchange)
                // Finance
                .Fields(Field.RegularMarketPrice, Field.RegularMarketOpen, Field.RegularMarketPreviousClose, 
                        Field.MarketCap, Field.MarketState)
                // Ranges
                .Fields(Field.FiftyTwoWeekHigh, Field.FiftyTwoWeekLow)
                // Fundamentals
                .Fields(Field.EpsTrailingTwelveMonths, Field.EpsForward, Field.ForwardPE, Field.PriceToBook, 
                        Field.BookValue, Field.SharesOutstanding)
                .QueryAsync();

            var dto = TickerDtoMapper.ToDto(data[symbol]);
            
            return GenericResult<TickerDto>.Ok("DTO created", dto);
        }
        catch (Exception ex) { return GenericResult<TickerDto>.Fail("Error getting ticker from API", ex.Message); }
    }
}
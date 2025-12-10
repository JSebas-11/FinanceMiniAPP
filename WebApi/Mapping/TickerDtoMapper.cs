using WebApi.Builders;
using WebApi.Common;
using WebApi.DTOs;
using WebApi.External.Clients;
using WebApi.Models;
using YahooFinanceApi;

namespace WebApi.Mapping;

internal static class TickerDtoMapper {
    //------------------------MAPPING METHODS------------------------
    // FROM DTO object
    public async static Task<Ticker> ToModelAsync(TickerDto dto, IArtificialIntelligenceClient aiClient) {
        var builder = new TickerBuilder()
            .WithBasicInfo(dto.Symbol, dto.ShortName, dto.LongName, dto.QuoteType)
            .WithLogisticInfo(dto.Currency, dto.ExchangeName, dto.Region)
            .With52WeeksInfo(dto.FiftyTwoWeekHigh, dto.FiftyTwoWeekLow)
            .WithMarketInfo(dto.MarketPrice, dto.RegularMarketOpen, dto.RegularMarketClose, 
                            dto.RegularMarketVolume, dto.MarketCap, dto.MarketState)
            .WithFundamentals(dto.EpsTtm, dto.EpsForward, dto.ForwardPE, dto.Price2Book, 
                            dto.BookValue, dto.SharesOutstanding);
        
        //Generacion de resumen con IA y asignacion mediante el builder
        var summResult = await aiClient.GenerateSummarizeAsync(dto);
        string? summarize = summResult.Success ? summResult.Value : null;
        
        return builder.WithIaAnalysis(summarize).Build();
    }

    // FROM FinanceAPI object
    public static TickerDto ToDto(Security security) {
        var parseQuote = Enum.TryParse<QuoteType>(security.QuoteType, true, out var quoteType);
        if (!parseQuote) quoteType = QuoteType.Unknown;

        var parseMState = Enum.TryParse<MarketState>(security.MarketState, true, out var marketState);
        if (!parseMState) marketState = MarketState.Unknown;

        return new TickerDto() {
            Symbol = security.Symbol,
            ShortName = GetStringOrNull(security, "ShortName"),
            LongName = GetStringOrNull(security, "LongName"),
            QuoteType = quoteType,
            Currency = GetStringOrNull(security, "Currency"),
            ExchangeName = GetStringOrNull(security, "FullExchangeName"),
            Region = GetStringOrNull(security, "Region"),

            MarketPrice = GetDecimalOrNull(security, "RegularMarketPrice"),
            RegularMarketOpen = GetDecimalOrNull(security, "RegularMarketOpen"),
            RegularMarketClose = GetDecimalOrNull(security, "RegularMarketPreviousClose"),
            RegularMarketVolume = GetDecimalOrNull(security, "RegularMarketVolume"),
            MarketCap = GetDecimalOrNull(security, "MarketCap"),
            MarketState = marketState,

            FiftyTwoWeekHigh = GetDecimalOrNull(security, "FiftyTwoWeekHigh"),
            FiftyTwoWeekLow = GetDecimalOrNull(security, "FiftyTwoWeekLow"),

            EpsTtm = GetDecimalOrNull(security, "EpsTrailingTwelveMonths"),
            EpsForward = GetDecimalOrNull(security, "EpsForward"),
            ForwardPE = GetDecimalOrNull(security, "ForwardPE"),
            Price2Book = GetDecimalOrNull(security, "PriceToBook"),
            BookValue = GetDecimalOrNull(security, "BookValue"),
            SharesOutstanding = (long?)GetFieldOrNull(security, "SharesOutstanding"),
        };
    }

    //------------------------innerMeths------------------------
    private static object? GetFieldOrNull(Security sec, string key) {
        sec.Fields.TryGetValue(key, out var value);
        return value;
    }
    private static string? GetStringOrNull(Security sec, string key) => GetFieldOrNull(sec, key)?.ToString();
    private static decimal? GetDecimalOrNull(Security sec, string key) {
        var rawObject = GetFieldOrNull(sec, key);
        if (rawObject is null) return null;

        try { return Convert.ToDecimal(rawObject); }
        catch (Exception) { return null; }
    }
}
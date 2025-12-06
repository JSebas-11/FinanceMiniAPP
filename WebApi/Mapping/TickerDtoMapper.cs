using WebApi.Common;
using WebApi.DTOs;
using YahooFinanceApi;

namespace WebApi.Mapping;

internal static class TickerDtoMapper {
    //------------------------MAPPING METHODS------------------------
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
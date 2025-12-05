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
            ShortName = GetFieldOrNull<string>(security, "ShortName"),
            LongName = GetFieldOrNull<string>(security, "LongName"),
            QuoteType = quoteType,
            Currency = GetFieldOrNull<string>(security, "Currency"),
            ExchangeName = GetFieldOrNull<string>(security, "FullExchangeName"),
            Region = GetFieldOrNull<string>(security, "Region"),

            MarketPrice = (decimal?)GetFieldOrNull<double>(security, "RegularMarketPrice"),
            RegularMarketOpen = (decimal?)GetFieldOrNull<double>(security, "RegularMarketOpen"),
            RegularMarketClose = (decimal?)GetFieldOrNull<double>(security, "RegularMarketPreviousClose"),
            RegularMarketVolume = GetFieldOrNull<long>(security, "RegularMarketVolume"),
            MarketCap = GetFieldOrNull<long>(security, "MarketCap"),
            MarketState = marketState,

            FiftyTwoWeekHigh = (decimal?)GetFieldOrNull<double>(security, "FiftyTwoWeekHigh"),
            FiftyTwoWeekLow = (decimal?)GetFieldOrNull<double>(security, "FiftyTwoWeekLow"),

            EpsTtm = (decimal?)GetFieldOrNull<double>(security, "EpsTrailingTwelveMonths"),
            EpsForward = (decimal?)GetFieldOrNull<double>(security, "EpsForward"),
            ForwardPE = (decimal?)GetFieldOrNull<double>(security, "ForwardPE"),
            Price2Book = (decimal?)GetFieldOrNull<double>(security, "PriceToBook"),
            BookValue = (decimal?)GetFieldOrNull<double>(security, "BookValue"),
            SharesOutstanding = GetFieldOrNull<long>(security, "SharesOutstanding"),
        };
    }

    //------------------------innerMeths------------------------
    private static T? GetFieldOrNull<T>(Security sec, string key) {
        if (sec.Fields.TryGetValue(key, out var value) && value is T typed)
            return typed;

        return default;
    }
}
using WebApi.Common;
using WebApi.Models;

namespace WebApi.Builders;

internal class TickerBuilder {
    //-------------------------INITIALIZATION-------------------------
    private readonly Ticker _ticker;
    public TickerBuilder() => _ticker = new Ticker() { LastUpdated = DateTime.Now };

    //-------------------------METHODS-------------------------
    public Ticker Build() {
        if (_ticker.Symbol is null) throw new InvalidOperationException("Ticker does not have Symbol");

        return _ticker;
    }

    #region CreationalMeths
    public TickerBuilder WithBasicInfo(string symbol, string? shortName, string? longName, QuoteType? quoteType) {
        if (string.IsNullOrWhiteSpace(symbol)) throw new ArgumentException("Symbol cannot be null or empty");

        _ticker.Symbol = symbol.Trim().ToUpperInvariant();
        _ticker.ShortName = shortName?.Trim();
        _ticker.LongName = longName?.Trim();
        _ticker.QuoteType = quoteType ?? QuoteType.Unknown;

        return this;
    }
    
    public TickerBuilder WithLogisticInfo(string? currency, string? exchangeName, string? region) {
        _ticker.Currency = currency?.Trim().ToUpperInvariant();
        _ticker.ExchangeName = exchangeName?.Trim();
        _ticker.Region = region?.Trim();

        return this;
    }
    
    public TickerBuilder WithMarketInfo(decimal? price, decimal? open, decimal? close, 
        decimal? volume, decimal? cap, MarketState? state)
    {
        _ticker.MarketPrice = price;
        _ticker.RegularMarketOpen = open;
        _ticker.RegularMarketClose = close;
        _ticker.RegularMarketVolume = volume;
        _ticker.MarketCap = cap;
        _ticker.MarketState = state ?? MarketState.Unknown;

        return this;
    }
    
    public TickerBuilder With52WeeksInfo(decimal? high52Weeks, decimal? low52Weeks) {
        _ticker.FiftyTwoWeekHigh = high52Weeks;
        _ticker.FiftyTwoWeekLow = low52Weeks;

        return this;
    }
    
    public TickerBuilder WithFundamentals(decimal? epsTtm, decimal? epsForward, decimal? 
        peForward, decimal? priceBook, decimal? bookValue, decimal? shares)
    {
        _ticker.EpsTtm = epsTtm;
        _ticker.EpsForward = epsForward;
        _ticker.ForwardPE = peForward;
        _ticker.Price2Book = priceBook;
        _ticker.BookValue = bookValue;
        _ticker.SharesOutstanding = shares;

        return this;
    }
    #endregion
}

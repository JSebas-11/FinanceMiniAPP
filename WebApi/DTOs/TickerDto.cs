using WebApi.Common;

namespace WebApi.DTOs;

internal class TickerDto {
    public string Symbol { get; set; } = null!;
    public string? ShortName { get; set; }
    public string? LongName { get; set; }
    public QuoteType? QuoteType { get; set; }
    public string? Currency { get; set; }
    public string? ExchangeName { get; set; }
    public string? Region { get; set; }

    public decimal? MarketPrice { get; set; }
    public decimal? RegularMarketOpen { get; set; }
    public decimal? RegularMarketClose { get; set; }
    public decimal? RegularMarketVolume { get; set; }
    public decimal? MarketCap { get; set; }
    public MarketState? MarketState { get; set; }
    
    public decimal? FiftyTwoWeekHigh { get; set; }
    public decimal? FiftyTwoWeekLow { get; set; }

    public decimal? EpsTtm { get; set; }
    public decimal? EpsForward { get; set; }
    public decimal? ForwardPE { get; set; }
    public decimal? Price2Book { get; set; }
    public decimal? BookValue { get; set; }
    public long? SharesOutstanding { get; set; }
}
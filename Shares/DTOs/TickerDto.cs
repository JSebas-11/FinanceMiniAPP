using Shares.Enums;

namespace Shares.DTOs;

public class TickerDto {
    // Basic Info
    public string Symbol { get; set; } = null!;
    public string? ShortName { get; set; }
    public string? LongName { get; set; }
    public QuoteType? QuoteType { get; set; }
    public string? Currency { get; set; }
    public string? ExchangeName { get; set; }
    public string? Region { get; set; }

    // Date Info
    public DateTime LastUpdated { get; set; }

    // Financial Info
    public decimal? MarketPrice { get; set; }
    public decimal? RegularMarketOpen { get; set; }
    public decimal? RegularMarketClose { get; set; }
    public decimal? RegularMarketVolume { get; set; }
    public decimal? MarketCap { get; set; }
    public MarketState? MarketState { get; set; }
    
    // Range Info
    public decimal? FiftyTwoWeekHigh { get; set; }
    public decimal? FiftyTwoWeekLow { get; set; }

    // Fundamentals
    public decimal? EpsTtm { get; set; }
    public decimal? EpsForward { get; set; }
    public decimal? ForwardPE { get; set; }
    public decimal? Price2Book { get; set; }
    public decimal? BookValue { get; set; }
    public long? SharesOutstanding { get; set; }

    // IA Analysis
    public string? Summarize { get; set; }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WebApi.Common;

namespace WebApi.Models;

internal class Ticker {
    #region BasicInfo
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("symbol")]
    public string Symbol { get; set; } = null!;

    [BsonElement("short_name")]
    [BsonIgnoreIfNull]
    public string? ShortName { get; set; }

    [BsonElement("long_name")]
    [BsonIgnoreIfNull]
    public string? LongName { get; set; }

    [BsonElement("quote_type")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.String)]
    public QuoteType? QuoteType { get; set; }

    [BsonElement("currency")]
    [BsonIgnoreIfNull]
    public string? Currency { get; set; }

    [BsonElement("exchange_name")]
    [BsonIgnoreIfNull]
    public string? ExchangeName { get; set; }
    
    [BsonElement("region")]
    [BsonIgnoreIfNull]
    public string? Region { get; set; }
    #endregion

    #region DateInfo
    public DateTime LastUpdated { get; set; }
    #endregion

    #region FinanceInfo
    [BsonElement("market_price")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? MarketPrice { get; set; }

    [BsonElement("market_open")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? RegularMarketOpen { get; set; }

    [BsonElement("market_close")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? RegularMarketClose { get; set; }

    [BsonElement("market_volume")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? RegularMarketVolume { get; set; }

    [BsonElement("market_cap")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? MarketCap { get; set; }

    [BsonElement("market_state")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.String)]
    public MarketState? MarketState { get; set; }
    #endregion

    #region RangesInfo
    [BsonElement("fifty_weeks_high")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? FiftyTwoWeekHigh { get; set; }

    [BsonElement("fifty_weeks_low")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? FiftyTwoWeekLow { get; set; }

    // Fundamentals
    [BsonElement("eps_ttm")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? EpsTtm { get; set; }

    [BsonElement("eps_forward")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? EpsForward { get; set; }

    [BsonElement("forward_pe")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? ForwardPE { get; set; }

    [BsonElement("price_book")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? Price2Book { get; set; }

    [BsonElement("book_value")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? BookValue { get; set; }

    [BsonElement("shares")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? SharesOutstanding { get; set; }
    #endregion
}
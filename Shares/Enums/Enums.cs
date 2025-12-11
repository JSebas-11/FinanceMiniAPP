namespace Shares.Enums;

// Enums de uso global para la API y el client 
public enum MarketState { Unknown, Pre, Regular, Closed, Post }
public enum QuoteType { Unknown, Equity, ETF, Index, CryptoCurrency, Fund }
public enum InternalApiErrors { NotFound, CastingError, ExternalApiError, InternalOperationError, NoResponse }
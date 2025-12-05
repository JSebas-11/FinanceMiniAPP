namespace WebApi.Common;

internal enum InternalApiErrors { NotFound, CastingError, ExternalApiError, InternalOperationError }
internal enum MarketState { Unknown, Pre, Regular, Closed, Post }
internal enum QuoteType { Unknown, Equity, ETF, Index, CryptoCurrency, Fund }
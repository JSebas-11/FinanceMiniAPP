using Shares.DTOs;
using Shares.Enums;
using Shares.Results;
using WebApi.External.DTOs;

namespace WebApi.External.Clients;

internal class GeminiClient : IArtificialIntelligenceClient {
    //------------------------INITIALIZATION------------------------
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _endPoint;

    public GeminiClient(HttpClient httpClient, string apiKey) {
        _httpClient = httpClient;
        _apiKey = apiKey.Trim();
        _endPoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-lite:generateContent?key={_apiKey}";
    }

    //------------------------METHODS------------------------
    public async Task<GenericResult<string>> GenerateSummarizeAsync(TickerDto ticker) {
        try {
            string prompt = $"""
            You are a financial analyst. Provide a clear, data-driven analysis of the stock {ticker.Symbol} using only the metrics below.
            If any field is "-", omit it or infer only from related provided values. Do not introduce external data.

            Key Metrics:
            - Current Price: ${GetAsStringOrHyphen(ticker.MarketPrice)}
            - Day's Range: ${GetAsStringOrHyphen(ticker.RegularMarketOpen)} to ${GetAsStringOrHyphen(ticker.RegularMarketClose)}
            - Volume: {GetAsStringOrHyphen(ticker.RegularMarketVolume)}
            - Market Cap: {GetAsStringOrHyphen(ticker.MarketCap)}
            - 52-Week Range: ${GetAsStringOrHyphen(ticker.FiftyTwoWeekLow)} to ${GetAsStringOrHyphen(ticker.FiftyTwoWeekHigh)}
            - Earnings: TTM ${GetAsStringOrHyphen(ticker.EpsTtm)}, Forward ${GetAsStringOrHyphen(ticker.EpsForward)}
            - Valuation: Forward P/E {GetAsStringOrHyphen(ticker.ForwardPE)}, P/B {GetAsStringOrHyphen(ticker.Price2Book)}
            - Book Value: ${GetAsStringOrHyphen(ticker.BookValue)}
            - Shares Outstanding: {GetAsStringOrHyphen(ticker.SharesOutstanding)}

            Provide the analysis in the following structure:
            1. **Valuation** — overvalued/undervalued signals supported by metrics
            2. **Momentum & Liquidity** — volume, ranges, volatility hints
            3. **Risk Indicators** — earnings quality, range compression, valuation risks
            4. **Investment Thesis** — a concise bull vs. bear case based ONLY on the provided data

            Rules:
            - Base every claim strictly on the metrics above.
            - Avoid generic or speculative statements.
            - Do not exceed 350 words.
            - Keep tone analytical, not conversational.
            """;

            //Creacion objeto request siguiendo la estructura que requiere Gemini
            var request = new GeminiRequest(
                [ new Content([ new Part(prompt) ]) ], new GenerationConfig(.7, .95, 40, 700)
            );

            //Enviar solicitud al endpoint
            var response = await _httpClient.PostAsJsonAsync(_endPoint, request);
            response.EnsureSuccessStatusCode();

            //Serializar y obtener respuesta
            var gemResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
            string? summarize = gemResponse?.GetTextResponse();

            //Retornal resultado correspondiente
            return summarize is null 
                ? GenericResult<string>.Fail("Summarize was not generated", InternalApiErrors.NoResponse)
                : GenericResult<string>.Ok("Summarize generated successfully", summarize);
        }
        catch (Exception ex) {
            return GenericResult<string>.Fail("There has been an error generating summarize from gemini",
                InternalApiErrors.ExternalApiError, ex.Message);
        }
    }

    //------------------------aux.Meths------------------------
    private string GetAsStringOrHyphen(object? value) => value?.ToString() ?? "-";
}
using WebApi.Common;
using WebApi.DTOs;

namespace WebApi.External.Clients;

internal class GeminiClient : IArtificialIntelligenceClient {
    //------------------------INITIALIZATION------------------------
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private string _endPoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent";

    public GeminiClient(HttpClient httpClient, string apiKey) {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    //------------------------METHODS------------------------
    public Task<GenericResult<string>> GenerateSummarizeAsync(TickerDto ticker) {
        throw new NotImplementedException();
    }
}
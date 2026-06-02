using System.Text.Json.Serialization;

namespace WebApi.External.DTOs;

#region Common
// CONTENT
internal class Content(Part[] parts) {
    [JsonPropertyName("parts")]
    public Part[] Parts { get; init; } = parts;
}

// PART
internal class Part(string text) {
    [JsonPropertyName("text")]
    public string Text { get; init; } = text;
}
#endregion

#region Request
// REQUEST
internal class GeminiRequest(Content[] contents, GenerationConfig generationConfig) {
    [JsonPropertyName("contents")]
  public Content[] Contents { get; init; } = contents;
    [JsonPropertyName("generationConfig")]
  public GenerationConfig? GenerationConfig { get; init; } = generationConfig;
}

// GENCONFIG
internal class GenerationConfig(double temperature, double topP, int topK, int maxOutputTokens) {
    [JsonPropertyName("temperature")]
    public double Temperature { get; init; } = temperature;
    [JsonPropertyName("topP")]
    public double TopP { get; init; } = topP;
    [JsonPropertyName("topK")]
    public int TopK { get; init; } = topK;
    [JsonPropertyName("maxOutputTokens")]
    public int MaxOutputTokens { get; init; } = maxOutputTokens;
}
#endregion

#region Response
// RESPONSE
internal class GeminiResponse(Candidate[] candidates) {
    [JsonPropertyName("candidates")]
    public Candidate[] Candidates { get; init; } = candidates;

    public string? GetTextResponse() => Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;
}

// CANDIDATE
internal class Candidate(Content content) {
    [JsonPropertyName("content")]
    public Content Content { get; init; } = content;
}
#endregion
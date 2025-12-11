using Shares.Enums;
using Shares.Results;
using WebApi;
using WebApi.Data;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AddAPI
var settings = builder.Configuration.GetSection("MongoSettings").Get<MongoDbSettings>() 
    ?? throw new InvalidOperationException("DB Settings could not be found");

string apiKey = builder.Configuration.GetValue<string>("GeminiApiKey")
    ?? throw new InvalidOperationException("Gemini API Key not configured");

builder.Services.AddMiniFinanceWebApi(settings, apiKey);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// API ENDPOINTS
app.MapGet("/tickers/{symbol}", 
    async (string symbol, ITickerService tickerService) => {
        var result = await tickerService.GetTickerAsync(symbol);

        if (!result.Success) {
            return result.Error switch {
                InternalApiErrors.NotFound => Results.NotFound(result),
                _ => Results.InternalServerError(result),
            };
        }

        return Results.Ok(result.Value);
    }
);

app.MapPost("/tickers/{symbol}/refresh", 
    async (string symbol, ITickerService tickerService) => {
        Result result = await tickerService.CatchUpTickerAsync(symbol);

        if (!result.Success) {
            return result.Error switch {
                InternalApiErrors.NotFound => Results.NotFound(result),
                _ => Results.InternalServerError(result),
            };
        }

        return Results.Ok(result);
    }
);



app.Run();
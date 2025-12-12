using Shares.DTOs;
using Shares.Enums;
using Shares.Results;
using WebApi;
using WebApi.Data;
using WebApi.Mapping;
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

// AddCORS
string[] allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
    ?? throw new InvalidOperationException("CORS is not configured (AllowedOrigins) could not be found");

string clientPolicy = "Client Policy";

builder.Services.AddCors(opts => {
    opts.AddPolicy(clientPolicy, policy => {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(clientPolicy);


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

        return Results.Ok(TickerDtoMapper.ToDto(result.Value!));
    }
)   .WithName("GetTicker")
    .WithTags("Tickers")
    .Produces<TickerDto>(StatusCodes.Status200OK)
    .Produces<Result>(StatusCodes.Status404NotFound)
    .Produces<Result>(StatusCodes.Status500InternalServerError);

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
)   .WithName("RefreshTicker")
    .WithTags("Tickers")
    .Produces<Result>(StatusCodes.Status200OK)
    .Produces<Result>(StatusCodes.Status404NotFound)
    .Produces<Result>(StatusCodes.Status500InternalServerError);

// CORSTEST ENDPOINT
app.MapPost("/cors-test", () => {
    return Results.Ok(new {
       Message = "CORS working propertly",
       Timestampo = DateTime.UtcNow,
       AllowedOrigins = allowedOrigins 
    });
})  .WithName("TestCors")
    .Produces(StatusCodes.Status200OK);


app.Run();
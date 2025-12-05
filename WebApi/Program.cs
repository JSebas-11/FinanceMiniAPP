using WebApi;
using WebApi.Common;
using WebApi.Data;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AddAPI
var settings = builder.Configuration.GetSection("MongoSettings").Get<MongoDbSettings>() 
    ?? throw new InvalidOperationException("DB Settings could not be found");

builder.Services.AddMiniFinanceWebApi(settings);


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

app.Run();
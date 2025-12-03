using WebApi;
using WebApi.Data;

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

app.Run();
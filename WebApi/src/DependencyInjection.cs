using WebApi.Cache;
using WebApi.Data;
using WebApi.External.Clients;
using WebApi.Repositories;
using WebApi.Services;

namespace WebApi;

internal static class DependencyInjection {
    public static IServiceCollection AddMiniFinanceWebApi(
            this IServiceCollection services, 
            MongoDbSettings mongoDbSettings, CacheSettings cacheSettings, 
            string apiKey
        ) {
        // Injeccion DBDriver
        services.AddSingleton(_ => new MongoDriver(mongoDbSettings));

        // Cache
        // services.AddMemoryCache();
        services.AddSingleton(cacheSettings);
        services.AddStackExchangeRedisCache(opts => {
            opts.Configuration = cacheSettings.Configuration;
            opts.InstanceName = cacheSettings.InstanceName;
        });
        services.AddScoped<ICacheService, CacheService>();

        // Repositories
        services.AddScoped<ITickerRepository, TickerRepository>();
        
        // Externals
        services.AddHttpClient();
        services.AddScoped<IFinanceApiClient, FinanceClient>();
        services.AddSingleton<IArtificialIntelligenceClient>(
            sp => new GeminiClient(sp.GetRequiredService<HttpClient>(), apiKey)
        );

        // Services
        services.AddScoped<ITickerService, TickerService>();

        return services;
    }
}
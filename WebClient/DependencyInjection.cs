using WebClient.Services;

namespace WebClient;

public static class DependencyInjection {
    public static IServiceCollection AddMiniFinanceClient(this IServiceCollection services) {
        // Services
        services.AddScoped<IAssetService, AssetService>();

        return services;
    }
}
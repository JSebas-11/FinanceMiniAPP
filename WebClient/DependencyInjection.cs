using WebClient.Services.API.ApiService;

namespace WebClient;

public static class DependencyInjection {
    public static IServiceCollection AddMiniFinanceClient(this IServiceCollection services, string ApiUrl) {
        // Services
        services.AddScoped<IApiService>(sp => {
            var client = new HttpClient(){ BaseAddress = new Uri(ApiUrl) };

            return new ApiService(client);
        });

        return services;
    }
}
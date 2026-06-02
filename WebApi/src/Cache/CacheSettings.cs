namespace WebApi.Cache;

internal class CacheSettings(
    string configuration, 
    string instanceName = "financeCache:", 
    int expiration = 30) 
{
    public string Configuration { get; set; } = configuration;
    public string InstanceName { get; set; } = instanceName;
    public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(expiration);
}
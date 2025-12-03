namespace WebApi.Data;

internal class MongoDbSettings(
    string connectionString, 
    string databaseName = "finance_api_db", 
    string tickersCollectionName = "tickers") 
{
    public string ConnectionString { get; set; } = connectionString;
    public string DatabaseName { get; set; } = databaseName;
    public string TickersCollectionName { get; set; } = tickersCollectionName;
}
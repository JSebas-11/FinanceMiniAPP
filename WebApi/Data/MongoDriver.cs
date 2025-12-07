using MongoDB.Driver;
using WebApi.Models;

namespace WebApi.Data;

internal class MongoDriver {
    //-------------------------INITIALIZATION-------------------------
    private readonly IMongoDatabase _database;

    public IMongoCollection<Ticker> Tickers { get; }
    
    public MongoDriver(MongoDbSettings mongoSettings) {
        var client = new MongoClient(mongoSettings.ConnectionString);
        _database = client.GetDatabase(mongoSettings.DatabaseName);

        Tickers = _database.GetCollection<Ticker>(mongoSettings.TickersCollectionName);

        //Crear Index en campo Symbol
        var indexKeys = Builders<Ticker>.IndexKeys.Ascending(t => t.Symbol);
        var indexOptions = new CreateIndexOptions { Unique = true, Name="index_symbol" };
        
        Tickers.Indexes.CreateOne(new CreateIndexModel<Ticker>(indexKeys, indexOptions));
    }
}
using MongoDB.Driver;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repositories;

internal class TickerRepository : ITickerRepository {
    //------------------------INITIALIZATION------------------------
    private readonly MongoDriver _mongoDriver;
    public TickerRepository(MongoDriver mongoDriver) => _mongoDriver = mongoDriver;

    //------------------------METHODS------------------------
    #region ReadMethods
    public async Task<bool> ExistsTickerAsync(string symbol) {
        Ticker? ticker = await GetTickerAsync(symbol);

        return ticker is not null;
    }

    public async Task<Ticker?> GetTickerAsync(string symbol)
        => await _mongoDriver.Tickers.Find(e => e.Symbol == symbol).FirstOrDefaultAsync();
    #endregion

    #region OpsMethods

    #endregion
}
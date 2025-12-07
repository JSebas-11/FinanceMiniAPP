using MongoDB.Driver;
using WebApi.Common;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repositories;

internal class TickerRepository : ITickerRepository {
    //------------------------INITIALIZATION------------------------
    private readonly MongoDriver _mongoDriver;
    public TickerRepository(MongoDriver mongoDriver) => _mongoDriver = mongoDriver;

    //------------------------METHODS------------------------
    #region ReadMethods
    public async Task<bool> ExistsTickerAsync(string symbol)
        => await _mongoDriver.Tickers.Find(e => e.Symbol == symbol).AnyAsync();

    public async Task<Ticker?> GetTickerAsync(string symbol)
        => await _mongoDriver.Tickers.Find(e => e.Symbol == symbol).FirstOrDefaultAsync();
    #endregion

    #region OpsMethods
    public async Task<Result> InsertTickerAsync(Ticker ticker) {
        try {
            if (await ExistsTickerAsync(ticker.Symbol)) return Result.Fail($"{ticker.Symbol} already exists in DataBase");

            await _mongoDriver.Tickers.InsertOneAsync(ticker);
            return Result.Ok($"Ticker ({ticker.Symbol}) inserted successfully");
        }
        catch (Exception ex) { 
            return Result.Fail($"Error inserting ticker ({ticker.Symbol}) in DB", InternalApiErrors.InternalOperationError, ex.Message); 
        }
    }

    public async Task<Result> UpdateTickerAsync(Ticker ticker) {
        try {
            //Obtener ID del objeto original de la DB
            string? id = await GetTickerIdAsync(ticker.Symbol);
            if (id is null) return Result.Fail($"Ticker {ticker.Symbol}'s id was not found in DataBase");

            //Asignar id correspondiente
            ticker.Id = id;

            var filter = Builders<Ticker>.Filter.Eq(t => t.Symbol, ticker.Symbol);
            
            //Reemplazar todos los campos ya que hay info nueva de la API
            var updateResult = await _mongoDriver.Tickers.ReplaceOneAsync(filter, ticker);

            if (updateResult.MatchedCount == 0) return Result.Fail($"Ticker {ticker.Symbol} was not found in DataBase");

            return Result.Ok($"Ticker ({ticker.Symbol}) updated successfully");
        }
        catch (Exception ex) { 
            return Result.Fail($"Error updating ticker ({ticker.Symbol}) in DB", InternalApiErrors.InternalOperationError, ex.Message); 
        }
    }

    public async Task<Result> DeleteTickerAsync(string symbol) {
        try {
            var delResult = await _mongoDriver.Tickers.DeleteOneAsync(t => t.Symbol == symbol);

            if (delResult.DeletedCount == 0) return Result.Fail($"Ticker {symbol} was not found in DataBase");

            return Result.Ok($"Document Ticker ({symbol}) removed successfully");

        } catch (Exception ex) { 
            return Result.Fail($"Error removing ticker ({symbol}) in DB", InternalApiErrors.InternalOperationError, ex.Message); 
        }
    }
    #endregion

    #region AxuMeths
    public async Task<string?> GetTickerIdAsync(string symbol) {
        Ticker? ticker = await GetTickerAsync(symbol);

        return ticker?.Id;
    }
    #endregion
}
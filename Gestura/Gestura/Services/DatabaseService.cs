 using Gestura.Models;
using SQLite;

namespace Gestura.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitializeAsync()
        {
            try
            {
                await _database.CreateTableAsync<Models.Directory>();
                await _database.CreateTableAsync<DrawingSession>();
                await _database.CreateTableAsync<ImageReference>();
                await _database.CreateTableAsync<DrawingSessionImageReference>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public SQLiteAsyncConnection GetConnection()
        {
            return _database;
        }
    }
}

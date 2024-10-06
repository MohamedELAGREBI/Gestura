using Gestura.Interfaces;
using Gestura.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestura.Repositories
{
    public class DirectoryRepository : IDirectoryRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public DirectoryRepository()
        {
            _database = MauiProgram.Services.GetService<DatabaseService>().GetConnection();
        }

        public async Task<int> CreateDirectoryAsync(Models.Directory directory)
        {
            var existingDirectory = await GetDirectoryByNameAsync(directory.Name);
            if (existingDirectory != null)
            {
                throw new InvalidOperationException($"Le répertoire \"{directory.Name}\" existe déjà.");
            }

            return await _database.InsertAsync(directory);
        }

        public async Task<int> DeleteDirectoryAsync(Models.Directory directory)
        {
            var existingDirectory = await GetDirectoryByIdAsync(directory.Id);
            if (existingDirectory == null)
            {
                throw new InvalidOperationException($"Le répertoire \"{directory.Name}\" avec l'ID valant {directory.Id} n'existe pas en base.");
            }

            return await _database.DeleteAsync(existingDirectory);
        }

        public Task<List<Models.Directory>> GetAllDirectoriesAsync()
        {
            return _database.Table<Models.Directory>().ToListAsync();
        }

        public Task<Models.Directory> GetDirectoryByIdAsync(int id)
        {
            return _database.Table<Models.Directory>().FirstOrDefaultAsync(d => d.Id == id);
        }

        public Task<Models.Directory> GetDirectoryByNameAsync(string name)
        {
            return _database.Table<Models.Directory>().FirstOrDefaultAsync(d => d.Name == name);
        }
    }
}

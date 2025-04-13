using Gestura.Interfaces;
using Gestura.Models;
using Gestura.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestura.Repositories
{
    public class DirectoryRepository : IImageDirectoryRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public DirectoryRepository()
        {
            _database = MauiProgram.Services.GetService<DatabaseService>().GetConnection();
        }

        public Task<int> DeleteAsync(ImageDirectory directory)
        {
            var existingDirectory = GetDirectoryByIdAsync(directory.Id);
            if (existingDirectory == null)
            {
                throw new InvalidOperationException($"Le répertoire \"{directory.Name}\" avec l'ID valant {directory.Id} n'existe pas en base.");
            }

            return _database.DeleteAsync(existingDirectory);
        }

        public Task<List<ImageDirectory>> GetAllAsync()
        {
            return _database.Table<ImageDirectory>().ToListAsync();
        }

        public Task<ImageDirectory> GetDirectoryByIdAsync(int id)
        {
            return _database.Table<ImageDirectory>().FirstOrDefaultAsync(d => d.Id == id);
        }

        public Task<ImageDirectory> GetDirectoryByNameAsync(string name)
        {
            return _database.Table<ImageDirectory>().FirstOrDefaultAsync(d => d.Name == name);
        }

        public async Task<ImageDirectory> InsertAsync(ImageDirectory directory)
        {
            var existingDirectory = await GetDirectoryByNameAsync(directory.Name);
            if (existingDirectory != null)
            {
                throw new InvalidOperationException($"Le répertoire \"{directory.Name}\" existe déjà.");
            }

            await _database.InsertAsync(directory);

            return await GetDirectoryByNameAsync(directory.Name);
        }

        public Task<int> UpdateAsync(ImageDirectory directory)
        {
            return _database.UpdateAsync(directory);
        }
    }
}

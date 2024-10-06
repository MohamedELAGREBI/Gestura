namespace Gestura.Interfaces
{
    public interface IDirectoryRepository
    {
        Task<int> CreateDirectoryAsync(Models.Directory directory);
        Task<int> DeleteDirectoryAsync(Models.Directory directory);
        public Task<Models.Directory> GetDirectoryByIdAsync(int id);
        public Task<Models.Directory> GetDirectoryByNameAsync(string name);
        public Task<List<Models.Directory>> GetAllDirectoriesAsync();
    }
}

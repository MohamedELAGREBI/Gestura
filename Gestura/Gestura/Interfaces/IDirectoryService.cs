namespace Gestura.Interfaces
{
    public interface IDirectoryService
    {
        Task<bool> CreateDirectoryAsync(Models.Directory directory);
        Task<bool> DeleteDirectoryAsync(Models.Directory directory);
        Task<Models.Directory> GetDirectoryByIdAsync(int directoryId);
        Task<Models.Directory> GetDirectoryByNameAsync(string directoryName);
        Task<List<Models.Directory>> GetAllDirectoriesAsync();
    }
}

using Gestura.Interfaces;

namespace Gestura.Services
{
    public class DirectoryService : IDirectoryService
    {
        private readonly IDirectoryRepository _directoryRepository;

        public DirectoryService(IDirectoryRepository directoryRepository)
        {
            _directoryRepository = directoryRepository;
        }

        public async Task<bool> CreateDirectoryAsync(Models.Directory directory)
        {
            return await _directoryRepository.CreateDirectoryAsync(directory) > 0;
        }

        public async Task<bool> DeleteDirectoryAsync(Models.Directory directory)
        {
            return await _directoryRepository.DeleteDirectoryAsync(directory) > 0;
        }

        public async Task<Models.Directory> GetDirectoryByIdAsync(int directoryId)
        {
            return await _directoryRepository.GetDirectoryByIdAsync(directoryId);
        }

        public async Task<Models.Directory> GetDirectoryByNameAsync(string directoryName)
        {
            return await _directoryRepository.GetDirectoryByNameAsync(directoryName);
        }

        public async Task<List<Models.Directory>> GetAllDirectoriesAsync()
        {
            return await _directoryRepository.GetAllDirectoriesAsync();
        }
    }
}

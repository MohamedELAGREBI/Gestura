using Gestura.Commons;
using Gestura.Components;
using Gestura.Interfaces;
using Gestura.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.ViewModels
{
    public class ImageGalleryViewModel : BaseViewModel
    {
        private readonly IImageDirectoryService _directoryService;
        private readonly IImageService _imageService;

        public ObservableCollection<DirectoryComponentViewModel> FilteredDirectories { get; } = new ObservableCollection<DirectoryComponentViewModel>();
        public ICommand OpenImportPopupCommand { get; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                ApplyFilter();
            }
        }

        public ImageGalleryViewModel(IImageDirectoryService directoryService, IImageService imageService)
        {
            _directoryService = directoryService;
            _imageService = imageService;

            OpenImportPopupCommand = new Command(async () =>
            {
                var popup = new ImportImagePopup(_directoryService, _imageService);
                await Shell.Current.Navigation.PushModalAsync(popup);
                // Attendre la fermeture de la popup
                await popup.DisappearingTask;
                // Recharger les répertoires
                await LoadDirectoriesAsync();
            });

            LoadDirectoriesAsync();
        }

        private async Task LoadDirectoriesAsync()
        {
            var dirs = await _directoryService.GetAllDirectoriesAsync();
            FilteredDirectories.Clear();
            foreach (var dir in dirs)
            {
                dir.ImageReferences = await _directoryService.GetImagesForDirectoryAsync(dir.Id);
                if (dir.ImageReferences is null || !dir.ImageReferences.Any())
                {
                    continue;
                }

                var dirCompVm = new DirectoryComponentViewModel(_directoryService, dir);
                dirCompVm.DirectoryDeleted += OnDirectoryDeleted;
                FilteredDirectories.Add(dirCompVm);
            }
        }

        private void OnDirectoryDeleted(object? sender, EventArgs e)
        {
            if (sender is DirectoryComponentViewModel viewModel)
            {
                FilteredDirectories.Remove(viewModel);
            }
        }

        private void ApplyFilter()
        {
            foreach (var dirVM in FilteredDirectories)
            {
                dirVM.IsVisible = string.IsNullOrWhiteSpace(SearchText) ||
                                  dirVM.Directory.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}

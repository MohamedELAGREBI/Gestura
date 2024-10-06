using Gestura.Commons;
using Gestura.Interfaces;
using Gestura.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.ViewModels
{
    public class ImageGalleryViewModel : BaseViewModel, IImageGalleryViewModel
    {
        private readonly IImageService _imageService;
        private readonly IDirectoryService _directoryService;
        private readonly INotificationService _notificationService;

        private ObservableCollection<DirectoryComponentViewModel> _directories;
        public ObservableCollection<DirectoryComponentViewModel> Directories
        {
            get => _directories;
            set => SetProperty(ref _directories, value);
        }

        private string _selectedImportMethod;
        public string SelectedImportMethod
        {
            get => _selectedImportMethod;
            set
            {
                SetProperty(ref _selectedImportMethod, value);
            }
        }

        private string _selectedDirectory;
        public string SelectedDirectory
        {
            get => _selectedDirectory;
            set => SetProperty(ref _selectedDirectory, value);
        }

        private ObservableCollection<string> _importMethods;
        public ObservableCollection<string> ImportMethods
        {
            get => _importMethods;
            set => SetProperty(ref _importMethods, value);
        }

        private ObservableCollection<string> _directoryNames;
        public ObservableCollection<string> DirectoryNames
        {
            get => _directoryNames;
            set => SetProperty(ref _directoryNames, value);
        }

        public ICommand ImportCommand { get; }
        public ICommand ImportImageFromLocalCommand { get; }
        public ICommand ImportImageFromUrlCommand { get; }
        public ICommand CreateDirectoryCommand { get; }

        public ImageGalleryViewModel(IImageService imageService, IDirectoryService directoryService, INotificationService notificationService)
        {
            _notificationService = notificationService;
            _imageService = imageService;
            _directoryService = directoryService;

            Directories = new ObservableCollection<DirectoryComponentViewModel>();
            DirectoryNames = new ObservableCollection<string>();

            ImportMethods = new ObservableCollection<string>
            {
                ImportMethodEnum.LocalStorage.ToString(),
                ImportMethodEnum.UrlWeb.ToString()
            };

            SelectedImportMethod = ImportMethods[0];

            ImportCommand = new Command(async () => await OnImportMethodChangedAsync());
            ImportImageFromLocalCommand = new Command(async () => await OnImportImageFromLocalAsync());
            ImportImageFromUrlCommand = new Command<string>(async (url) => await OnImportImageFromUrlAsync(url));
            CreateDirectoryCommand = new Command(async () => await OnCreateDirectoryAsync());

            LoadDirectories();
        }

        private async void LoadDirectories()
        {
            Directories.Clear();

            var directories = await _directoryService.GetAllDirectoriesAsync();
            if (directories == null || directories.Count == 0)
            {
                return;
            }

            foreach (var directory in directories)
            {
                var directoryViewModel = new DirectoryComponentViewModel(directory, _imageService, _notificationService);
                Directories.Add(directoryViewModel);
            }

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                DirectoryNames.Clear();
                foreach (var directory in directories)
                {
                    if (!DirectoryNames.Any())
                    {
                        DirectoryNames.Add(" ");
                    }
                    else
                    {
                        if (!string.Equals(directory.Name, Constantes.DEFAULT_FROM_LOCALSTORAGE_DIRECTORY, StringComparison.OrdinalIgnoreCase)
                            && !string.Equals(directory.Name, Constantes.DEFAULT_FROM_WEBURL_DIRECTORY, StringComparison.OrdinalIgnoreCase))
                        {
                            DirectoryNames.Add(directory.Name);
                        }
                    }
                }
            });
        }

        private async Task OnImportMethodChangedAsync()
        {
            try
            {
                if (SelectedImportMethod == ImportMethodEnum.LocalStorage.ToString())
                {
                    await OnImportImageFromLocalAsync();
                }
                else if (SelectedImportMethod == ImportMethodEnum.UrlWeb.ToString())
                {
                    await Shell.Current.DisplayPromptAsync("Importer une image", "Collez l'URL de l'image :", "OK", "Annuler", "https://")
                        .ContinueWith(async urlTask =>
                        {
                            var url = await urlTask;
                            if (!string.IsNullOrWhiteSpace(url))
                            {
                                await OnImportImageFromUrlAsync(url);
                            }
                        });
                }

                await _notificationService.ShowSuccessAsync("L'image a été importée avec succès.");
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Erreur lors de l'importation de l'image." + ex.Message);
            }
        }

        private async Task OnImportImageFromLocalAsync()
        {
            var image = await _imageService.ImportImageFromLocalAsync(SelectedDirectory);

            LoadDirectories();
        }

        private async Task OnImportImageFromUrlAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                return;
            }

            var image = await _imageService.ImportImageFromUrlAsync(url, SelectedDirectory);

            LoadDirectories();
        }

        private async Task OnCreateDirectoryAsync()
        {
            var newDirectoryName = await Shell.Current.DisplayPromptAsync("Créez un répertoire", "Nom : ", "OK", "Annuler");
            if (!string.IsNullOrWhiteSpace(newDirectoryName))
            {
                try
                {
                    await _directoryService.CreateDirectoryAsync(new Models.Directory { Name = newDirectoryName });
                    await _notificationService.ShowSuccessAsync($"Le répertoire \"{newDirectoryName}\" a été créé avec succès.");
                    LoadDirectories();
                }
                catch (Exception ex)
                {
                    await _notificationService.ShowErrorAsync($"Erreur lors de la création du répertoire \"{newDirectoryName}\" : " + ex.Message);
                }
            }
        }

    }
}

using Gestura.Commons;
using Gestura.Interfaces;
using Gestura.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.ViewModels
{
    public class ImportImageViewModel : BaseViewModel, IImportImageViewModel
    {
        private readonly IImageDirectoryService _directoryService;
        private readonly IImageService _imageService;

        public ObservableCollection<string> DirectoryNames { get; } = new ObservableCollection<string>();
        public string SelectedDirectory { get; set; }

        private string _newDirectoryName;
        public string NewDirectoryName
        {
            get => _newDirectoryName;
            set => SetProperty(ref _newDirectoryName, value);
        }

        public ObservableCollection<string> ImportMethods { get; } = new ObservableCollection<string>();

        private string _selectedImportMethod = ImportMethodEnum.None.EnumToString();
        public string SelectedImportMethod
        {
            get => _selectedImportMethod;
            set
            {
                if (SetProperty(ref _selectedImportMethod, value))
                {
                    OnPropertyChanged(nameof(IsUrlMode));
                }
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        public bool IsUrlMode => SelectedImportMethod == ImportMethodEnum.UrlWeb.EnumToString();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    OnPropertyChanged(nameof(IsNotBusy));
                    //ImportCommand.ChangeCanExecute();
                }
            }
        }

        public bool IsNotBusy => !IsBusy;

        public ICommand ImportCommand { get; }
        public ICommand CancelCommand { get; }

        public ImportImageViewModel(IImageDirectoryService imageDirectoryService, IImageService imageService)
        {
            _directoryService = imageDirectoryService;
            _imageService = imageService;

            var methods = Common.GetImportMethodList();
            foreach (var importMethod in methods)
            {
                ImportMethods.Add(importMethod);
            }

            ImportCommand = new Command(async () => await OnImportAsync(), () => IsNotBusy);
            CancelCommand = new Command(async () => await Shell.Current.Navigation.PopModalAsync());

            LoadDirectories();
        }

        private async void LoadDirectories()
        {
            IsBusy = true;

            try
            {
                DirectoryNames.Clear();
                var dirs = await _directoryService.GetAllDirectoriesAsync();
                DirectoryNames.Add(string.Empty); // On ajoute un champ vide, afin que si l'utilisateur souhaite d'abord rechercher un dossier
                // puis décide d'abandonner l'idée pour créer un nouveau dossier il puisse le faire
                foreach (var dir in dirs)
                {
                    DirectoryNames.Add(dir.Name);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Erreur lors du chargement des répertoires : {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnImportAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var directoryName = SelectedDirectory;

                if (!string.IsNullOrWhiteSpace(directoryName) || !string.IsNullOrWhiteSpace(NewDirectoryName))
                {
                    directoryName = SelectedDirectory ?? NewDirectoryName;
                    var directory = await _directoryService.GetDirectoryByNameAsync(directoryName);
                    if (directory == null)
                    {
                        directory = await _directoryService.CreateDirectoryAsync(new ImageDirectory { Name = directoryName });
                    }
                    LoadDirectories();
                    SelectedDirectory = directoryName;
                }

                if (SelectedImportMethod == ImportMethodEnum.LocalStorage.EnumToString())
                {
                    if (string.IsNullOrWhiteSpace(directoryName))
                    {
                        directoryName = ImportMethodEnum.LocalStorage.EnumToString();
                    }

                    await _imageService.ImportImageFromLocalAsync(directoryName);
                }
                else if (SelectedImportMethod == ImportMethodEnum.UrlWeb.EnumToString())
                {
                    if (string.IsNullOrWhiteSpace(ImageUrl))
                    {
                        await Shell.Current.DisplayAlert("Erreur", "Veuillez saisir une URL d'image valide.", "OK");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(directoryName))
                    {
                        directoryName = ImportMethodEnum.UrlWeb.EnumToString();
                    }

                    await _imageService.ImportImageFromUrlAsync(ImageUrl, directoryName);
                }

                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (NotSupportedException ex)
            {
                // Gérer le cas où le format de l'image n'est pas supporté
                await Shell.Current.DisplayAlert("Erreur", $"Format d'image non supporté : {ex.Message}", "OK");
                // Afficher un message à l'utilisateur ou enregistrer l'erreur
            }
            catch (IOException ex)
            {
                // Gérer les erreurs liées au flux, comme un flux corrompu
                await Shell.Current.DisplayAlert("Erreur", $"Erreur de lecture du flux : {ex.Message}", "OK");
                // Afficher un message à l'utilisateur ou enregistrer l'erreur
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Une erreur est survenue lors de l'importation : {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

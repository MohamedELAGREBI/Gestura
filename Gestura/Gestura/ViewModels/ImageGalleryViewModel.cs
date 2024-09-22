﻿using Gestura.Commons;
using Gestura.Interfaces;
using Gestura.Models;
using Gestura.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.ViewModels
{
    public class ImageGalleryViewModel : BaseViewModel
    {
        private readonly IImageService _imageService;
        private readonly INotificationService _notificationService;

        private ObservableCollection<ImageReference> _imageReferences;
        public ObservableCollection<ImageReference> ImageReferences
        {
            get => _imageReferences;
            set => SetProperty(ref _imageReferences, value);
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

        private ObservableCollection<string> _importMethods;
        public ObservableCollection<string> ImportMethods
        {
            get => _importMethods;
            set => SetProperty(ref _importMethods, value);
        }

        private int _columnCount;
        public int ColumnCount
        {
            get => _columnCount;
            set => SetProperty(ref _columnCount, value);
        }

        public ICommand ImportCommand { get; }
        public ICommand ImportImageFromLocalCommand { get; }
        public ICommand ImportImageFromUrlCommand { get; }
        public ICommand DeleteImageCommand { get; }

        public ImageGalleryViewModel(IImageService imageService, INotificationService notificationService)
        {
            _notificationService = notificationService;
            _imageService = imageService;
            ImageReferences = new ObservableCollection<ImageReference>();

            ImportMethods = new ObservableCollection<string>
            {
                ImportMethodEnum.LocalStorage.ToString(),
                ImportMethodEnum.UrlWeb.ToString()
            };

            SelectedImportMethod = ImportMethods[0];

            ImportCommand = new Command(async () => await OnImportMethodChangedAsync());
            ImportImageFromLocalCommand = new Command(async () => await OnImportImageFromLocalAsync());
            ImportImageFromUrlCommand = new Command<string>(async (url) => await OnImportImageFromUrlAsync(url));
            DeleteImageCommand = new Command<ImageReference>(async (image) => await OnDeleteImageAsync(image));

            LoadImages();

            UpdateColumnCount();
            DeviceDisplay.MainDisplayInfoChanged += (s, e) => UpdateColumnCount();
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
            var image = await _imageService.ImportImageFromLocalAsync();
            if (image != null)
            {
                ImageReferences.Add(image);
            }
        }

        private async Task OnImportImageFromUrlAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                return;
            }

            var image = await _imageService.ImportImageFromUrlAsync(url);
            if (image != null)
            {
                ImageReferences.Add(image);
            }
        }

        private async Task OnDeleteImageAsync(ImageReference imageReference)
        {
            try
            {
                var result = await _imageService.DeleteImageAsync(imageReference);
                if (result)
                {
                    var image = ImageReferences.FirstOrDefault(i => i == imageReference);
                    if (image != null)
                    {
                        ImageReferences.Remove(image);
                        await _notificationService.ShowSuccessAsync("Image supprimée avec succès.");
                    }
                }
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Erreur lors de la suppression de l'image : " + ex.Message);
            }
        }

        private void UpdateColumnCount()
        {
            // Obtenir la largeur de l'écran
            var screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;

            // Definir le nombre de colonnes basé sur la largeur de l'écran
            if (screenWidth > 1000)
            {
                ColumnCount = 4;
            }
            else if (screenWidth > 600)
            {
                ColumnCount = 3;
            }
            else
            {
                ColumnCount = 2;
            }
        }

        private async void LoadImages()
        {
            try
            {
                var existingImages = await _imageService.GetAllImagesAsync();
                foreach (var image in existingImages)
                {
                    ImageReferences.Add(image);
                }
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Erreur lors de la récupération des images de référence : " + ex.Message);
            }
        }
    }
}

using Gestura.Interfaces;
using Gestura.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.ViewModels
{
    public class DirectoryComponentViewModel : BaseViewModel
    {
        private readonly IImageService _imageService;
        private readonly INotificationService _notificationService;
        private readonly Models.Directory _directory;

        public string DirectoryName { get; set; }
        public ObservableCollection<ImageReference> Images { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        public ICommand ToggleVisibilityCommand { get; }
        public ICommand DeleteImageCommand { get; }

        public DirectoryComponentViewModel(Models.Directory directory, IImageService imageService, INotificationService notificationService)
        {
            DirectoryName = directory.Name;
            _directory = directory;
            _imageService = imageService;

            Images = new ObservableCollection<ImageReference>();

            IsExpanded = false;

            ToggleVisibilityCommand = new Command(async () => await ToggleVisibilityAsync());
            DeleteImageCommand = new Command<ImageReference>(async (image) => await OnDeleteImageAsync(image));
            _notificationService = notificationService;
        }

        private async Task ToggleVisibilityAsync()
        {
            IsExpanded = !IsExpanded;

            Images.Clear();

            var images = await _imageService.GetImagesByDirectoryIdAsync(_directory.Id);
            foreach (var image in images)
            {
                Images.Add(image);
            }
        }

        private async Task OnDeleteImageAsync(ImageReference imageReference)
        {
            try
            {
                var result = await _imageService.DeleteImageAsync(imageReference);
                if (result)
                {
                    var image = Images.FirstOrDefault(i => i == imageReference);
                    if (image != null)
                    {
                        Images.Remove(image);
                        await _notificationService.ShowSuccessAsync("Image supprimée avec succès.");
                    }
                }
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Erreur lors de la suppression de l'image : " + ex.Message);
            }
        }

    }
}

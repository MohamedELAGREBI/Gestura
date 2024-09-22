using Gestura.Interfaces;
using Gestura.Models;
using Gestura.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.ViewModels
{
    public class ImageSelectionViewModel : BaseViewModel
    {
        private readonly IImageService _imageService;

        public ObservableCollection<ImageReference> AvailableImages { get; set; }
        public ObservableCollection<ImageReference> SelectedAvailableImages { get; set; }

        private ObservableCollection<ImageReference> _selectedSessionImages;
        public ObservableCollection<ImageReference> SelectedSessionImages
        {
            get => _selectedSessionImages;
            set => SetProperty(ref _selectedSessionImages, value);
        }
        
        public ICommand ConfirmSelectionCommand { get; set; }
        public ICommand CancelCommand { get; }
        public ICommand AddSelectedImageCommand { get; }
        public ICommand RemoveImageCommand { get; }

        public event EventHandler<IEnumerable<ImageReference>> ImagesSelected;

        public ImageSelectionViewModel(IImageService imageService, IEnumerable<ImageReference> sessionImages)
        {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));

            AvailableImages = new ObservableCollection<ImageReference>();
            SelectedSessionImages = new ObservableCollection<ImageReference>(sessionImages);
            SelectedAvailableImages = new ObservableCollection<ImageReference>();

            LoadAvailableImages();

            ConfirmSelectionCommand = new Command(OnConfirmSelection);
            CancelCommand = new Command(async () => await OnCancelAsync());
            AddSelectedImageCommand = new Command(OnAddSelectedImage);
            RemoveImageCommand = new Command<ImageReference>(OnRemoveImage);
        }

        private async void LoadAvailableImages()
        {
            var existingImages = await _imageService.GetAllImagesAsync();
            foreach (var image in existingImages)
            {
                AvailableImages.Add(image);
            }
        }

        public void OnConfirmSelection()
        {
            ImagesSelected?.Invoke(this, SelectedSessionImages);
        }

        private async Task OnCancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        private void OnAddSelectedImage()
        {
            foreach (var image in SelectedAvailableImages)
            {
                if (!SelectedSessionImages.Contains(image))
                {
                    SelectedSessionImages.Add(image);
                }
            }
        }

        private void OnRemoveImage(ImageReference image)
        {
            if (SelectedSessionImages.Contains(image))
            {
                SelectedSessionImages.Remove(image);
            }
        }
    }
}

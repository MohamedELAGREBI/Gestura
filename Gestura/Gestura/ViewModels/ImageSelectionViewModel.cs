using Gestura.Interfaces;
using Gestura.Models;
using Gestura.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.ViewModels
{
    public class ImageSelectionViewModel : BaseViewModel, IImageSelectionViewModel
    {
        private readonly IImageDirectoryService _directoryService;
        private readonly IImageService _imageService;

        public ObservableCollection<ImageReference> AvailableImages { get; set; }
        public ObservableCollection<ImageReference> SelectedAvailableImages { get; set; }

        private ObservableCollection<ImageReference> _selectedSessionImages;
        public ObservableCollection<ImageReference> SelectedSessionImages
        {
            get => _selectedSessionImages;
            set => SetProperty(ref _selectedSessionImages, value);
        }

        private ObservableCollection<ImageGroup> _groupedAvailableImages = new ObservableCollection<ImageGroup>();
        public ObservableCollection<ImageGroup> GroupedAvailableImages
        {
            get => _groupedAvailableImages;
            set => SetProperty(ref _groupedAvailableImages, value);
        }

        private int _columns = 3;
        public int Columns
        {
            get => _columns;
            set => SetProperty(ref _columns, value);
        }

        public ICommand ConfirmSelectionCommand { get; set; }
        public ICommand CancelCommand { get; }
        public ICommand AddSelectedImageCommand { get; }
        public ICommand RemoveImageCommand { get; }
        public ICommand ToggleImageSelectionCommand { get; }


        public event EventHandler<IEnumerable<ImageReference>> ImagesSelected;

        public ImageSelectionViewModel(IImageService imageService, IImageDirectoryService imageDirectoryService, IEnumerable<ImageReference> sessionImages)
        {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _directoryService = imageDirectoryService ?? throw new ArgumentNullException(nameof(imageDirectoryService));

            AvailableImages = new ObservableCollection<ImageReference>();
            SelectedSessionImages = new ObservableCollection<ImageReference>(sessionImages);
            SelectedAvailableImages = new ObservableCollection<ImageReference>();

            LoadAvailableImages();

            ConfirmSelectionCommand = new Command(OnConfirmSelection);
            CancelCommand = new Command(async () => await OnCancelAsync());
            AddSelectedImageCommand = new Command(OnAddSelectedImage);
            RemoveImageCommand = new Command<ImageReference>(OnRemoveImage);
            ToggleImageSelectionCommand = new Command<ImageReference>(OnToggleImageSelection);
        }

        private async void LoadAvailableImages()
        {
            var existingImages = await _imageService.GetAllImagesAsync();
            var directories = await _directoryService.GetAllDirectoriesAsync();
            var grouped = existingImages.GroupBy(img => img.DirectoryId).Select(g => new ImageGroup(directories.First(d => d.Id == g.Key).Name, g)).OrderBy(g => g.DirectoryName);

            GroupedAvailableImages.Clear();
            foreach (var grp in grouped)
            {
                GroupedAvailableImages.Add(grp); 
            }
        }

        public void OnConfirmSelection()
        {
            ImagesSelected?.Invoke(this, SelectedSessionImages);
            Shell.Current.Navigation.PopModalAsync();
        }

        private async Task OnCancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        private void OnAddSelectedImage()
        {
            foreach (var group in GroupedAvailableImages)
            {
                foreach (var img in group)
                {
                    if (!SelectedSessionImages.Contains(img))
                    {
                        SelectedSessionImages.Add(img);
                    }
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

        private void OnToggleImageSelection(ImageReference image)
        {
            if (image != null)
            {
                // Si l'image est déjà sélectionnée, la retirer
                if (SelectedSessionImages.Contains(image))
                {
                    SelectedSessionImages.Remove(image);
                }
                else
                {
                    SelectedSessionImages.Add(image);
                }

                OnPropertyChanged(nameof(SelectedSessionImages));
            }
        }
    }

    public class ImageGroup : ObservableCollection<ImageReference>
    {
        public string DirectoryName { get; }

        public ImageGroup(string directoryName, IEnumerable<ImageReference> images) : base (images)
        {
            DirectoryName = directoryName;
        }
    }
}

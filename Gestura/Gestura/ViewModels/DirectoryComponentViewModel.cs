using Gestura.Interfaces;
using Gestura.Models;
using Microsoft.Maui.Controls.Platform;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.ViewModels
{
    public class DirectoryComponentViewModel : BaseViewModel
    {
        private readonly IImageDirectoryService _imageDirectoryService;

        public ImageDirectory Directory { get; }
        public ObservableCollection<ImageReference> VisibleImages { get; } = new ObservableCollection<ImageReference>();

        private bool _isExpanded = false;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (SetProperty(ref _isExpanded, value))
                { 
                    RefreshImages();
                    AnimatedExpand?.Invoke(_isExpanded);
                }
            }
        }

        public ICommand ToggleExpandCommand { get; }
        public ICommand RenameDirectoryCommand { get; }
        public ICommand DeleteDirectoryCommand { get; }

        public string ToggleButtonText => IsExpanded ? "Réduire" : "Voir plus";

        public bool IsVisible { get; set; } = true; // pour filtrage

        public Action<bool>? AnimatedExpand { get; set; }

        public event EventHandler? DirectoryDeleted;

        public DirectoryComponentViewModel(IImageDirectoryService imageDirectoryService, ImageDirectory directory)
        {
            Directory = directory;
            _imageDirectoryService = imageDirectoryService;

            ToggleExpandCommand = new Command(() => IsExpanded = !IsExpanded);
            DeleteDirectoryCommand = new Command(OnDeleteDirectory);
            RenameDirectoryCommand = new Command(OnRenameDirectory);

            RefreshImages();
        }

        private async void OnDeleteDirectory()
        {
            var isDeleted = await _imageDirectoryService.DeleteDirectoryAsync(Directory);
            if (isDeleted)
            {
                DirectoryDeleted?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                //
            }
        }

        private async void OnRenameDirectory()
        {
            //var newName = await PromptForNewNameAsync();
            //if (!string.IsNullOrWhiteSpace(newName))
            //{
            //    var isRenamed = await _imageDirectoryService.UpdateDirectoryByNameAsync(Directory.Id, newName);
            //    if (isRenamed)
            //    {
            //        Directory.Name = newName;
            //        OnPropertyChanged(nameof(Directory));
            //    }
            //    else
            //    {
            //        //
            //    }
            //}
        }

        private void RefreshImages()
        {
            VisibleImages.Clear();
            var images = IsExpanded ? Directory.ImageReferences : Directory.ImageReferences.Take(5);
            foreach (var image in images)
            {
                VisibleImages.Add(image);
            }
            OnPropertyChanged(nameof(ToggleButtonText));
        }
    }
}

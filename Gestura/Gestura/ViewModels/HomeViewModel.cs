using Gestura.Interfaces;
using Gestura.Views;
using System.Windows.Input;

namespace Gestura.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand NavigateToImageGalleryCommand { get; }
        public ICommand NavigateToDrawingSessionCommand { get; }

        public HomeViewModel()
        {

            NavigateToImageGalleryCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(ImageGalleryPage), new Dictionary<string, object>
            {
                { "ViewModel", MauiProgram.Services.GetService<ImageGalleryViewModel>() }
            }));

            NavigateToDrawingSessionCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(DrawingSessionsManagerPage), new Dictionary<string, object>
            {
                { "ViewModel", MauiProgram.Services.GetService<DrawingSessionManagerViewModel>() }
            }));
        }
    }
}

using Gestura.ViewModels;

namespace Gestura.Views;

public partial class ImageGalleryPage : ContentPage
{
    public ImageGalleryPage(ImageGalleryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
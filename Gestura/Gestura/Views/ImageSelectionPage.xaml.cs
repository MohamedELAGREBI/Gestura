using Gestura.Interfaces;
using Gestura.Models;
using Gestura.ViewModels;

namespace Gestura.Views;

public partial class ImageSelectionPage : ContentPage
{
    public ImageSelectionPage(IImageService imageService, IEnumerable<ImageReference> sessionImages)
    {
        InitializeComponent();
        BindingContext = new ImageSelectionViewModel(imageService, sessionImages);
    }

    private void OnImageSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var viewModel = BindingContext as ImageSelectionViewModel;
        viewModel.SelectedAvailableImages.Clear();
        foreach (var image in e.CurrentSelection.Cast<ImageReference>())
        {
            viewModel.SelectedAvailableImages.Add(image);
        }
    }
}
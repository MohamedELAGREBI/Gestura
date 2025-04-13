using Gestura.Interfaces;
using Gestura.Models;
using Gestura.ViewModels;

namespace Gestura.Views;

public partial class ImageSelectionPage : ContentPage
{
    public ImageSelectionPage(IImageService imageService, IImageDirectoryService imageDirectoryService, IEnumerable<ImageReference> sessionImages)
    {
        InitializeComponent();
        BindingContext = new ImageSelectionViewModel(imageService, imageDirectoryService, sessionImages);
        this.SizeChanged += OnPageSizeChanged;
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

    private void OnPageSizeChanged(object sender, EventArgs e)
    {
        if (BindingContext is ImageSelectionViewModel viewModel)
        {
            // Supposons que chaque image a une largeur de 100 et un espacement de 10
            int imageWidth = 200 + 10;
            int columns = Math.Max(1, (int)(this.Width / imageWidth));
            viewModel.Columns = columns;
        }
    }
}
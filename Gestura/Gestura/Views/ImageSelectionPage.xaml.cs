using Gestura.Models;
using Gestura.ViewModels;

namespace Gestura.Views;

public partial class ImageSelectionPage : ContentPage
{
    public ImageSelectionPage(IEnumerable<ImageReference> sessionImages)
    {
        InitializeComponent();
        BindingContext = new ImageSelectionViewModel(sessionImages);
    }

    private void OnConfirmSelectionClicked(object sender, EventArgs e)
    {
        var viewModel = BindingContext as ImageSelectionViewModel;
        viewModel.OnConfirmSelection();
        Shell.Current.Navigation.PopModalAsync();
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
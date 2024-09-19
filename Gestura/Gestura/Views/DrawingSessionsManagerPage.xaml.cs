using Gestura.ViewModels;

namespace Gestura.Views;

public partial class DrawingSessionsManagerPage : ContentPage
{
    public DrawingSessionsManagerPage(DrawingSessionManagerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
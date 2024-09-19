using Gestura.ViewModels;

namespace Gestura.Views;

public partial class CreateSessionPage : ContentPage
{
    public CreateSessionPage(DrawingSessionManagerViewModel parentViewModel)
    {
        InitializeComponent();
        BindingContext = new CreateSessionViewModel(parentViewModel);
    }
}
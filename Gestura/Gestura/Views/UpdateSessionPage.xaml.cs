using Gestura.Models;
using Gestura.ViewModels;

namespace Gestura.Views;

public partial class UpdateSessionPage : ContentPage
{
    public UpdateSessionPage(DrawingSession session, DrawingSessionManagerViewModel parentViewModel)
    {
        InitializeComponent();
        BindingContext = new UpdateSessionViewModel(session, parentViewModel);
    }
}
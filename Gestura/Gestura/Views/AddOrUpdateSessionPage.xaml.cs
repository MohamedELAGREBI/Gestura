using Gestura.Models;
using Gestura.ViewModels;

namespace Gestura.Views;

public partial class AddOrUpdateSessionPage : ContentPage
{
	public AddOrUpdateSessionPage(DrawingSessionManagerViewModel parentViewModel, DrawingSession session)
	{
		InitializeComponent();
		BindingContext = new AddOrUpdateSessionViewModel(parentViewModel, session);
	}
}
using Gestura.Interfaces;
using Gestura.Models;
using Gestura.ViewModels;

namespace Gestura.Views;

public partial class AddOrUpdateSessionPage : ContentPage
{
	public AddOrUpdateSessionPage(IImageService imageService, IDrawingSessionManagerViewModel parentViewModel, DrawingSession session)
	{
		InitializeComponent();
		BindingContext = new AddOrUpdateSessionViewModel(imageService, parentViewModel, session);
	}
}
using Gestura.Interfaces;
using Gestura.ViewModels;

namespace Gestura.Components;

public partial class ImportImagePopup : ContentPage
{
	private TaskCompletionSource<bool> _disappearingTcs = new TaskCompletionSource<bool>();
	public Task DisappearingTask => _disappearingTcs.Task;

	public ImportImagePopup(IImageDirectoryService imageDirectoryService, IImageService imageService)
	{
		InitializeComponent();
		BindingContext = new ImportImageViewModel(imageDirectoryService, imageService);

		Disappearing += (s, e) => _disappearingTcs.TrySetResult(true);
	}
}
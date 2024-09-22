using Gestura.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.Interfaces
{
    public interface IImageSelectionViewModel
    {
        ObservableCollection<ImageReference> AvailableImages { get; }
        ObservableCollection<ImageReference> SelectedAvailableImages { get; }
        ObservableCollection<ImageReference> SelectedSessionImages { get; }

        ICommand ConfirmSelectionCommand { get; set; }
        ICommand CancelCommand { get; }
        ICommand AddSelectedImageCommand { get; }
        ICommand RemoveImageCommand { get; }

        event EventHandler<IEnumerable<ImageReference>> ImagesSelected;

        void OnConfirmSelection();
    }
}

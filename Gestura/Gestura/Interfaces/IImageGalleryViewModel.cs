using Gestura.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.Interfaces
{
    public interface IImageGalleryViewModel
    {
        ObservableCollection<ImageReference> ImageReferences { get; }
        ObservableCollection<string> ImportMethods { get; }

        string SelectedImportMethod { get; set; }
        int ColumnCount { get; set; }

        ICommand ImportCommand { get; }
        ICommand ImportImageFromLocalCommand { get; }
        ICommand ImportImageFromUrlCommand { get; }
        ICommand DeleteImageCommand { get; }
    }
}

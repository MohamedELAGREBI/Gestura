using Gestura.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.Interfaces
{
    public interface IImageGalleryViewModel
    {
        ObservableCollection<string> ImportMethods { get; }

        string SelectedImportMethod { get; set; }

        ICommand ImportCommand { get; }
        ICommand ImportImageFromLocalCommand { get; }
        ICommand ImportImageFromUrlCommand { get; }
    }
}

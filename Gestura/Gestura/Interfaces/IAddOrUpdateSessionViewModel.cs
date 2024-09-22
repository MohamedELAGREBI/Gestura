using Gestura.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.Interfaces
{
    public interface IAddOrUpdateSessionViewModel
    {
        bool IsEditMode { get; set; }
        string Title { get; set; }

        ObservableCollection<ImageReference> SessionImages { get; }
        ObservableCollection<int> HoursOptions { get; }
        ObservableCollection<int> MinutesOptions { get; }
        ObservableCollection<int> SecondsOptions { get; }

        int SelectedHours { get; set; }
        int SelectedMinutes { get; set; }
        int SelectedSeconds { get; set; }

        bool IsLimitless { get; set; }

        ICommand AddImageCommand { get; }
        ICommand SaveSessionCommand { get; }
        ICommand CancelCommand { get; }
    }
}

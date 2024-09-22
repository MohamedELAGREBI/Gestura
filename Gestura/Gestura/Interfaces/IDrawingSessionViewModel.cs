using Gestura.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.Interfaces
{
    public interface IDrawingSessionViewModel
    {
        ObservableCollection<ImageReference> Images { get; }

        ImageReference CurrentImage { get; set; }
        int CurrentImageIndex { get; set; }
        TimeSpan RemainingTime { get; set; }
        string PlayPauseButtonText { get; }
        bool AreControlsVisible { get; set; }
        bool IsFirstImage { get; }
        bool IsLastImage { get; }

        ICommand PreviousPoseCommand { get; }
        ICommand NextPoseCommand { get; }
        ICommand PlayPauseCommand { get; }
        ICommand QuitCommand { get; }
        ICommand EndSessionCommand { get; }
        ICommand ToggleControlsVisibilityCommand { get; }

    }
}

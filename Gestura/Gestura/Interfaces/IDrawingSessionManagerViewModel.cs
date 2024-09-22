using Gestura.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.Interfaces
{
    public interface IDrawingSessionManagerViewModel
    {
        ObservableCollection<DrawingSession> DrawingSessions { get; }
        ObservableCollection<DrawingSession> FilteredDrawingSessions { get; }

        string SearchQuery { get; set; }

        ICommand AddNewSessionCommand { get; }
        ICommand SearchCommand { get; }
        ICommand EditSessionCommand { get; }
        ICommand DeleteSessionCommand { get; }
        ICommand StartOrReplaySessionCommand { get; }

        Task AddSessionAsync(DrawingSession session);
        Task UpdateSessionAsync(DrawingSession session);
    }
}

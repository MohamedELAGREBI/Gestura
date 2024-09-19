using Gestura.Models;
using Gestura.Services;
using Gestura.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.ViewModels
{
    public class DrawingSessionManagerViewModel : BaseViewModel
    {
        private readonly DrawingSessionService _drawingSessionService;
        public ObservableCollection<DrawingSession> DrawingSessions { get; set; }
        public ObservableCollection<DrawingSession> FilteredDrawingSessions { get; set; }

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                SetProperty(ref _searchQuery, value);
                OnSearchSessions();
            }
        }

        public ICommand AddNewSessionCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand EditSessionCommand { get; }
        public ICommand DeleteSessionCommand { get; }
        public ICommand StartOrReplaySessionCommand { get; }

        public DrawingSessionManagerViewModel(DrawingSessionService drawingSessionService)
        {
            _drawingSessionService = drawingSessionService;

            DrawingSessions = new ObservableCollection<DrawingSession>();
            FilteredDrawingSessions = new ObservableCollection<DrawingSession>();

            AddNewSessionCommand = new Command(async () => await OnAddNewSessionAsync());
            SearchCommand = new Command(OnSearchSessions);
            EditSessionCommand = new Command<DrawingSession>(async (session) => await OnEditSessionAsync(session));
            DeleteSessionCommand = new Command<DrawingSession>(async (session) => await OnDeleteSessionAsync(session));
            StartOrReplaySessionCommand = new Command<DrawingSession>(async (session) => await OnStartOrReplaySessionAsync(session));

            LoadSessions();
        }

        private async void LoadSessions()
        {
            var sessions = await _drawingSessionService.GetSessionsAsync();
            foreach (var session in sessions)
            {
                DrawingSessions.Add(session);
            }
            FilterSessions();
        }

        private void FilterSessions()
        {
            FilteredDrawingSessions.Clear();
            foreach (var session in DrawingSessions)
            {
                if (string.IsNullOrWhiteSpace(SearchQuery) || session.Title.ToLower().Contains(SearchQuery.ToLower()))
                {
                    FilteredDrawingSessions.Add(session);
                }
            }
        }

        private void OnSearchSessions()
        {
            FilterSessions();
        }

        private async Task OnAddNewSessionAsync()
        {
            await Shell.Current.Navigation.PushModalAsync(new CreateSessionPage(this));
        }

        public async Task AddSessionAsync(DrawingSession session)
        {
            await _drawingSessionService.AddSessionAsync(session);
            DrawingSessions.Add(session);

            FilterSessions();
        }

        private async Task OnEditSessionAsync(DrawingSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            await Shell.Current.Navigation.PushModalAsync(new UpdateSessionPage(session, this));
        }

        public async Task UpdateSessionAsync(DrawingSession session)
        {
            await _drawingSessionService.UpdateSessionAsync(session);

            var index = DrawingSessions.IndexOf(DrawingSessions.FirstOrDefault(s => s.Id == session.Id));
            if (index >= 0)
            {
                DrawingSessions[index] = session;
            }

            FilterSessions();
        }

        private async Task OnDeleteSessionAsync(DrawingSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            var confirm = await Shell.Current.DisplayAlert("Confirmer", "Voulez-vous vraiment supprimer cette session ?", "Oui", "Non");
            if (!confirm)
            {
                return;
            }

            await _drawingSessionService.DeleteSessionAsync(session);
            DrawingSessions.Remove(session);
            FilterSessions();
        }

        private async Task OnStartOrReplaySessionAsync(DrawingSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (!session.SelectedImages.Any())
            {
                await Shell.Current.DisplayAlert("Erreur", "La session ne contient aucune image.", "OK");
                return;
            }

            if (session.IsCompleted)
            {
                session.IsCompleted = false;
                await _drawingSessionService.UpdateSessionAsync(session);
                await Shell.Current.Navigation.PushAsync(new DrawingSessionPage(session));
            }
            else
            {
                await Shell.Current.Navigation.PushAsync(new DrawingSessionPage(session));
            }
        }
    }
}

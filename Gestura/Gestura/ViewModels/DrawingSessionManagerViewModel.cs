using Gestura.Interfaces;
using Gestura.Models;
using Gestura.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.ViewModels
{
    public class DrawingSessionManagerViewModel : BaseViewModel
    {
        private readonly IDrawingSessionService _drawingSessionService;
        private readonly INotificationService _notificationService;
        private readonly IImageService _imageService;

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

        public DrawingSessionManagerViewModel(IImageService imageService, IDrawingSessionService drawingSessionService, INotificationService notificationService)
        {
            _drawingSessionService = drawingSessionService ?? throw new ArgumentNullException(nameof(drawingSessionService));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));

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
            try
            {
                var sessions = await _drawingSessionService.GetSessionsAsync();
                foreach (var session in sessions)
                {
                    DrawingSessions.Add(session);
                }
                FilterSessions();
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Erreur lors du chargement des sessions : " + ex.Message);
            }
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

        private async void OnSearchSessions()
        {
            try
            {
                FilterSessions();
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Erreur lors de la recherche de sessions : " + ex.Message);
            }
        }

        private async Task OnAddNewSessionAsync()
        {
            try
            {
                await Shell.Current.Navigation.PushModalAsync(new AddOrUpdateSessionPage(_imageService, this, null));
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Erreur lors de l'ajout d'une nouvelle session : " + ex.Message);
            }
        }

        public async Task AddSessionAsync(DrawingSession session)
        {
            try
            {
                await _drawingSessionService.AddOrUpdateSessionAsync(session);
                DrawingSessions.Add(session);

                FilterSessions();

                await _notificationService.ShowSuccessAsync($"La session \"{session.Title}\" a été ajoutée avec succès.");
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Erreur lors de l'ajout de la session : " + ex.Message);
            }
        }

        private async Task OnEditSessionAsync(DrawingSession session)
        {
            try
            {
                if (session == null)
                {
                    throw new ArgumentNullException(nameof(session));
                }

                await Shell.Current.Navigation.PushModalAsync(new AddOrUpdateSessionPage(_imageService, this, session));
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Erreur lors de la modification de la session : " + ex.Message);
            }
        }

        public async Task UpdateSessionAsync(DrawingSession session)
        {
            try
            {
                await _drawingSessionService.AddOrUpdateSessionAsync(session);

                var index = DrawingSessions.IndexOf(DrawingSessions.FirstOrDefault(s => s.Id == session.Id));
                if (index >= 0)
                {
                    DrawingSessions[index] = session;
                }

                FilterSessions();

                await _notificationService.ShowSuccessAsync("Session modifiée avec succès.");
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Erreur lors de la modification de la session : " + ex.Message);
            }
        }

        private async Task OnDeleteSessionAsync(DrawingSession session)
        {
            try
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

                await _notificationService.ShowSuccessAsync("Session supprimée avec succès.");
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Erreur lors de la suppression de la session : " + ex.Message);
            }
        }

        private async Task OnStartOrReplaySessionAsync(DrawingSession session)
        {
            try
            {
                if (session == null)
                {
                    throw new ArgumentNullException(nameof(session));
                }

                if (!session.CanStart())
                {
                    await _notificationService.ShowWarningAsync("La session ne contient aucune image.");
                    return;
                }

                if (session.IsCompleted)
                {
                    session.IsCompleted = false;
                    await _drawingSessionService.AddOrUpdateSessionAsync(session);
                    await Shell.Current.Navigation.PushAsync(new DrawingSessionPage(session));
                }
                else
                {
                    await Shell.Current.Navigation.PushAsync(new DrawingSessionPage(session));
                }
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Erreur lors du démarrage ou du replay de la session : " + ex.Message);
            }
        }
    }
}

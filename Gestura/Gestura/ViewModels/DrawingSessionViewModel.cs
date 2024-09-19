using Gestura.Models;
using Gestura.Services;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;

namespace Gestura.ViewModels
{
    public class DrawingSessionViewModel : BaseViewModel
    {
        private readonly DrawingSession _currentSession;
        private readonly DrawingSessionService _drawingSessionService;
        private int _currentImageIndex;
        private bool _isPlaying;
        private TimeSpan _remainingTime;
        private TimeSpan? _poseDuration;
        private bool _areControlsVisible;
        private System.Timers.Timer _controlsVisibilityTimer;
        private System.Timers.Timer _timer;

        public ObservableCollection<ImageReference> Images { get; set; }
        public ImageReference CurrentImage { get; set; }

        public int CurrentImageIndex
        {
            get => _currentImageIndex;
            set
            {
                if (SetProperty(ref _currentImageIndex, value))
                {
                    CurrentImage = Images[_currentImageIndex];
                    OnPropertyChanged(nameof(CurrentImage));
                    UpdateCommands();
                }
            }
        }

        public TimeSpan RemainingTime
        {
            get => _remainingTime;
            set => SetProperty(ref _remainingTime, value);
        }

        public string PlayPauseButtonText => _isPlaying ? "Pause" : "Reprendre";

        public bool AreControlsVisible
        {
            get => _areControlsVisible;
            set => SetProperty(ref _areControlsVisible, value);
        }

        public bool IsFirstImage => CurrentImageIndex == 0;
        public bool IsLastImage => CurrentImageIndex == Images.Count - 1;

        public ICommand PreviousPoseCommand { get; }
        public ICommand NextPoseCommand { get; }
        public ICommand PlayPauseCommand { get; }
        public ICommand QuitCommand { get; }
        public ICommand EndSessionCommand { get; }
        public ICommand ToggleControlsVisibilityCommand { get; }

        public DrawingSessionViewModel(DrawingSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (!session.SelectedImages.Any())
            {
                Shell.Current.DisplayAlert("Erreur", "La session ne contient aucune image.", "OK").GetAwaiter().GetResult();
                return;
            }

            _currentSession = session;
            Images = new ObservableCollection<ImageReference>(session.SelectedImages);

            PreviousPoseCommand = new Command(OnPreviousPose, () => !IsFirstImage);
            NextPoseCommand = new Command(OnNextPose, () => !IsLastImage);
            PlayPauseCommand = new Command(OnPlayPause, () => !_currentSession.IsLimitless);
            QuitCommand = new Command(async () => await OnQuitAsync());
            EndSessionCommand = new Command(async () => await EndSessionAsync());
            ToggleControlsVisibilityCommand = new Command(OnToggleControlsVisibility);

            AreControlsVisible = true;
            InitializeSession();
        }

        private void InitializeSession()
        {
            _poseDuration = _currentSession.PoseDuration;
            CurrentImageIndex = 0;
            CurrentImage = Images[CurrentImageIndex];
            RemainingTime = _poseDuration ?? TimeSpan.Zero;

            if (!_currentSession.IsLimitless)
            {
                StartTimer();
            }

            StartControlVisibilityTimer();
        }

        private void StartTimer()
        {
            StopTimer();

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();

            _isPlaying = true;
            OnPropertyChanged(nameof(PlayPauseButtonText));
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            RemainingTime -= TimeSpan.FromSeconds(1);

            if (RemainingTime.TotalSeconds <= 0)
            {
                _timer.Stop();
                OnNextPose();
            }
        }

        private void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }

            _isPlaying = false;
            OnPropertyChanged(nameof(PlayPauseButtonText));
        }

        private void OnNextPose()
        {
            if (CurrentImageIndex < Images.Count - 1)
            {
                CurrentImageIndex++;
                RemainingTime = _poseDuration ?? TimeSpan.Zero;

                if (!_currentSession.IsLimitless)
                {
                    StartTimer();
                }
            }
            else
            {
                EndSessionAsync();
            }
        }

        private void OnPreviousPose()
        {
            if (CurrentImageIndex > 0)
            {
                CurrentImageIndex--;
                RemainingTime = _poseDuration ?? TimeSpan.Zero;

                if (!_currentSession.IsLimitless)
                {
                    StartTimer();
                }
            }
        }

        private void OnPlayPause()
        {
            if (_currentSession.IsLimitless)
            {
                return;
            }

            if (_isPlaying)
            {
                StopTimer();
            }
            else
            {
                StartTimer();
            }
        }

        private async Task OnQuitAsync()
        {
            StopTimer();
            StopControlVisibilityTimer();

            bool replay = await Shell.Current.DisplayAlert("Session terminée", "Voulez-vous rejouer la session ?", "Oui", "Non");
            if (replay)
            {
                RestartSession();
            }
            else
            {
                StopTimer();
                StopControlVisibilityTimer();

                await Shell.Current.GoToAsync(nameof(Views.DrawingSessionsManagerPage), new Dictionary<string, object>
                {
                    { "ViewModel", MauiProgram.Services.GetService<DrawingSessionManagerViewModel>() }
                });
            }
        }

        private async Task EndSessionAsync()
        {
            StopTimer();
            StopControlVisibilityTimer();
            _currentSession.IsCompleted = true;
            await _drawingSessionService.UpdateSessionAsync(_currentSession);

            // TODO : Gerer l'opacitéde la dernière image et afficher une alerte 
        }

        private void RestartSession()
        {
            CurrentImageIndex = 0;
            RemainingTime = _poseDuration ?? TimeSpan.Zero;
            AreControlsVisible = true;

            if (!_currentSession.IsLimitless)
            {
                StartTimer();
            }

            StartControlVisibilityTimer();
        }

        private void OnToggleControlsVisibility()
        {
            AreControlsVisible = !AreControlsVisible;

            if (AreControlsVisible)
            {
                StartControlVisibilityTimer();
            }
            else
            {
                StopControlVisibilityTimer();
            }
        }

        private void StartControlVisibilityTimer()
        {
            StopControlVisibilityTimer();

            _controlsVisibilityTimer = new System.Timers.Timer(5000);
            _controlsVisibilityTimer.Elapsed += (sender, e) =>
            {
                AreControlsVisible = false;
                OnPropertyChanged(nameof(AreControlsVisible));
                StopControlVisibilityTimer();
            };

            _controlsVisibilityTimer.Start();
        }

        private void StopControlVisibilityTimer()
        {
            if (_controlsVisibilityTimer != null)
            {
                _controlsVisibilityTimer.Stop();
                _controlsVisibilityTimer.Dispose();
                _controlsVisibilityTimer = null;
            }
        }

        private void UpdateCommands()
        {
            ((Command)PreviousPoseCommand).ChangeCanExecute();
            ((Command)NextPoseCommand).ChangeCanExecute();
            ((Command)PlayPauseCommand).ChangeCanExecute();
        }
    }
}

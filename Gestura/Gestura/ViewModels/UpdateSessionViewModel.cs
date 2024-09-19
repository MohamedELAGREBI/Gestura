using Gestura.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.ViewModels
{
    public class UpdateSessionViewModel : BaseViewModel
    {
        private readonly DrawingSessionManagerViewModel _parentViewModel;
        private readonly DrawingSession _originalSession;

        public string Title { get; set; }

        public ObservableCollection<int> HoursOptions { get; set; }
        public ObservableCollection<int> MinutesOptions { get; set; }
        public ObservableCollection<int> SecondsOptions { get; set; }

        private int _selectedHours;
        public int SelectedHours
        {
            get => _selectedHours;
            set => SetProperty(ref _selectedHours, value);
        }

        private int _selectedMinutes;
        public int SelectedMinutes
        {
            get => _selectedMinutes;
            set => SetProperty(ref _selectedMinutes, value);
        }

        private int _selectedSeconds;
        public int SelectedSeconds
        {
            get => _selectedSeconds;
            set => SetProperty(ref _selectedSeconds, value);
        }

        private bool _isLimitless;
        public bool IsLimitless
        {
            get => _isLimitless;
            set => SetProperty(ref _isLimitless, value);
        }

        public ICommand UpdateSessionCommand { get; }
        public ICommand CancelCommand { get; }

        public UpdateSessionViewModel(DrawingSession session, DrawingSessionManagerViewModel parentViewModel)
        {
            _originalSession = session;
            _parentViewModel = parentViewModel;

            Title = session.Title;
            IsLimitless = session.IsLimitless;

            HoursOptions = new ObservableCollection<int>();
            MinutesOptions = new ObservableCollection<int>();
            SecondsOptions = new ObservableCollection<int>();

            for (int i = 0; i < 24; i++)
            {
                HoursOptions.Add(i); // 0-23 pour les heures
            }
            for (int i = 0; i < 60; i++)
            {
                MinutesOptions.Add(i); // 0-59 pour les minutes
                SecondsOptions.Add(i); // 0-59 pour les minutes
            }

            SelectedHours = session.PoseDuration.Hours;
            SelectedMinutes = session.PoseDuration.Minutes;
            SelectedSeconds = session.PoseDuration.Seconds;

            UpdateSessionCommand = new Command(async () => await OnUpdateSessionAsync());
            CancelCommand = new Command(async () => await OnCancelAsync());
        }

        private async Task OnUpdateSessionAsync()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                await Shell.Current.DisplayAlert("Erreur", "Veuillez remplir tous les champs.", "OK");
                return;
            }

            var newDuration = new TimeSpan(SelectedHours, SelectedMinutes, SelectedSeconds);

            _originalSession.Title = Title;
            _originalSession.PoseDuration = newDuration;
            _originalSession.IsLimitless = IsLimitless;

            _parentViewModel.UpdateSessionAsync(_originalSession);

            if (Shell.Current.Navigation.ModalStack.Count > 0)
            {
                await Shell.Current.Navigation.PopModalAsync();
            }
            else
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        private async Task OnCancelAsync()
        {
            if (Shell.Current.Navigation.ModalStack.Count > 0)
            {
                await Shell.Current.Navigation.PopModalAsync();
            }
            else
            {
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}

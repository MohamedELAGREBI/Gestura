using Gestura.Models;
using Gestura.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gestura.ViewModels
{
    public class AddOrUpdateSessionViewModel : BaseViewModel
    {
        private readonly DrawingSessionManagerViewModel _parentViewModel;

        private DrawingSession _session;

        public bool IsEditMode { get; set; }

        public string Title { get; set; }

        public ObservableCollection<ImageReference> SessionImages { get; set; }

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

        public ICommand AddImageCommand { get; }
        public ICommand SaveSessionCommand { get; }
        public ICommand CancelCommand { get; }

        public AddOrUpdateSessionViewModel(DrawingSessionManagerViewModel parentViewModel, DrawingSession session = null)
        {
            _parentViewModel = parentViewModel;
            IsEditMode = session != null;

            _session = session ?? new DrawingSession();
            SessionImages = session != null && session.SelectedImages != null && session.SelectedImages.Any() ?
                            new ObservableCollection<ImageReference>(session.SelectedImages) :
                            new ObservableCollection<ImageReference>();

            Title = _session.Title;
            IsLimitless = _session.IsLimitless;

            HoursOptions = new ObservableCollection<int>();
            MinutesOptions = new ObservableCollection<int>();
            SecondsOptions = new ObservableCollection<int>();

            for (var i = 0; i < 24; i++)
            {
                HoursOptions.Add(i);
            }
            for (var i = 0; i < 60; i++)
            {
                MinutesOptions.Add(i);
                SecondsOptions.Add(i);
            }

            SelectedHours = _session.PoseDuration.Hours;
            SelectedMinutes = _session.PoseDuration.Minutes;
            SelectedSeconds = _session.PoseDuration.Seconds;

            AddImageCommand = new Command(async () => await OnAddImagesAsync());
            SaveSessionCommand = new Command(async () => await OnSaveSessionAsync());
            CancelCommand = new Command(async () => await OnCancelAsync());
        }

        private async Task OnAddImagesAsync()
        {
            var tcs = new TaskCompletionSource<IEnumerable<ImageReference>>();

            var imageSelectionPage = new ImageSelectionPage(_session.SelectedImages);
            imageSelectionPage.BindingContext = new ImageSelectionViewModel(_session.SelectedImages);

            var viewModel = imageSelectionPage.BindingContext as ImageSelectionViewModel;
            viewModel.ImagesSelected += (sender, selectedImages) =>
            {
                tcs.SetResult(selectedImages);
            };

            await Shell.Current.Navigation.PushModalAsync(imageSelectionPage);

            var selectedImages = await tcs.Task;

            if (selectedImages != null && selectedImages.Any())
            {
                SessionImages.Clear();
                foreach (var image in selectedImages)
                {
                    SessionImages.Add(image);
                }
            }
        }

        private async Task OnSaveSessionAsync()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                await Shell.Current.DisplayAlert("Erreur", "Veuillez remplir tous les champs.", "OK");
                return;
            }

            var newDuration = new TimeSpan(SelectedHours, SelectedMinutes, SelectedSeconds);
            _session.Title = Title;
            _session.PoseDuration = newDuration;
            _session.IsLimitless = IsLimitless;
            _session.LastUpdateAt = DateTime.Now;
            _session.SelectedImages = SessionImages.ToList();

            if (IsEditMode)
            {
                await _parentViewModel.UpdateSessionAsync(_session);
            }
            else
            {
                await _parentViewModel.AddSessionAsync(_session);
            }

            await Shell.Current.GoToAsync("..");
        }

        private async Task OnCancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}

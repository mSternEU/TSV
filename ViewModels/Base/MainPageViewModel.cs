using System.Windows.Input;
using TSV.ViewModels.Base;
using TSV.Services.Navigation;

namespace TSV.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        // Original Demo-Properties (beibehalten)
        private int _count = 0;
        private string _text = "Click me";

        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        // Navigation Properties (neu)
        private bool _canGoBack;
        private bool _canGoForward;
        private string _currentPageTitle = "Hauptmenü";

        public bool CanGoBack
        {
            get => _canGoBack;
            set => SetProperty(ref _canGoBack, value);
        }

        public bool CanGoForward
        {
            get => _canGoForward;
            set => SetProperty(ref _canGoForward, value);
        }

        public string CurrentPageTitle
        {
            get => _currentPageTitle;
            set => SetProperty(ref _currentPageTitle, value);
        }

        // Commands
        public ICommand ClickCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand GoForwardCommand { get; }

        // Navigation Commands (für später)
        public ICommand NavigateToKundenCommand { get; }
        public ICommand NavigateToKurseCommand { get; }
        public ICommand NavigateToTeamCommand { get; }
        public ICommand NavigateToStatistikCommand { get; }

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.NavigationChanged += OnNavigationChanged;

            // Original Demo Command
            ClickCommand = new Command(OnCounterClicked);

            // Navigation Commands
            GoBackCommand = new Command(
                async () => await _navigationService.GoBackAsync(),
                () => CanGoBack);

            GoForwardCommand = new Command(
                async () => await _navigationService.GoForwardAsync(),
                () => CanGoForward);

            // Tanzschul-Navigation Commands (für später)
            NavigateToKundenCommand = new Command(async () =>
                await _navigationService.NavigateToAsync("KundenListe"));
            NavigateToKurseCommand = new Command(async () =>
                await _navigationService.NavigateToAsync("kurse"));
            NavigateToTeamCommand = new Command(async () =>
                await _navigationService.NavigateToAsync("team"));
            NavigateToStatistikCommand = new Command(async () =>
                await _navigationService.NavigateToAsync("statistik"));

            // Initial State
            UpdateNavigationState();
        }

        private void OnCounterClicked()
        {
            Count++;
            Text = Count == 1 ? $"Clicked {Count} time" : $"Clicked {Count} times";
        }

        private void OnNavigationChanged(object sender, Models.Navigation.NavigationItem e)
        {
            CurrentPageTitle = e.Name;
            UpdateNavigationState();
        }

        private void UpdateNavigationState()
        {
            CanGoBack = _navigationService.CanGoBack;
            CanGoForward = _navigationService.CanGoForward;

            // Commands aktualisieren
            ((Command)GoBackCommand).ChangeCanExecute();
            ((Command)GoForwardCommand).ChangeCanExecute();
        }

        // MAUI Lifecycle
        public override Task OnNavigatedToAsync()
        {
            // Hier kannst du später Daten laden
            System.Diagnostics.Debug.WriteLine("MainPage navigated to");
            return base.OnNavigatedToAsync();
        }
    }
}
using System.Collections.ObjectModel;
using System.Windows.Input;
using TSV.Models.Business;
using TSV.Services.Data;
using TSV.Services.Navigation;
using TSV.ViewModels.Base;

namespace TSV.ViewModels.Kunden
{
    public partial class KundenListeViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly INavigationService _navigationService;

        // =====================================================
        // CONSTRUCTOR
        // =====================================================
        public KundenListeViewModel(IDatabaseService databaseService, INavigationService navigationService)
        {
            _databaseService = databaseService;
            _navigationService = navigationService;

            // Collections initialisieren
            AllKunden = new ObservableCollection<Kunde>();
            FilteredKunden = new ObservableCollection<Kunde>();

            // Commands initialisieren
            SearchCommand = new Command(async () => await ExecuteSearchAsync());
            AddKundeCommand = new Command(async () => await ExecuteAddKundeAsync());
            KundeSelectedCommand = new Command<Kunde>(async (kunde) => await ExecuteKundeSelectedAsync(kunde));
            RefreshCommand = new Command(async () => await ExecuteRefreshAsync());

            // Initial laden
            _ = Task.Run(async () => await LoadKundenAsync());
        }

        // =====================================================
        // PROPERTIES
        // =====================================================

        // Collections
        public ObservableCollection<Kunde> AllKunden { get; }
        public ObservableCollection<Kunde> FilteredKunden { get; }

        // Selected Customer
        private Kunde _selectedKunde;
        public Kunde SelectedKunde
        {
            get => _selectedKunde;
            set => SetProperty(ref _selectedKunde, value);
        }

        // Search
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    // Auto-Search bei Texteingabe
                    _ = Task.Run(async () => await ExecuteSearchAsync());
                }
            }
        }

        // Loading State
        private bool _isLoading = true;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsNotLoading => !IsLoading;

        // Status Messages
        private string _statusMessage = "Lade Kunden...";
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public string EmptyStateMessage
        {
            get
            {
                if (IsLoading) return "Lade Kunden...";
                if (!string.IsNullOrWhiteSpace(SearchText)) return $"Keine Kunden für '{SearchText}' gefunden";
                return "Noch keine Kunden vorhanden.\nFügen Sie den ersten Kunden hinzu!";
            }
        }

        // =====================================================
        // COMMANDS
        // =====================================================
        public ICommand SearchCommand { get; }
        public ICommand AddKundeCommand { get; }
        public ICommand KundeSelectedCommand { get; }
        public ICommand RefreshCommand { get; }

        // =====================================================
        // COMMAND IMPLEMENTATIONS
        // =====================================================

        private async Task LoadKundenAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Lade Kunden...";

                // Datenbankverbindung testen
                var canConnect = await _databaseService.TestConnectionAsync();
                if (!canConnect)
                {
                    StatusMessage = "Datenbankverbindung fehlgeschlagen";
                    return;
                }

                // Datenbank initialisieren (falls nötig)
                await _databaseService.InitializeDatabaseAsync();

                // Kunden laden
                var kunden = await _databaseService.GetKundenAsync();

                // UI Thread für ObservableCollection Updates
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    AllKunden.Clear();
                    FilteredKunden.Clear();

                    foreach (var kunde in kunden)
                    {
                        AllKunden.Add(kunde);
                        FilteredKunden.Add(kunde);
                    }
                });

                StatusMessage = $"{kunden.Count} Kunden geladen";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"LoadKundenAsync Error: {ex}");
            }
            finally
            {
                IsLoading = false;
                OnPropertyChanged(nameof(IsNotLoading));
                OnPropertyChanged(nameof(EmptyStateMessage));
            }
        }

        private async Task ExecuteSearchAsync()
        {
            try
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    FilteredKunden.Clear();

                    var searchTerm = SearchText?.ToLower() ?? string.Empty;

                    var filteredResults = string.IsNullOrWhiteSpace(searchTerm)
                        ? AllKunden
                        : AllKunden.Where(k =>
                            k.Vorname.ToLower().Contains(searchTerm) ||
                            k.Nachname.ToLower().Contains(searchTerm) ||
                            k.Mail.ToLower().Contains(searchTerm));
                        //    k.DisplayName.ToLower().Contains(searchTerm)); eventuell später einfügen

                    foreach (var kunde in filteredResults)
                    {
                        FilteredKunden.Add(kunde);
                    }
                });

                OnPropertyChanged(nameof(EmptyStateMessage));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ExecuteSearchAsync Error: {ex}");
            }
        }

        private async Task ExecuteAddKundeAsync()
        {
            try
            {
                // Navigation zu Kunden-Detail Seite (Create Mode)
                var parameters = new Dictionary<string, object>
        {
            { "Mode", "Create" }
        };

                await _navigationService.NavigateToAsync("KundenDetail", parameters);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Navigation Fehler: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"ExecuteAddKundeAsync Error: {ex}");
            }
        }

        private async Task ExecuteKundeSelectedAsync(Kunde kunde)
        {
            if (kunde == null) return;

            try
            {
                SelectedKunde = kunde;

                // Navigation zu Kunden-Detail Seite (Edit Mode)
                var parameters = new Dictionary<string, object>
        {
            { "KundeId", kunde.Id },
            { "Mode", "Edit" }
        };

                await _navigationService.NavigateToAsync("KundenDetail", parameters);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Navigation Fehler: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"ExecuteKundeSelectedAsync Error: {ex}");
            }
        }
        private async Task ExecuteRefreshAsync()
        {
            await LoadKundenAsync();
        }

        // =====================================================
        // LIFECYCLE METHODS (für später)
        // =====================================================

        public async Task OnAppearingAsync()
        {
            // Wird aufgerufen wenn Page erscheint
            await ExecuteRefreshAsync();
        }
        public override async Task OnNavigatedToAsync()
        {
            // Wird aufgerufen wenn zur Page navigiert wird
            await LoadKundenAsync();
            await base.OnNavigatedToAsync();
        }
    }
}
using System.Collections.ObjectModel;
using System.Windows.Input;
using TSV.Models.Business;
using TSV.Services.Data;
using TSV.Services.Navigation;
using TSV.ViewModels.Base;

namespace TSV.ViewModels.Kunden
{
    public class KundenDetailViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly INavigationService _navigationService;

        // =====================================================
        // CONSTRUCTOR
        // =====================================================
        public KundenDetailViewModel(IDatabaseService databaseService, INavigationService navigationService)
        {
            _databaseService = databaseService;
            _navigationService = navigationService;

            // Commands initialisieren
            SaveCommand = new Command(async () => await ExecuteSaveAsync(), () => CanSave);
            CancelCommand = new Command(async () => await ExecuteCancelAsync());
            DeleteCommand = new Command(async () => await ExecuteDeleteAsync(), () => CanDelete);

            // Collections für Dropdowns
            Geschlechter = new ObservableCollection<Geschlecht>();
            Zahlweisen = new ObservableCollection<Zahlweise>();
            KundeRollen = new ObservableCollection<KundeRolle>();

            // Initialisierung
            InitializeKunde();
        }

        // =====================================================
        // PROPERTIES - KUNDE MODEL
        // =====================================================

        private Kunde _kunde;
        public Kunde Kunde
        {
            get => _kunde;
            set
            {
                if (SetProperty(ref _kunde, value))
                {
                    OnPropertyChanged(nameof(Title));
                    OnPropertyChanged(nameof(CanDelete));
                    ((Command)DeleteCommand).ChangeCanExecute();
                }
            }
        }

        // =====================================================
        // UI PROPERTIES
        // =====================================================

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (SetProperty(ref _isEditMode, value))
                {
                    OnPropertyChanged(nameof(IsCreateMode));
                    OnPropertyChanged(nameof(Title));
                    OnPropertyChanged(nameof(CanDelete));
                    ((Command)DeleteCommand).ChangeCanExecute();
                }
            }
        }

        public bool IsCreateMode => !IsEditMode;

        public string Title => IsEditMode ?
            $"Kunde bearbeiten: {Kunde?.VollName}" :
            "Neuen Kunden erstellen";

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private bool _isSaving;
        public bool IsSaving
        {
            get => _isSaving;
            set
            {
                if (SetProperty(ref _isSaving, value))
                {
                    OnPropertyChanged(nameof(CanSave));
                    ((Command)SaveCommand).ChangeCanExecute();
                }
            }
        }

        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        private bool _hasValidationErrors;
        public bool HasValidationErrors
        {
            get => _hasValidationErrors;
            set
            {
                if (SetProperty(ref _hasValidationErrors, value))
                {
                    OnPropertyChanged(nameof(CanSave));
                    ((Command)SaveCommand).ChangeCanExecute();
                }
            }
        }

        // =====================================================
        // DROPDOWN COLLECTIONS
        // =====================================================
        public ObservableCollection<Geschlecht> Geschlechter { get; }
        public ObservableCollection<Zahlweise> Zahlweisen { get; }
        public ObservableCollection<KundeRolle> KundeRollen { get; }

        // =====================================================
        // VALIDATION PROPERTIES
        // =====================================================

        private string _vornameError = string.Empty;
        public string VornameError
        {
            get => _vornameError;
            set => SetProperty(ref _vornameError, value);
        }

        private string _nachnameError = string.Empty;
        public string NachnameError
        {
            get => _nachnameError;
            set => SetProperty(ref _nachnameError, value);
        }

        private string _mailError = string.Empty;
        public string MailError
        {
            get => _mailError;
            set => SetProperty(ref _mailError, value);
        }

        private string _telefonError = string.Empty;
        public string TelefonError
        {
            get => _telefonError;
            set => SetProperty(ref _telefonError, value);
        }

        // =====================================================
        // COMMANDS
        // =====================================================
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteCommand { get; }

        // =====================================================
        // COMMAND CAN EXECUTE
        // =====================================================
        public bool CanSave => !IsSaving && !HasValidationErrors && Kunde != null;
        public bool CanDelete => IsEditMode && Kunde?.Id > 0;

        // =====================================================
        // INITIALIZATION
        // =====================================================

        private void InitializeKunde()
        {
            Kunde = new Kunde
            {
                ErstelltAm = DateTime.Now,
                IstAktiv = true
            };

            // Property Changed Handler für Validation
            Kunde.PropertyChanged += OnKundePropertyChanged;
        }

        private void OnKundePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Validation bei Änderungen
            ValidateProperty(e.PropertyName);
        }

        // =====================================================
        // PUBLIC METHODS (für Navigation)
        // =====================================================

        public async Task InitializeAsync(Dictionary<string, object> parameters)
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Lade...";

                System.Diagnostics.Debug.WriteLine($"🔍 KundenDetailViewModel.InitializeAsync - Parameter count: {parameters?.Count ?? 0}");

                // DEBUG: Alle Parameter ausgeben
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        System.Diagnostics.Debug.WriteLine($"🔍   Parameter: {param.Key} = {param.Value} (Type: {param.Value?.GetType().Name})");
                    }
                }

                // Dropdown-Daten laden
                await LoadDropdownDataAsync();

                // Parameter auswerten
                if (parameters != null)
                {
                    System.Diagnostics.Debug.WriteLine($"🔍 Checking parameters...");

                    if (parameters.TryGetValue("KundeId", out var kundeIdObj))
                    {
                        System.Diagnostics.Debug.WriteLine($"🔍 Found KundeId parameter: {kundeIdObj} (Type: {kundeIdObj?.GetType().Name})");

                        // Verschiedene Typen handhaben
                        int kundeId = 0;
                        if (kundeIdObj is int intId)
                        {
                            kundeId = intId;
                        }
                        else if (kundeIdObj is string strId && int.TryParse(strId, out var parsedId))
                        {
                            kundeId = parsedId;
                        }

                        System.Diagnostics.Debug.WriteLine($"🔍 Parsed KundeId: {kundeId}");

                        if (kundeId > 0)
                        {
                            // Edit Mode
                            System.Diagnostics.Debug.WriteLine($"🔍 Loading kunde with ID: {kundeId}");
                            await LoadKundeAsync(kundeId);
                            IsEditMode = true;
                            System.Diagnostics.Debug.WriteLine($"🔍 Edit mode set. Kunde loaded: {Kunde?.VollName}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"🔍 Invalid KundeId: {kundeId}");
                        }
                    }
                    else if (parameters.TryGetValue("Mode", out var modeObj) &&
                             modeObj?.ToString() == "Create")
                    {
                        // Create Mode
                        System.Diagnostics.Debug.WriteLine($"🔍 Create mode detected");
                        IsEditMode = false;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"🔍 No valid parameters found, defaulting to Create mode");
                        IsEditMode = false;
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"🔍 No parameters provided, defaulting to Create mode");
                    IsEditMode = false;
                }

                StatusMessage = string.Empty;
                System.Diagnostics.Debug.WriteLine($"🔍 InitializeAsync completed. IsEditMode: {IsEditMode}");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Laden: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"❌ InitializeAsync Error: {ex}");
                System.Diagnostics.Debug.WriteLine($"❌ Stack Trace: {ex.StackTrace}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadDropdownDataAsync()
        {
            try
            {
                // Kunde Rollen laden
                var rollen = await _databaseService.GetKundeRollenAsync();
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    KundeRollen.Clear();
                    foreach (var rolle in rollen)
                    {
                        KundeRollen.Add(rolle);
                    }
                });

                // TODO: Geschlechter und Zahlweisen laden (wenn die Services implementiert sind)
                // var geschlechter = await _databaseService.GetGeschlechterAsync();
                // var zahlweisen = await _databaseService.GetZahlweisenAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadDropdownDataAsync Error: {ex}");
            }
        }

        private async Task LoadKundeAsync(int kundeId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"🔍 LoadKundeAsync - Loading kunde with ID: {kundeId}");

                var kunde = await _databaseService.GetKundeByIdAsync(kundeId);

                if (kunde != null)
                {
                    System.Diagnostics.Debug.WriteLine($"🔍 LoadKundeAsync - Kunde found: {kunde.VollName}");
                    System.Diagnostics.Debug.WriteLine($"🔍 LoadKundeAsync - Kunde details: ID={kunde.Id}, Email={kunde.Mail}, Phone={kunde.Telefon}");

                    // Property Changed Handler vorübergehend entfernen
                    if (Kunde != null)
                        Kunde.PropertyChanged -= OnKundePropertyChanged;

                    Kunde = kunde;
                    Kunde.PropertyChanged += OnKundePropertyChanged;

                    System.Diagnostics.Debug.WriteLine($"🔍 LoadKundeAsync - Kunde assigned to property. Current Kunde: {Kunde?.VollName}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"❌ LoadKundeAsync - Kunde not found for ID: {kundeId}");
                    StatusMessage = "Kunde nicht gefunden";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Laden des Kunden: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"❌ LoadKundeAsync Error: {ex}");
                System.Diagnostics.Debug.WriteLine($"❌ Stack Trace: {ex.StackTrace}");
            }
        }

        // =====================================================
        // COMMAND IMPLEMENTATIONS
        // =====================================================

        private async Task ExecuteSaveAsync()
        {
            try
            {
                IsSaving = true;
                StatusMessage = "Speichere...";

                // Validation
                if (!ValidateAllProperties())
                {
                    StatusMessage = "Bitte korrigieren Sie die Eingabefehler";
                    return;
                }

                // Speichern
                if (IsEditMode)
                {
                    await _databaseService.UpdateKundeAsync(Kunde);
                    StatusMessage = "Kunde aktualisiert";
                }
                else
                {
                    await _databaseService.CreateKundeAsync(Kunde);
                    StatusMessage = "Kunde erstellt";
                }

                // Zurück zur Liste
                await Task.Delay(1000); // Kurz Status anzeigen
                await _navigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Speichern fehlgeschlagen: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"ExecuteSaveAsync Error: {ex}");
            }
            finally
            {
                IsSaving = false;
            }
        }

        private async Task ExecuteCancelAsync()
        {
            // TODO: Bei Änderungen nachfragen
            await _navigationService.GoBackAsync();
        }

        private async Task ExecuteDeleteAsync()
        {
            try
            {
                // TODO: Bestätigungsdialog einbauen
                var result = await Application.Current.MainPage.DisplayAlert(
                    "Kunde löschen",
                    $"Möchten Sie {Kunde.VollName} wirklich löschen?",
                    "Löschen",
                    "Abbrechen");

                if (!result) return;

                StatusMessage = "Lösche...";
                var success = await _databaseService.DeleteKundeAsync(Kunde.Id);

                if (success)
                {
                    StatusMessage = "Kunde gelöscht";
                    await Task.Delay(1000);
                    await _navigationService.GoBackAsync();
                }
                else
                {
                    StatusMessage = "Löschen fehlgeschlagen";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Löschen fehlgeschlagen: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"ExecuteDeleteAsync Error: {ex}");
            }
        }

        // =====================================================
        // VALIDATION
        // =====================================================

        private void ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Kunde.Vorname):
                    VornameError = string.IsNullOrWhiteSpace(Kunde.Vorname) ? "Vorname ist erforderlich" : string.Empty;
                    break;
                case nameof(Kunde.Nachname):
                    NachnameError = string.IsNullOrWhiteSpace(Kunde.Nachname) ? "Nachname ist erforderlich" : string.Empty;
                    break;
                case nameof(Kunde.Mail):
                    MailError = ValidateEmail(Kunde.Mail);
                    break;
                case nameof(Kunde.Telefon):
                    TelefonError = string.IsNullOrWhiteSpace(Kunde.Telefon) ? "Telefon ist erforderlich" : string.Empty;
                    break;
            }

            UpdateValidationState();
        }

        private bool ValidateAllProperties()
        {
            ValidateProperty(nameof(Kunde.Vorname));
            ValidateProperty(nameof(Kunde.Nachname));
            ValidateProperty(nameof(Kunde.Mail));
            ValidateProperty(nameof(Kunde.Telefon));

            return !HasValidationErrors;
        }

        private string ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "E-Mail ist erforderlich";

            if (!email.Contains("@") || !email.Contains("."))
                return "Ungültige E-Mail-Adresse";

            return string.Empty;
        }

        private void UpdateValidationState()
        {
            HasValidationErrors = !string.IsNullOrEmpty(VornameError) ||
                                  !string.IsNullOrEmpty(NachnameError) ||
                                  !string.IsNullOrEmpty(MailError) ||
                                  !string.IsNullOrEmpty(TelefonError);
        }
    }
}
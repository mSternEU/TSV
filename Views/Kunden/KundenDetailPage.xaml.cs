using TSV.ViewModels.Kunden;

namespace TSV.Views.Kunden
{
    [QueryProperty(nameof(KundeId), "KundeId")]
    [QueryProperty(nameof(Mode), "Mode")]
    public partial class KundenDetailPage : ContentPage
    {
        private readonly KundenDetailViewModel _viewModel;

        public KundenDetailPage(KundenDetailViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        private Dictionary<string, object> _navigationParameters = new();

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // ViewModel mit Parametern initialisieren
            if (_viewModel != null)
            {
                await _viewModel.InitializeAsync(_navigationParameters);
            }
        }

        // MAUI Query Parameter Handling - wird von Navigation aufgerufen
        public void SetParameters(Dictionary<string, object> parameters)
        {
            _navigationParameters = parameters ?? new Dictionary<string, object>();

            // Falls Page bereits geladen ist, direkt initialisieren
            if (_viewModel != null)
            {
                _ = Task.Run(async () => await _viewModel.InitializeAsync(_navigationParameters));
            }
        }

        // Alternative: MAUI Query Attributes (für typisierte Parameter)
        [QueryProperty(nameof(KundeId), "KundeId")]
        [QueryProperty(nameof(Mode), "Mode")]
        public string KundeId
        {
            set
            {
                if (int.TryParse(value, out var id))
                {
                    _navigationParameters["KundeId"] = id;
                }
            }
        }

        public string Mode
        {
            set
            {
                _navigationParameters["Mode"] = value ?? "Create";
            }
        }
    }
}
using TSV.ViewModels.Kunden;

namespace TSV.Views.Kunden
{
    public partial class KundenDetailPage : ContentPage
    {
        private readonly KundenDetailViewModel _viewModel;

        public KundenDetailPage(KundenDetailViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Parameter aus Shell Navigation extrahieren
            var parameters = ExtractNavigationParameters();

            // ViewModel initialisieren
            if (_viewModel != null)
            {
                await _viewModel.InitializeAsync(parameters);
            }
        }

        private Dictionary<string, object> ExtractNavigationParameters()
        {
            var parameters = new Dictionary<string, object>();

            // Parameter aus der aktuellen Shell Route extrahieren
            var query = Shell.Current.CurrentState?.Location?.Query;
            if (!string.IsNullOrEmpty(query))
            {
                // Einfache Query-Parameter Parsing
                var queryParts = query.TrimStart('?').Split('&');
                foreach (var part in queryParts)
                {
                    var keyValue = part.Split('=');
                    if (keyValue.Length == 2)
                    {
                        var key = Uri.UnescapeDataString(keyValue[0]);
                        var value = Uri.UnescapeDataString(keyValue[1]);

                        if (key == "KundeId" && int.TryParse(value, out var kundeId))
                        {
                            parameters["KundeId"] = kundeId;
                        }
                        else if (key == "Mode")
                        {
                            parameters["Mode"] = value;
                        }
                    }
                }
            }

            return parameters;
        }

        // Fallback: Direkte Parameter-Setzung
        public void SetParameters(Dictionary<string, object> parameters)
        {
            if (_viewModel != null)
            {
                _ = Task.Run(async () => await _viewModel.InitializeAsync(parameters ?? new Dictionary<string, object>()));
            }
        }
    }
}

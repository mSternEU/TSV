using TSV.ViewModels.Kunden;
using TSV.Services.Navigation;

namespace TSV.Views.Kunden
{
    public partial class KundenDetailPage : ContentPage
    {
        private readonly KundenDetailViewModel _viewModel;
        private readonly IParameterService _parameterService;

        public KundenDetailPage(KundenDetailViewModel viewModel, IParameterService parameterService)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _parameterService = parameterService;
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            System.Diagnostics.Debug.WriteLine($"?? KundenDetailPage: OnAppearing");

            // SCHRITT 1: Parameter aus dem ParameterService holen
            var parameters = _parameterService.GetParameters("KundenDetail");

            // SCHRITT 2: ViewModel initialisieren
            if (_viewModel != null)
            {
                await _viewModel.InitializeAsync(parameters);
            }

            // SCHRITT 3: Parameter löschen (aufräumen)
            _parameterService.ClearParameters("KundenDetail");
        }
    }
}
using TSV.ViewModels.Kunden;

namespace TSV.Views.Kunden;

public partial class KundenListePage : ContentPage
{
    private readonly KundenListeViewModel _viewModel;

    public KundenListePage(KundenListeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Daten laden wenn Page erscheint
        await _viewModel.OnAppearingAsync();
    }
}
using TSV.ViewModels.Kunden;

namespace TSV.Views.Kunden;

public partial class KundenListePage : ContentPage
{
    public KundenListePage(KundenListeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
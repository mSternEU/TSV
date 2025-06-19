using TSV.Models.Navigation;

namespace TSV.Services.Navigation
{
    public interface INavigationService
    {
        event EventHandler<NavigationItem> NavigationChanged;

        Task NavigateToAsync(string route, Dictionary<string, object> parameters = null);
        Task NavigateToAsync(NavigationItem navigationItem);
        Task GoBackAsync();
        Task GoForwardAsync();

        bool CanGoBack { get; }
        bool CanGoForward { get; }

        NavigationItem CurrentNavigation { get; }
    }
}
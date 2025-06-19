using TSV.Models.Navigation;
using TSV.Services.Navigation;

namespace TSV.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public event EventHandler<NavigationItem> NavigationChanged;

        private readonly List<NavigationItem> _navigationHistory = new();
        private int _currentIndex = -1;

        public bool CanGoBack => _currentIndex > 0;
        public bool CanGoForward => _currentIndex < _navigationHistory.Count - 1;

        public NavigationItem CurrentNavigation =>
            _currentIndex >= 0 && _currentIndex < _navigationHistory.Count
                ? _navigationHistory[_currentIndex]
                : null;

        public async Task NavigateToAsync(string route, Dictionary<string, object> parameters = null)
        {
            var navigationItem = new NavigationItem(route, route)
            {
                Parameters = parameters ?? new Dictionary<string, object>()
            };
            await NavigateToAsync(navigationItem);
        }

        public async Task NavigateToAsync(NavigationItem navigationItem)
        {
            try
            {
                // MAUI Shell Navigation
                await Shell.Current.GoToAsync(navigationItem.Route, navigationItem.Parameters);

                // History Management (dein WPF-Pattern beibehalten)
                AddToHistory(navigationItem);

                // Event feuern
                NavigationChanged?.Invoke(this, navigationItem);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation failed: {ex.Message}");
                // Hier könntest du später ein User-Notification-System einbauen
            }
        }

        public async Task GoBackAsync()
        {
            if (!CanGoBack) return;

            _currentIndex--;
            var targetItem = _navigationHistory[_currentIndex];

            // MAUI Shell Back Navigation
            await Shell.Current.GoToAsync("..");
            NavigationChanged?.Invoke(this, targetItem);
        }

        public async Task GoForwardAsync()
        {
            if (!CanGoForward) return;

            _currentIndex++;
            var targetItem = _navigationHistory[_currentIndex];

            await Shell.Current.GoToAsync(targetItem.Route, targetItem.Parameters);
            NavigationChanged?.Invoke(this, targetItem);
        }

        private void AddToHistory(NavigationItem item)
        {
            // Remove forward history wenn wir zu neuer Seite navigieren
            if (_currentIndex < _navigationHistory.Count - 1)
            {
                _navigationHistory.RemoveRange(_currentIndex + 1, _navigationHistory.Count - _currentIndex - 1);
            }

            // Doppelte consecutive Einträge vermeiden
            if (_navigationHistory.Count == 0 || !_navigationHistory.Last().Route.Equals(item.Route))
            {
                _navigationHistory.Add(item);
                _currentIndex = _navigationHistory.Count - 1;
            }
        }
    }
}
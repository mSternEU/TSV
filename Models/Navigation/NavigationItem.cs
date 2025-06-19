using TSV.Models.Base;

namespace TSV.Models.Navigation
{
    public class NavigationItem : ModelBase
    {
        private string _name;
        private string _route;
        private string _icon;
        private Dictionary<string, object> _parameters;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Route
        {
            get => _route;
            set => SetProperty(ref _route, value);
        }

        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public Dictionary<string, object> Parameters
        {
            get => _parameters ??= new Dictionary<string, object>();
            set => SetProperty(ref _parameters, value);
        }

        public NavigationItem() { }

        public NavigationItem(string name, string route = null, string icon = null)
        {
            Name = name;
            Route = route ?? name.ToLower();
            Icon = icon;
        }
    }
}
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TSV.ViewModels.Base
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        // Lifecycle Methods für MAUI
        public virtual Task OnNavigatedToAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnNavigatedFromAsync()
        {
            return Task.CompletedTask;
        }

        public virtual void OnNavigatedTo()
        {
            // Synchrone Version falls benötigt
        }

        public virtual void OnNavigatedFrom()
        {
            // Synchrone Version falls benötigt
        }
    }
}
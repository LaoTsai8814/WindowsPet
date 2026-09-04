using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindowsPet.Models.ServiceInterface;
using WindowsPet.Views;

namespace WindowsPet.VM
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly INavigationService _navigationService;
        private object? _currentView;

        public object? CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainWindowVM(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.CurrentViewChanged += view => CurrentView = view;
            _navigationService.NavigateTo<LoginView>();
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WindowsPet.Command;
using WindowsPet.Models.ServiceInterface;
using WindowsPet.Views.Tabs;

namespace WindowsPet.VM
{
    public class HomeVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly ITabNavigationService _tabNavigationService;

        public Dictionary<string, ICommand>? SideButtonCommand { get; }
        public ICommand MinimizeCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        private object? _currentTab;
        public object? CurrentTab
        {
            get => _currentTab;
            set
            {
                _currentTab = value;
                OnPropertyChanged();
            }
        }

        public HomeVM(ITabNavigationService tabNavigationService)
        {
            _tabNavigationService = tabNavigationService;

            MinimizeCommand = new RelayCommands((object? obj) =>
            {
                if (Application.Current.MainWindow != null)
                    Application.Current.MainWindow.WindowState = WindowState.Minimized;
            });

            CloseCommand = new RelayCommands((object? obj) =>
            {
                Application.Current.Shutdown();
            });

            SideButtonCommand = new Dictionary<string, ICommand>
            {
                { "Home", new RelayCommands((object? obj) => _tabNavigationService.NavigateTo<HomeTab>()) },
                { "Friends", new RelayCommands((object? obj) => _tabNavigationService.NavigateTo<FriendTab>()) },
                { "Shopping", new RelayCommands((object? obj) => _tabNavigationService.NavigateTo<BuyPetTab>()) },
                { "Chat", new RelayCommands((object? obj) => { }) },
                { "Medal", new RelayCommands((object? obj) => { }) },
                { "Account", new RelayCommands((object? obj) => { }) },
                { "Setting", new RelayCommands((object? obj) => { }) }
            };

            _tabNavigationService.CurrentTabChanged += tab => CurrentTab = tab;
            _tabNavigationService.NavigateTo<HomeTab>();
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

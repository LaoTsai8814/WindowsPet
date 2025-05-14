using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WindowsPet.Command;
using WindowsPet.Models;
using WindowsPet.Views.Tabs;
using WindowsPet.Views.Ucontrol;

namespace WindowsPet.VM
{
    internal class HomeVM:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Dictionary<string, ICommand>? SideButtonCommand { get; }

        public static Action<object>? ChangeTab;

        public ICommand MinimizeCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        private object? _currentTab;

        public object? CurrentTab
        {
            get { return _currentTab; }
            set 
            {
                _currentTab = value;
                OnPropertyChanged();


            }
        }

       

        public HomeVM()
        {
            MinimizeCommand = new RelayCommands((object obj) =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            });
            CloseCommand = new RelayCommands((object obj) =>
            {
                Application.Current.Shutdown();
            });
            SideButtonCommand = new Dictionary<string, ICommand>
            {
                { "Home", new RelayCommands((object obj)=>{TabManager.Instance.GetTab<HomeTab>();}) },
                { "Friends", new RelayCommands((object obj)=>{TabManager.Instance.GetTab<FriendTab>(); }) },
                { "Shopping", new RelayCommands((object obj)=>{}) },
                { "Chat", new RelayCommands((object obj)=>{}) },
                { "Medal", new RelayCommands((object obj)=>{}) },
                { "Account", new RelayCommands((object obj)=>{}) },
                { "Setting", new RelayCommands((object obj)=>{}) }

            };
            ChangeTab += OnTabChanging;
            TabManager.Instance.GetTab<HomeTab>();
        }
        private void OnTabChanging(object view)
        {
            CurrentTab = view;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
    



}

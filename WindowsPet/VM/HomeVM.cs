using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsPet.Command;
using WindowsPet.Views.Tabs;
using WindowsPet.Views.Ucontrol;

namespace WindowsPet.VM
{
    internal class HomeVM:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Dictionary<string, ICommand>? SideButtonCommand { get; }

        private object _currentTab;

        public object CurrentTab
        {
            get { return _currentTab; }
            set { _currentTab = value; }
        }

       

        public HomeVM()
        {
            SideButtonCommand = new Dictionary<string, ICommand>
            {
                { "Home", new RelayCommands((object obj)=>{}) },
                { "MonitorDashboard", new RelayCommands((object obj)=>{}) },
                { "Shopping", new RelayCommands((object obj)=>{}) },
                { "Chat", new RelayCommands((object obj)=>{}) },
                { "Medal", new RelayCommands((object obj)=>{}) },
                { "Account", new RelayCommands((object obj)=>{}) },
                { "Setting", new RelayCommands((object obj)=>{}) }

            };
            CurrentTab = new HomeTab();
        }


        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
    



}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WindowsPet.Models;
using WindowsPet.Views;

namespace WindowsPet.VM
{
    internal class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public static Action<object?>? _changeViewAction;


        private object? _currentView;

        public object? CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainWindowVM()
        {
            _changeViewAction += ChangeViewAction;
            // Initialize the view to HomeView
            CurrentView = ViewManager.Instance.GetView<LoginView>();
        }
        private void ChangeViewAction(object view)
        {
            CurrentView = view;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

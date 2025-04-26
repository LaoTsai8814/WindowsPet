using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WindowsPet.Command;
using WindowsPet.Models;
using WindowsPet.Views.Ucontrol;

namespace WindowsPet.VM.TabsVM
{
    public class HomeTabVM : INotifyPropertyChanged
    {
        public ObservableCollection<UserPets> OnlinePets { get; set; } = new ObservableCollection<UserPets>();
        public ObservableCollection<UserPets> PopularPets { get; set; } = new();
        
        public ObservableCollection<UserPets> MyFavoritePets { get; set; } = new ObservableCollection<UserPets>();

        public ICommand OnPetClick { get; set; }
        public HomeTabVM() 
        {
            OnPetClick = new RelayCommands(OnPetClicked);

        }

        private void OnPetClicked(object obj)
        {
            

            
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

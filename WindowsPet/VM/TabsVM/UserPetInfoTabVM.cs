using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsPet.Command;
using WindowsPet.Models;
using YourGodDamnPet;

namespace WindowsPet.VM.TabsVM
{
    internal class UserPetInfoTabVM:INotifyPropertyChanged
    {
        public ObservableCollection<string>? GIFList { get; set; } = new ObservableCollection<string>
        {

        };
        private int _currentImageIndex = 0;
        private Uri? Gifuri;
        public Uri? GifUri
        {
            get
            {
                Gifuri = new Uri(GIFList[_currentImageIndex], UriKind.Absolute);

                return Gifuri;

            }
            set
            {
                Gifuri = new Uri(GIFList[_currentImageIndex], UriKind.Absolute);
                OnPropertyChanged();

            }
        }

        public event Action? OnImageChange;
        public event PropertyChangedEventHandler? PropertyChanged;
        private Pet? _pet;

        public Pet? Pet
        {
            get { return _pet; }
            set
            {
                _pet = value;
                _currentImageIndex = 0;
                GIFList = new ObservableCollection<string>(_pet.GifPath);
                OnPropertyChanged();

            }
        }
        public ICommand UseCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand PreviousImageCommand { get; set; }
        public ICommand NextImageCommand { get; set; }

        
        public UserPetInfoTabVM()
        {
            UseCommand = new RelayCommands(OnUse);
            RemoveCommand = new RelayCommands(OnRemove);
            BackCommand = new RelayCommands(OnBack);
            NextImageCommand = new RelayCommands(OnNextImage);
            PreviousImageCommand = new RelayCommands(OnPreviousImage);
        }

        private void OnBack(object? obj)
        {
            //throw new NotImplementedException();
        }

        private void OnUse(object? obj)
        {
            DisplayPetManager.Instance.DisplayPet(GifUri);
            //throw new NotImplementedException();
        }

        private void OnRemove(object? obj)
        {
            DisplayPetManager.Instance.RemoveDisplayPet(GifUri);
            //throw new NotImplementedException();
        }

        private void OnNextImage(object obj)
        {
            if (GIFList.Count == 0) return;

            _currentImageIndex = (_currentImageIndex + 1) % GIFList.Count;
            OnImageChange!.Invoke();
            OnPropertyChanged(nameof(GifUri));
        }

        private void OnPreviousImage(object obj)
        {
            if (GIFList.Count == 0) return;

            _currentImageIndex = (_currentImageIndex - 1 + GIFList.Count) % GIFList.Count;
            OnImageChange!.Invoke();
            OnPropertyChanged(nameof(GifUri));
        }

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

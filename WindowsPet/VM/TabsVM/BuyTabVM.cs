using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsPet.Models;
using WindowsPet.Command;
using System.Collections.ObjectModel;
using System.IO;
using static WindowsPet.Models.FileManager;

namespace WindowsPet.VM.TabsVM
{
    internal class BuyTabVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private int _currentImageIndex = 0;

        public event Action? OnImageChange;

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
        public ObservableCollection<string>? GIFList { get; set; } = new ObservableCollection<string>
        {
            
        };
        public decimal Credit
        {
            get { return CurrentUser.Credit; }
            set 
            {
                CurrentUser.Credit = value;
                OnPropertyChanged();
            
            }
        }
        private Pet? _pet;

        public Pet? Pet
        {
            get { return _pet; }
            set 
            {
                _pet = value;
                _currentImageIndex = 0;

                GIFList =new ObservableCollection<string>(LocalStorageSetting.GetAllFileFromDirectory(_pet.PetToken));
                OnPropertyChanged();
            
            }
        }
        public ICommand? PreviousImageCommand { get; set; }
        public ICommand? NextImageCommand { get; set; }

        public ICommand? BackCommand { get; set; }
        public ICommand? BuyCommand { get; set; }

        public BuyTabVM()
        {
            PreviousImageCommand = new RelayCommands(OnPreviousImage);
            NextImageCommand = new RelayCommands(OnNextImage);
            BackCommand = new RelayCommands(OnBack);
            BuyCommand = new RelayCommands(OnBuy);
        }

        private async  void OnBuy(object obj)
        {
            /*
            try
            {
                int id = (int)obj;
                if (Pet != null)
                {
                    await PurchaseManager.Instance.OnPurchasePet(id);
                }
                else
                {
                    ErrorHandle.ShowError("No Pet Selected");
                    // Show message that no pet is selected
                }
            }
            catch(Exception e)
            {
                ErrorHandle.ShowError(e.Message);
                // Handle the exception
            }
            */

        }

        private void OnBack(object obj)
        {

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

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

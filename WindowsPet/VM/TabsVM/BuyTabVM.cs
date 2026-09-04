using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WindowsPet.Command;
using WindowsPet.Models;
using WindowsPet.Models.ServiceInterface;

namespace WindowsPet.VM.TabsVM
{
    public class BuyTabVM : INotifyPropertyChanged
    {
        private readonly IFileManager _fileManager;
        private readonly IPurchaseManager? _purchaseManager;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action? OnImageChange;

        private int _currentImageIndex = 0;
        private Uri? _gifUri;

        public Uri? GifUri
        {
            get
            {
                if (GIFList != null && GIFList.Count > _currentImageIndex && _currentImageIndex >= 0)
                {
                    _gifUri = new Uri(GIFList[_currentImageIndex], UriKind.Absolute);
                    return _gifUri;
                }
                return null;
            }
            set
            {
                _gifUri = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string>? GIFList { get; set; } = new();

        public decimal Credit
        {
            get => CurrentUser.Credit;
            set
            {
                CurrentUser.Credit = value;
                OnPropertyChanged();
            }
        }

        private Pet? _pet;
        public Pet? Pet
        {
            get => _pet;
            set
            {
                _pet = value;
                _currentImageIndex = 0;

                if (_pet != null)
                {
                    GIFList = new ObservableCollection<string>(_fileManager.GetAllFileFromDirectory(_pet.PetToken));
                }
                else
                {
                    GIFList = new ObservableCollection<string>();
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(GifUri));
            }
        }

        public ICommand? PreviousImageCommand { get; set; }
        public ICommand? NextImageCommand { get; set; }
        public ICommand? BackCommand { get; set; }
        public ICommand? BuyCommand { get; set; }

        public BuyTabVM(IFileManager fileManager, IPurchaseManager? purchaseManager = null)
        {
            _fileManager = fileManager;
            _purchaseManager = purchaseManager;

            PreviousImageCommand = new RelayCommands(OnPreviousImage);
            NextImageCommand = new RelayCommands(OnNextImage);
            BackCommand = new RelayCommands(OnBack);
            BuyCommand = new RelayCommands(OnBuy);
        }

        private void OnBuy(object? obj)
        {
        }

        private void OnBack(object? obj)
        {
        }

        private void OnNextImage(object? obj)
        {
            if (GIFList == null || GIFList.Count == 0) return;

            _currentImageIndex = (_currentImageIndex + 1) % GIFList.Count;
            OnImageChange?.Invoke();
            OnPropertyChanged(nameof(GifUri));
        }

        private void OnPreviousImage(object? obj)
        {
            if (GIFList == null || GIFList.Count == 0) return;

            _currentImageIndex = (_currentImageIndex - 1 + GIFList.Count) % GIFList.Count;
            OnImageChange?.Invoke();
            OnPropertyChanged(nameof(GifUri));
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

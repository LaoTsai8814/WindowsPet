using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WindowsPet.Command;
using WindowsPet.Models;
using WindowsPet.Models.ServiceInterface;
using WindowsPet.Views.Tabs;

namespace WindowsPet.VM.TabsVM
{
    public class UserPetInfoTabVM : INotifyPropertyChanged
    {
        private readonly IDisplayPetManager _displayPetManager;
        private readonly IFileManager _fileManager;
        private readonly ITabNavigationService _tabNavigationService;

        public event Action? OnImageChange;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<string>? GIFList { get; set; } = new();

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

        private Pet? _pet;
        public Pet? Pet
        {
            get => _pet;
            set
            {
                _pet = value;
                _currentImageIndex = 0;
                try
                {
                    if (_pet != null)
                    {
                        GIFList = new ObservableCollection<string>(_fileManager.GetAllGIFFileFromDirectory(_pet.PetToken));
                    }
                    else
                    {
                        GIFList = new ObservableCollection<string>();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(GifUri));
            }
        }

        public ICommand UseCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand PreviousImageCommand { get; set; }
        public ICommand NextImageCommand { get; set; }

        public UserPetInfoTabVM(
            IDisplayPetManager displayPetManager,
            IFileManager fileManager,
            ITabNavigationService tabNavigationService)
        {
            _displayPetManager = displayPetManager;
            _fileManager = fileManager;
            _tabNavigationService = tabNavigationService;

            UseCommand = new RelayCommands(OnUse);
            RemoveCommand = new RelayCommands(OnRemove);
            BackCommand = new RelayCommands(OnBack);
            NextImageCommand = new RelayCommands(OnNextImage);
            PreviousImageCommand = new RelayCommands(OnPreviousImage);
        }

        private void OnBack(object? obj)
        {
            _tabNavigationService.NavigateTo<HomeTab>();
        }

        private void OnUse(object? obj)
        {
            if (GifUri != null)
            {
                _displayPetManager.DisplayPet(GifUri);
            }
        }

        private void OnRemove(object? obj)
        {
            if (GifUri != null)
            {
                _displayPetManager.RemoveDisplayPet(GifUri);
            }
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

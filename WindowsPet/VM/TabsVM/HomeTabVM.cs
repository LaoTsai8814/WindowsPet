using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WindowsPet.Command;
using WindowsPet.Models;
using WindowsPet.Models.Repository;
using WindowsPet.Models.ServiceInterface;
using WindowsPet.Views.Tabs;

namespace WindowsPet.VM.TabsVM
{
    public class HomeTabVM : INotifyPropertyChanged
    {
        private readonly IPetService _petService;
        private readonly IPetRepository _petRepository;
        private readonly ITabNavigationService _tabNavigationService;
        private readonly UserPetInfoTabVM _userPetInfoTabVM;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<UIPets> OnlinePets { get; set; } = new();
        public ObservableCollection<Pet> PopularPets { get; set; } = new();
        public ObservableCollection<UIPets> MyFavoritePets { get; set; } = new();

        public ICommand OnUserPetClick { get; set; }
        public ICommand OnPopularPetClick { get; set; }

        public HomeTabVM(
            IPetService petService,
            IPetRepository petRepository,
            ITabNavigationService tabNavigationService,
            UserPetInfoTabVM userPetInfoTabVM)
        {
            _petService = petService;
            _petRepository = petRepository;
            _tabNavigationService = tabNavigationService;
            _userPetInfoTabVM = userPetInfoTabVM;

            OnUserPetClick = new RelayCommands(OnUserPetClicked);
            OnPopularPetClick = new RelayCommands(OnPopularPetClicked);

            Application.Current?.Dispatcher?.Invoke(() =>
            {
                TimeElapsed();
            });
        }

        private void TimeElapsed()
        {
            try
            {
                var pets = _petService.GetPetsByCategory(new PetCategories("Popular"));
                if (pets != null)
                {
                    PopularPets = new ObservableCollection<Pet>(pets);
                    OnPropertyChanged(nameof(PopularPets));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HomeTabVM TimeElapsed error: {ex.Message}");
            }
        }

        private void OnPopularPetClicked(object? obj)
        {
            if (obj != null && Guid.TryParse(obj.ToString(), out var petId))
            {
                var popularpet = _petRepository.GetById(petId);
                if (popularpet != null)
                {
                    _userPetInfoTabVM.Pet = popularpet;
                    _tabNavigationService.NavigateTo<UserPetInfo>();
                }
                else
                {
                    ErrorHandle.ShowError("This Pet DOES NOT EXIST");
                }
            }
            else
            {
                ErrorHandle.ShowError("Invalid Pet ID");
            }
        }

        private void OnUserPetClicked(object? obj)
        {
            // Placeholder for user pet click logic
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;
using WindowsPet.Command;
using WindowsPet.Models;
using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using WindowsPet.Models.Service;
using WindowsPet.Models.ServiceInterface;
using System.IO;
using static WindowsPet.Models.FileManager;
using System.Collections.Generic;
using System.Windows;
using WindowsPet.Models.Repository;
using WindowsPet.Views.Tabs;
using WindowsPet.Views.Ucontrol;

namespace WindowsPet.VM.TabsVM
{
    public class HomeTabVM : INotifyPropertyChanged
    {
        public ObservableCollection<UIPets> OnlinePets { get; set; } = new ObservableCollection<UIPets>();
        public ObservableCollection<Pet> PopularPets { get; set; } = new();

        public ObservableCollection<UIPets> MyFavoritePets { get; set; } = new ObservableCollection<UIPets>();

        public ICommand OnUserPetClick { get; set; }
        public ICommand OnPopularPetClick { get; set; }

        public HomeTabVM()
        {
            OnUserPetClick = new RelayCommands(OnUserPetClicked);
            OnPopularPetClick = new RelayCommands(OnPopularPetClicked);


            Application.Current.Dispatcher.Invoke(() =>
            {
                TimeElapsed();
            });



        }

        private void TimeElapsed()
        {
            using (var scope = App.ServiceProvider.CreateScope())
            {
                var filemanager = scope.ServiceProvider.GetRequiredService<IPetService>();

                // 更新熱門寵物列表
                PopularPets = new ObservableCollection<Pet>(filemanager.GetPetsByCategory(new PetCategories("Popular")));
            }
            //throw new NotImplementedException();
        }

        private void OnPopularPetClicked(object obj)
        {


            if (Guid.Parse(obj.ToString()) != null)
            {
                string? petName = obj as string;
                using (var scope = App.ServiceProvider.CreateScope())
                {
                    var handle = scope.ServiceProvider.GetRequiredService<IPetRepository>();
                    if (handle == null)
                    {
                        ErrorHandle.ShowError("IPetService is null");
                        return;
                    }
                    Pet? popularpet = handle.GetById(Guid.Parse(obj.ToString()));

                    ViewModelManager.Instance.GetViewModel<UserPetInfoTabVM>(TabManager.Instance.GetTabObject<UserPetInfo>()).Pet = popularpet;
                    TabManager.Instance.GetTab<UserPetInfo>();

                }
            }
            else
            {
                ErrorHandle.ShowError("This Pet DOES NOT EXIST");
            }
            //throw new NotImplementedException();
        }

        private void OnUserPetClicked(object obj)
        {
            /*
            if(obj as string != null)
            {

                string? petName = obj as string;
                Pet? pet = AppDbContext.Instance.GetPet(petName);
                if (pet != null)
                {
                    ViewModelManager.Instance.GetViewModel<UserPetInfoTabVM>(TabManager.Instance.GetTabObject<UserPetInfo>()).Pet = pet;
                    TabManager.Instance.GetTab<UserPetInfo>();
                }
                // You can use the petName to find the corresponding pet in your collection
                // Handle the pet click event here
                // For example, navigate to a pet details page or show a popup
            }
            else
            {
                ErrorHandle.ShowError("This Pet DOES NOT EXIST");
            }
            */

        }

        

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

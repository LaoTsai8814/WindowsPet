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
using WindowsPet.Views.Tabs;
using WindowsPet.Views.Ucontrol;

namespace WindowsPet.VM.TabsVM
{
    public class HomeTabVM : INotifyPropertyChanged
    {
        public ObservableCollection<UIPets> OnlinePets { get; set; } = new ObservableCollection<UIPets>();
        public ObservableCollection<UIPets> PopularPets { get; set; } = new();
        
        public ObservableCollection<UIPets> MyFavoritePets { get; set; } = new ObservableCollection<UIPets>();

        public ICommand OnUserPetClick { get; set; }
        public ICommand OnPopularPetClick { get; set; }

        public HomeTabVM() 
        {
            OnUserPetClick = new RelayCommands(OnUserPetClicked);
            OnPopularPetClick = new RelayCommands(OnPopularPetClicked);

        }

        private void OnPopularPetClicked(object obj)
        {
            /*
            if (obj as string != null)
            {
                string? petName = obj as string;
                Pet? usrpet = AppDbContext.Instance.GetPet(petName);

                if (AppDbContext.Instance.IsPetOwnByUser(petName))
                {
                    ViewModelManager.Instance.GetViewModel<UserPetInfoTabVM>(TabManager.Instance.GetTabObject<UserPetInfo>()).Pet = usrpet;
                    TabManager.Instance.GetTab<UserPetInfo>();
                }               
                Pet? pet = AppDbContext.Instance.GetPopularPet(petName);
                if (!AppDbContext.Instance.IsPetPurchased(CurrentUser.Token, petName))
                {
                    var vm = ViewModelManager.Instance.GetViewModel<BuyTabVM>(TabManager.Instance.GetTabObject<BuyPetTab>());
                    vm.Pet = pet;
                    vm.Credit = CurrentUser.Credit;
                    TabManager.Instance.GetTab<BuyPetTab>();
                }
                // You can use the petName to find the corresponding pet in your collection
                // Handle the pet click event here
                // For example, navigate to a pet details page or show a popup
            }
            else
            {
                ErrorHandle.ShowError("This Pet DOES NOT EXIST");
            }*/
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

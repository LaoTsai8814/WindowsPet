using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsPet.Models;

namespace WindowsPet.Views.Ucontrol
{
    /// <summary>
    /// DashBoardCardUserPet.xaml 的互動邏輯
    /// </summary>
    public partial class DashBoardCardUserPet : UserControl
    {
        public DashBoardCardUserPet()
        {
            InitializeComponent();
        }


        public ObservableCollection<UserPets> OnlinePets
        {
            get { return (ObservableCollection<UserPets>)GetValue(OnlinePetsProperty); }
            set { SetValue(OnlinePetsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserPets.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnlinePetsProperty =
            DependencyProperty.Register("OnlinePets", typeof(ObservableCollection<UserPets>), typeof(DashBoardCardUserPet));


        public string DashBoardName
        {
            get { return (string)GetValue(DashBoardNameProperty); }
            set { SetValue(DashBoardNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DashBoardName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DashBoardNameProperty =
            DependencyProperty.Register("DashBoardName", typeof(string), typeof(DashBoardCardUserPet), new PropertyMetadata(""));

        public ICommand OnPetClick
        {
            get { return (ICommand)GetValue(OnPetClickProperty); }
            set { SetValue(OnPetClickProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnPetClick.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnPetClickProperty =
            DependencyProperty.Register("OnPetClick", typeof(ICommand), typeof(DashBoardCardUserPet));

    }
}

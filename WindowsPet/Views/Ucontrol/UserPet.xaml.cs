using System;
using System.Collections.Generic;
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

namespace WindowsPet.Views.Ucontrol
{
    /// <summary>
    /// UserPet.xaml 的互動邏輯
    /// </summary>
    public partial class UserPet : UserControl
    {
        public UserPet()
        {
            InitializeComponent();
        }


        public string PetImage
        {
            get { return (string)GetValue(PetImageProperty); }
            set { SetValue(PetImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PetImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PetImageProperty =
            DependencyProperty.Register("PetImage", typeof(string), typeof(UserPet), new PropertyMetadata(default(string)));



        public string PetName
        {
            get { return (string)GetValue(PetNameProperty); }
            set { SetValue(PetNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PetName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PetNameProperty =
            DependencyProperty.Register("PetName", typeof(string), typeof(UserPet), new PropertyMetadata(""));



       



    }
}

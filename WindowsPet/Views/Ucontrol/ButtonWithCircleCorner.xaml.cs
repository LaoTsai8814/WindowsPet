using MahApps.Metro.IconPacks;
using MaterialDesignThemes.Wpf;
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
    /// ButtonWithCircleCorner.xaml 的互動邏輯
    /// </summary>
    public partial class ButtonWithCircleCorner : UserControl
    {
        public ButtonWithCircleCorner()
        {
            InitializeComponent();
        }



        public PackIconKind ICON
        {
            get { return (PackIconKind)GetValue(ICONProperty); }
            set { SetValue(ICONProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ICONProperty =
            DependencyProperty.Register("ICON", typeof(PackIconKind), typeof(ButtonWithCircleCorner));



        public ICommand LoginBtn
        {
            get { return (ICommand)GetValue(LoginBtnProperty); }
            set { SetValue(LoginBtnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoginBtn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoginBtnProperty =
            DependencyProperty.Register("LoginBtn", typeof(ICommand), typeof(ButtonWithCircleCorner));



    }
}

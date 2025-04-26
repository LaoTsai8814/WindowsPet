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
using WindowsPet.Models;
using WindowsPet.VM.TabsVM;

namespace WindowsPet.Views.Tabs
{
    /// <summary>
    /// HomeTab.xaml 的互動邏輯
    /// </summary>
    public partial class HomeTab : UserControl
    {
        public HomeTab()
        {
            InitializeComponent();
            // Initialize the HomeTab view
            HomeTabVM? vm =  ViewModelManager.Instance.GetViewModel<HomeTabVM>(this);
            DataContext = vm;
        }
    }
}

using System.Windows.Controls;
using WindowsPet.VM.TabsVM;

namespace WindowsPet.Views.Tabs
{
    /// <summary>
    /// Interaction logic for HomeTab.xaml
    /// </summary>
    public partial class HomeTab : UserControl
    {
        public HomeTab(HomeTabVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}

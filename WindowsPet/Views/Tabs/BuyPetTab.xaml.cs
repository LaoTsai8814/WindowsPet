using System.Windows.Controls;
using WindowsPet.VM.TabsVM;

namespace WindowsPet.Views.Tabs
{
    /// <summary>
    /// Interaction logic for BuyPetTab.xaml
    /// </summary>
    public partial class BuyPetTab : UserControl
    {
        public BuyPetTab(BuyTabVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}

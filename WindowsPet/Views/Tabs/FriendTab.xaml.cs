using System.Windows.Controls;
using WindowsPet.VM.TabsVM;

namespace WindowsPet.Views.Tabs
{
    /// <summary>
    /// Interaction logic for FriendTab.xaml
    /// </summary>
    public partial class FriendTab : UserControl
    {
        public FriendTab(FriendTabVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}

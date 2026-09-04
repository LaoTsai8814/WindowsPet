using System.Windows.Controls;
using WindowsPet.VM;

namespace WindowsPet.Views.Tabs
{
    /// <summary>
    /// Interaction logic for RegisterTab.xaml
    /// </summary>
    public partial class RegisterTab : UserControl
    {
        public RegisterTab(RegisterVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}

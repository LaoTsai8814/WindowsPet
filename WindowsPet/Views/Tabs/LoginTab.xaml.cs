using System.Windows.Controls;
using WindowsPet.VM;

namespace WindowsPet.Views.Tabs
{
    /// <summary>
    /// Interaction logic for LoginTab.xaml
    /// </summary>
    public partial class LoginTab : UserControl
    {
        public LoginTab(LoginVM? vm = null)
        {
            InitializeComponent();
            if (vm != null)
            {
                DataContext = vm;
            }
        }
    }
}

using System.Windows.Controls;
using WindowsPet.VM.TabsVM;

namespace WindowsPet.Views.Tabs
{
    /// <summary>
    /// Interaction logic for UserPetInfo.xaml
    /// </summary>
    public partial class UserPetInfo : UserControl
    {
        public UserPetInfo(UserPetInfoTabVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}

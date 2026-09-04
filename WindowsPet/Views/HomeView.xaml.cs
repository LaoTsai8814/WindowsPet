using System.Windows.Controls;
using System.Windows.Input;
using WindowsPet.VM;

namespace WindowsPet.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView(HomeVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void DragBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow._ondragscreen?.Invoke(sender, e);
        }
    }
}

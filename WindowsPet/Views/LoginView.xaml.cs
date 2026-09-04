using System.Windows.Controls;
using System.Windows.Input;
using WindowsPet.VM;

namespace WindowsPet.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView(LoginVM vm)
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

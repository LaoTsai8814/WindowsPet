using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsPet.VM;

namespace WindowsPet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Action<object,MouseButtonEventArgs>? _ondragscreen;
        MainWindowVM vm = new MainWindowVM();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
            _ondragscreen = DragBar_MouseLeftButtonDown;
        }
        private void DragBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove(); // 拖曳視窗
            }
        }
    }
}
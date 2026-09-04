using System;
using System.Windows;
using System.Windows.Input;
using WindowsPet.VM;

namespace WindowsPet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Action<object, MouseButtonEventArgs>? _ondragscreen;

        public MainWindow(MainWindowVM vm)
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
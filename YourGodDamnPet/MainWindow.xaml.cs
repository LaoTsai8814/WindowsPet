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
using System.Windows.Threading;
using WpfAnimatedGif;

namespace YourGodDamnPet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer moveTimer;
        double screenWidth = SystemParameters.PrimaryScreenWidth;
        double screenHeight = SystemParameters.PrimaryScreenHeight;
        Random random = new Random();
        private double dx = 1; // 水平速度
        private double dy = 1; // 垂直速度
        public MainWindow()
        {
            InitializeComponent();
            // 取得螢幕寬度和高度
            double screenWidth = SystemParameters.WorkArea.Width;
            double screenHeight = SystemParameters.WorkArea.Height;

            // 生成隨機的 X 和 Y 座標
            double randomX = random.Next(0, (int)(screenWidth));
            double randomY = random.Next(0, (int)(screenHeight));

            // 設定視窗的啟動位置
            this.Left = randomX;
            this.Top = randomY;

            // 設定 GIF 路徑

            moveTimer = new DispatcherTimer();
            moveTimer.Interval = TimeSpan.FromMilliseconds(10); // 控制速度（越小越快）
            moveTimer.Tick += MoveWindow;
            moveTimer.Start();
        }
        private void MoveWindow(object sender, EventArgs e)
        {

            this.Left += dx;

            this.Top += dy;

            // 邊界偵測，撞牆就反彈
            if (this.Left <= 0 || this.Left + this.Width >= SystemParameters.WorkArea.Width)
                dx = -dx;

            if (this.Top <= 0 || this.Top + this.Height >= SystemParameters.WorkArea.Height)
                dy = -dy;
            // 隨機改變速度
            if (random.NextDouble() < 0.01) // 1% 機率改變速度
            {
                dx = random.Next(-5, 6);
                dy = random.Next(-5, 6);
            }
        }

    }
    
}
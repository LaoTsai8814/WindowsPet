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
        public static string PetName { get; set; }// 預設寵物名稱
        private DispatcherTimer moveTimer;
        private double dx = 1; // 水平速度
        private double dy = 1; // 垂直速度
        public MainWindow()
        {
            InitializeComponent();

            // 設定 GIF 路徑
            var gifUri = new Uri("pack://application:,,,/YourGodDamnPet;component/Asset/dog.gif", UriKind.Absolute);
            var image = new BitmapImage(gifUri);

            // 綁定 GIF 並播放
            ImageBehavior.SetAnimatedSource(PETImage, image);
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
        }
        
    }
    
}
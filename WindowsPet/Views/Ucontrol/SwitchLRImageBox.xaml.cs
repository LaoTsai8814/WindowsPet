using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsPet.Views.Tabs;
using WindowsPet.VM.TabsVM;
using WpfAnimatedGif;

namespace WindowsPet.Views.Ucontrol
{
    /// <summary>
    /// SwitchLRImageBox.xaml 的互動邏輯
    /// </summary>
    public partial class SwitchLRImageBox : UserControl
    {
        public SwitchLRImageBox()
        {
            InitializeComponent();
            this.Loaded += UCLoaded;
        }
        public string CurrentImage
        {
            get { return (string)GetValue(CurrentImageProperty); }
            set { SetValue(CurrentImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentImageProperty =
            DependencyProperty.Register("CurrentImage", typeof(string), typeof(SwitchLRImageBox));
        public ICommand PreviousImageCommand
        {
            get { return (ICommand)GetValue(PreviousImageCommandProperty); }
            set { SetValue(PreviousImageCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviousImageCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreviousImageCommandProperty =
            DependencyProperty.Register("PreviousImageCommand", typeof(ICommand), typeof(SwitchLRImageBox));


        public ICommand NextImageCommand
        {
            get { return (ICommand)GetValue(NextImageCommandProperty); }
            set { SetValue(NextImageCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NextImageCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NextImageCommandProperty =
            DependencyProperty.Register("NextImageCommand", typeof(ICommand), typeof(SwitchLRImageBox));
        private void UCLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is BuyTabVM buyvm)
            {
                buyvm.OnImageChange += () => {

                    var image = new BitmapImage(buyvm.GifUri);
                    ImageBehavior.SetAnimatedSource(AnimatedImage, image);

                };
                var image = new BitmapImage(buyvm.GifUri);
                ImageBehavior.SetAnimatedSource(AnimatedImage, image);
            }
            else if (DataContext is UserPetInfoTabVM usrpetvm)
            {
                usrpetvm.OnImageChange += () => {

                    var image = new BitmapImage(usrpetvm.GifUri);
                    ImageBehavior.SetAnimatedSource(AnimatedImage, image);

                };
                var image = new BitmapImage(usrpetvm.GifUri);
                ImageBehavior.SetAnimatedSource(AnimatedImage, image);
            }
        }
    }
}

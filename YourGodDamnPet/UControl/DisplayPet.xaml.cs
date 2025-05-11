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
using System.Windows.Threading;
using WpfAnimatedGif;

namespace YourGodDamnPet.UControl
{
    /// <summary>
    /// DisplayPet.xaml 的互動邏輯
    /// </summary>
    public partial class DisplayPet : UserControl
    {
        public static Uri _petPath = null;
        public  Uri PetPath
        {
            get {

                return _petPath;
            
            
            }
            set {
                _petPath = value;
                OnchangingPetGIF();



            }
        }
        public DisplayPet()
        {
            InitializeComponent();
            OnchangingPetGIF();


        }
        private void OnchangingPetGIF()
        {
            try
            {
                // 設定 GIF 路徑
                var gifUri = PetPath;
                var image = new BitmapImage(gifUri);

                // 綁定 GIF 並播放
                ImageBehavior.SetAnimatedSource(PETImage, image);
            }
            catch (Exception ex)
            {
                // 處理例外情況
                MessageBox.Show($"無法載入 GIF：{ex.Message}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YourGodDamnPet.UControl;

namespace YourGodDamnPet
{
    public class PetStartup
    {

        public void ShowWindow(Uri gifuri,out MainWindow mainwindow)
        {
            DisplayPet._petPath = gifuri;
            var window = new MainWindow();
            window.Show(); // 或 window.Show();
            mainwindow = window;
        }

    }
}

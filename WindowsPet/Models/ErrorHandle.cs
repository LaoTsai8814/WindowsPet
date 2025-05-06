using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WindowsPet.Models
{
    internal class ErrorHandle
    {
        private static ErrorHandle? _instance;
        public static ErrorHandle Instance => _instance ??= new ErrorHandle();
        

        public ErrorHandle()
        {
            
        }
        public void StartHandler()
        {
            NetworkManager.Instance.OnError += PrintErrorConsole;
        }
        private void PrintErrorConsole(string s)
        {
            Console.WriteLine(s);
        }
        public static void ShowError(string error)
        {
            MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
    
        
}

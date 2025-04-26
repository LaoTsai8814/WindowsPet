using Microsoft.Win32;
using System.Configuration;
using System.Data;
using System.Windows;

namespace WindowsPet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Set IE11 mode for WebView2
            SetIE11Mode();
            AllocConsole();
        }
        public static void SetIE11Mode()
        {
            string exeName = System.IO.Path.GetFileName(Environment.ProcessPath);
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION"))
            {
                key?.SetValue(exeName, 11001, RegistryValueKind.DWord); // 11001 = IE11 mode
            }
        }
    }

}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Configuration;
using System.Data;
using System.Windows;
using WindowsPet.Models;
using WindowsPet.Models.Repository;
using WindowsPet.Models.Repository.VandVM;
using WindowsPet.Models.RepositoryInterface.DatabaseRepositoryInterface;
using WindowsPet.Models.RepositoryInterface.VandVM;
using WindowsPet.Models.Service;
using WindowsPet.Models.ServiceInterface;
using Scrutor;
using WindowsPet.VM;
using WindowsPet.Models.RepositoryInterface.Network;
using WindowsPet.Models.Repository.Networks;
using WindowsPet.Models.RepositoryInterface.Database;
using WindowsPet.Models.Repository.Database;

namespace WindowsPet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]

        public static extern bool AllocConsole();
        public static IServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IFriendRepository, FriendRepository>();
            services.AddScoped<IFriendService, FriendService>();

            services.AddScoped<IPetRepository, PetRepository>();
            services.AddScoped<IPetService, PetService>();

            services.AddScoped<ICategoriesRepository, CategoriesRepository>();

            services.AddScoped<IView, View>();
            services.AddSingleton<IView, Tab>();
            services.AddSingleton<IViewModel, ViewModel>();

            services.AddSingleton<NetworkManager>();
            services.AddSingleton<HandleFromServer>();
            services.AddSingleton<FileManager>();            
            
            services.Scan(scan => scan
    .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
    .AddClasses(c => c.AssignableTo(typeof(INetworkRepository<>)))
    .AsImplementedInterfaces()
    .WithSingletonLifetime());

            ServiceProvider = services.BuildServiceProvider();

            
            
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
        protected override void OnExit(ExitEventArgs e)
        {


            base.OnExit(e);
            // Clean up resources or perform any necessary actions before the application exits
            // For example, you can close any open connections or save user settings
        }
    }

}

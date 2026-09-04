using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Scrutor;
using WindowsPet.Models;
using WindowsPet.Models.Repository;
using WindowsPet.Models.Repository.Database;
using WindowsPet.Models.Repository.Networks;
using WindowsPet.Models.RepositoryInterface.Database;
using WindowsPet.Models.RepositoryInterface.DatabaseRepositoryInterface;
using WindowsPet.Models.RepositoryInterface.Network;
using WindowsPet.Models.Service;
using WindowsPet.Models.ServiceInterface;
using WindowsPet.Views;
using WindowsPet.Views.Tabs;
using WindowsPet.VM;
using WindowsPet.VM.TabsVM;

namespace WindowsPet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            AllocConsole();
            // 攔截所有未處理例外
            AppDomain.CurrentDomain.UnhandledException += (s, args) =>
            {
                Console.WriteLine($"[Fatal Error] {args.ExceptionObject}");
            };
            try
            {
                var services = new ServiceCollection();

                // 1. Database Context
                services.AddDbContext<AppDbContext>();
                Console.WriteLine("ADD DB");
                // 2. Repositories
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IFriendRepository, FriendRepository>();
                services.AddScoped<IPetRepository, PetRepository>();
                services.AddScoped<ICategoriesRepository, CategoriesRepository>();
                Console.WriteLine("ADD Repo");
                // 3. Domain Services
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<IFriendService, FriendService>();
                services.AddScoped<IPetService, PetService>();
                Console.WriteLine("ADD Domain Services");
                // 4. Navigation Services
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<ITabNavigationService, TabNavigationService>();
                Console.WriteLine("ADD Navigation Service");
                // 5. Managers & Networking
                services.AddSingleton<HandleFromServer>();
                services.AddSingleton<INetworkManager, NetworkManager>();
                services.AddSingleton<NetworkManager>(sp => (NetworkManager)sp.GetRequiredService<INetworkManager>());
                Console.WriteLine("ADD Network Service");
                services.AddSingleton<ILoginManager, LoginManager>();
                services.AddSingleton<IDisplayPetManager, DisplayPetManager>();

                services.AddSingleton<IFileManager, FileManager>();
                services.AddSingleton<FileManager>(sp => (FileManager)sp.GetRequiredService<IFileManager>());

                services.AddSingleton<IPurchaseManager, PurchaseManager>();
                // 6. ViewModels
                services.AddSingleton<MainWindowVM>();
                services.AddSingleton<LoginVM>();
                services.AddTransient<RegisterVM>();
                services.AddSingleton<HomeVM>();
                services.AddSingleton<HomeTabVM>();
                services.AddSingleton<FriendTabVM>();
                services.AddSingleton<BuyTabVM>();
                services.AddSingleton<UserPetInfoTabVM>();

                // 7. Views & Windows
                services.AddSingleton<MainWindow>();
                services.AddSingleton<LoginView>();
                services.AddSingleton<HomeView>();
                services.AddSingleton<HomeTab>();
                services.AddSingleton<FriendTab>();
                services.AddSingleton<BuyPetTab>();
                services.AddTransient<RegisterTab>();
                services.AddTransient<LoginTab>();
                services.AddSingleton<UserPetInfo>();
                services.AddTransient<GoogleAuthWindow>();

                // 8. Network Repositories / Handlers via Scrutor assembly scanning
                services.Scan(scan => scan
                    .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(c => c.AssignableTo(typeof(INetworkRepository<>)))
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime());
                Console.WriteLine("Finish Reflection");
                ServiceProvider = services.BuildServiceProvider();
                Console.WriteLine("Dependency injection container initialized.");

                // Initialize Database
                using (var scope = ServiceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.ConnectToDB();
                }
                Console.WriteLine("Database initialized.");
                base.OnStartup(e);

                // Set IE11 mode for WebView2
                SetIE11Mode();


                // Launch Main Window via DI
                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"啟動失敗：{ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static void SetIE11Mode()
        {
            string? exeName = System.IO.Path.GetFileName(Environment.ProcessPath);
            if (exeName != null)
            {
                using (RegistryKey? key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION"))
                {
                    key?.SetValue(exeName, 11001, RegistryValueKind.DWord); // 11001 = IE11 mode
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }

            base.OnExit(e);
        }
    }
}

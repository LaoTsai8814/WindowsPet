using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using WindowsPet.Views;
using WindowsPet.Views.Tabs;
using WindowsPet.VM;
using WindowsPet.VM.TabsVM;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WindowsPet.Models
{
    /// <summary>
    /// Could be known as Server Respond Handler
    /// Class can be known as a manager for handling server responses
    /// </summary>
    internal class HandleFromServer
    {
        private static ConcurrentDictionary<Type, Action<object>> _handlers = new();

        private static HandleFromServer? _instance;
        public static HandleFromServer Instance => _instance ??= new HandleFromServer();
        public void ServerRespondHandler()
        {
            NetworkManager.Instance.OnMessageReceived += OnReceiveServerRespond;
            // Register other handlers here as needed
            Register<ServerRespondStatus>(OnHandleServerRespond);
        }
        /// <summary>
        /// This is the constructor for the HandleFromServer class.
        /// Setup SFTP download Complete Callback
        /// </summary>
        HandleFromServer()
        {
            FileManager.Instance.OnPopluarPetDownloadComplete += OnPopularPetDownloadComplete;
            FileManager.Instance.OnUserPetDownloadComplete += OnUserPetDownloadComplete;
        }
        /// <summary>
        /// This function will be triggered when the user pet download is complete.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="image"></param>
        public void OnUserPetDownloadComplete(string name, string image)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var hometabView = TabManager.Instance?.GetTabObject<HomeTab>();
                if (hometabView == null) return;

                var hometabVM = ViewModelManager.Instance?.GetViewModel<HomeTabVM>(hometabView);
                if (hometabVM == null) return;

                
                hometabVM.MyFavoritePets ??= new ObservableCollection<UserPets>();
                hometabVM.MyFavoritePets.Add(new UserPets(name, image));
            });




        }
        /// <summary>
        /// This function will be triggered when the popular pet download is complete.
        /// </summary>
        public void OnPopularPetDownloadComplete(string name, string image)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var homeView = TabManager.Instance?.GetTabObject<HomeTab>();
                if (homeView == null) return;

                var homeVM = ViewModelManager.Instance?.GetViewModel<HomeTabVM>(homeView);
                if (homeVM == null) return;

                homeVM.PopularPets ??= new ObservableCollection<UserPets>();
                homeVM.PopularPets.Add(new UserPets(name, image));
            });
            





        }
        /// <summary>
        /// This function will be triggered when the server responds to the request.
        /// </summary>
        /// <param name="receive"></param>
        private void OnReceiveServerRespond(string receive)
        {
            // Handle the server response here
            // For example, you can check the status and perform actions accordingly
            object? state = JsonSerialize.DeserializeJson<ServerRespondStatus>(receive);
            //Change It To STA Thread
            // Due to the invoke from the network thread, we need to switch to the UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                Dispatch(state);
            });
            //throw new NotImplementedException();
        }
        /// <summary>
        /// register the callback function for the server response
        ///</summary>
        private void RegisterCallBackFunction(ServerRespondStatus status)
        {
            // Register the callback function for the server response
            try
            {
                Register<RegisterCommand>(OnResoluteName);
                Register<LoginCommand>(OnResoluteName);
                Register<GoogleLoginCommand>(OnResoluteName);
                Register<UserDataRespond>(OnResoluteName);
                if (!status.RequestStatus)
                {
                    return;
                }
                Type? type = Type.GetType($"WindowsPet.Models." + status.RequestName);
                if (type != null)
                {
                    object? obj = JObjectConvert(status.RespondParameter, type);
                    if (obj != null)
                    {
                        // Call the registered callback function with the deserialized object
                        Dispatch(obj);

                        Console.WriteLine($"反序列化成功：{obj.GetType().Name}");
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                // Handle the case when the type is not found
                Console.WriteLine($"Type is Invaild :{ex}");
            }
        }
        /// <summary>
        /// This will convert the JObject to the target type
        /// Jobject is a json object that contain defined in serializable data
        ///</summary>
        public static object? JObjectConvert(object? input, Type targetType)
        {
            return input switch
            {
                JObject jObj => jObj.ToObject(targetType),
                string json => JsonConvert.DeserializeObject(json, targetType),
                _ => null
            };
        }
        /// <summary>
        /// Open the Register Callback Function
        /// </summary>
        private void OnHandleServerRespond(ServerRespondStatus status)
        {
            // Handle the server response here
            RegisterCallBackFunction(status);
        }

        #region Registered Funtion For The Server Response
        private void OnResoluteName(RegisterCommand register)
        {
            ///<summary>
            ///After the server responds to the register command, this function will be called.
            ///and it will change the view to the login view.
            ///</summary>

            LoginVM? vm = ViewModelManager.Instance.GetViewModel<LoginVM>(ViewManager.Instance.GetView<LoginView>());
            if (vm != null)
            {
                vm.ChangeTab?.Invoke();

            }
        }
        private void OnResoluteName(LoginCommand Login)
        {
            // Handle the server response here
            // For example, you can check the status and perform actions accordingly

            LoginManager.Instance.UserLoggedInSuccess(CommandPersonalDataConvertion(Login));

            ViewManager.Instance.GetView<HomeView>();
        }
        private async void OnResoluteName(GoogleLoginCommand googlelogin)
        {
            // Handle the server response here
            // For example, you can check the status and perform actions accordingly
            LoginManager.Instance.UserLoggedInSuccess(CommandPersonalDataConvertion(googlelogin));
            
            await FileManager.Instance.DownloadPoplarPet();
            
            await JsonSerialize.SerializeAndSendJson<UserDataRequest>(new UserDataRequest
            {
                UserToken = googlelogin.UserToken
            });
            ViewManager.Instance.GetView<HomeView>();

            
        }
        private async void OnResoluteName(UserDataRespond respond)
        {
            if (respond.UserPet == null || respond == null)
            {
                return;
            }
            AppDbContext.Instance.AddPetListToUser(respond.UserToken, respond.UserPet);
            await FileManager.Instance.DownloadUserPet(respond.UserPet);
        }
        #endregion
        #region Command To PersonalData Convertion Funtions
        private PersonalData CommandPersonalDataConvertion(GoogleLoginCommand googlelogin)
        {
            return new PersonalData
            {
                Name = googlelogin.Name,
                Token = googlelogin.UserToken,
                Email = googlelogin.Email,
            };
        }
        private PersonalData CommandPersonalDataConvertion(LoginCommand login)
        {
            return new PersonalData
            {
                Name = login.Name,
                Token = login.UserToken,
                Email = login.Email,
                UserPassword = login.Password,
            };
        }
        #endregion 
        #region Register Funtion
        public static void Register<T>(Action<T> action)
        {
            _handlers[typeof(T)] = (obj) => action((T)obj);
        }
        public void Dispatch(object obj)
        {
            var type = obj.GetType();
            if (_handlers.TryGetValue(type, out var handler))
            {
                handler(obj);
            }
            else
            {
                Console.WriteLine($"No handler registered for type {type.Name}");
            }
        }
        #endregion
    }
}

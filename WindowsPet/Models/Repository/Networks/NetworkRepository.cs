using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Ocsp;
using WindowsPet.Models.ConvertTools;
using WindowsPet.Models.RepositoryInterface.Network;
using WindowsPet.Models.Service;
using WindowsPet.Models.ServiceInterface;
using WindowsPet.Views;
using WindowsPet.Views.Tabs;
using WindowsPet.VM;
using WindowsPet.VM.TabsVM;
using static WindowsPet.Models.FileManager;

namespace WindowsPet.Models.Repository.Networks
{
    public class RegisterAccount : INetworkRepository<RegisterCommand>
    {
        
        public void Handler(RegisterCommand Command)
        {

            // Handle the command here
            // For example, you can send the command to a server or process it locally
            
        }

        public void OnFailedStatus()
        {
            throw new NotImplementedException();
        }

        public void OnSuccessStatus()
        {
            LoginVM? vm = ViewModelManager.Instance.GetViewModel<LoginVM>(ViewManager.Instance.GetView<LoginView>());
            if (vm != null)
            {
                vm.ChangeTab?.Invoke();
            }
            //throw new NotImplementedException();
        }
    }
    public class Login : INetworkRepository<LoginCommand>
    {
       
        public async void Handler(LoginCommand Command)
        {
            // Handle the server response here
            // For example, you can check the status and perform actions accordingly
            LoginManager.Instance.UserLoggedInSuccess(Request2PersonalData.CommandPersonalDataConvertion(Command));
            //Set User Token
            CurrentUser.Token = Command.UserToken;
            //Call DI Injection
            var filemanager = App.ServiceProvider.GetRequiredService<FileManager>();
            //Request For Current Popular Pet List etc...
            await JsonSerialize.SerializeAndSendJson<PetListRequestCommand>(new PetListRequestCommand { UserToken =CurrentUser.Token,categories = new(4)});
            
            ViewManager.Instance.GetView<HomeView>();
        }

        
    }
    public class PetListRequest : INetworkRepository<PetListRequestCommand>
    {
        public async void Handler(PetListRequestCommand Command)
        {
            if (Command == null)
            {
                return;
            }
            using (var scope = App.ServiceProvider.CreateScope())
            {
                var handle = scope.ServiceProvider.GetService<IPetService>();
                if (handle == null)
                {
                    Console.WriteLine("IPetService is null");
                    return;
                }
                handle.AddSpecificPetListToTable(Command.values,Command.categories);

            }
            try
            {
                var filehandle = App.ServiceProvider.GetRequiredService<FileManager>();
                var tasks = new List<Task>();

                foreach (var pet in Command.values)
                {
                    var files = await filehandle.GetAllFilesInFolderAsync(pet.PetToken.ToString());
                    
                    if (!Directory.Exists(Path.Combine(LocalStorageSetting.LocalCache, Command.categories.Type,pet.PetToken.ToString())))
                    {
                        Directory.CreateDirectory(Path.Combine(LocalStorageSetting.LocalCache, Command.categories.Type, pet.PetToken.ToString()));
                    }
                    using (var scope = App.ServiceProvider.CreateScope())
                    {
                        var handle = scope.ServiceProvider.GetRequiredService<IPetRepository>();
                        handle.GetById(pet.PetToken).ImagePath = Path.Combine(LocalStorageSetting.LocalCache, Command.categories.Type, pet.PetToken.ToString(), Path.GetFileName(files.FirstOrDefault(u => u.EndsWith(".png"))));
                        handle.Save();
                    }
                    foreach (var f in files)
                    {
                        string fileName = Path.GetFileName(f);
                        tasks.Add(filehandle.DownloadFileAsync(f, Path.Combine(LocalStorageSetting.LocalCache, Command.categories.Type, pet.PetToken.ToString()), Path.GetFileName(f)));
                        
                    }
                }
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PetListRequest: {ex.Message}");
            }
        }
        
    }
    public class UserDataRequest : INetworkRepository<UserDataRequestCommand>
    {
        public async void Handler(UserDataRequestCommand Command)
        {
            
            //throw new NotImplementedException();
        }

        
    }
    public class PetPurchase : INetworkRepository<PetPurchaseCommand>
    {
        public void Handler(PetPurchaseCommand Command)
        {
            if (Command == null)
            {
                return;
            }
            
            //throw new NotImplementedException();
        }

        public void OnFailedStatus()
        {
            throw new NotImplementedException();
        }

        public void OnSuccessStatus()
        {
            throw new NotImplementedException();
        }
    }
    public class ServerRespond : INetworkRepository<ServerRespondStatus>
    {
        public void Handler(ServerRespondStatus Command)
        {
            if(Command == null)
            {
                Console.WriteLine($"Error Occur On {typeof(ServerRespond)}");
                return;
            }
            Dispatch(Command.RespondParameter, Command.RequestStatus);


            //throw new NotImplementedException();
        }
        private void Dispatch(object? obj,bool status)
        {
            if (obj == null) return;

            var type = obj.GetType();
            var handlerType = typeof(INetworkRepository<>).MakeGenericType(type);
            var handler = App.ServiceProvider.GetService(handlerType);

            if (handler != null)
            {
                var method = handlerType.GetMethod("Handler");
                var success = handlerType.GetMethod("OnSuccessStatus");
                var failed = handlerType.GetMethod("OnFailedStatus");
                method?.Invoke(handler, new[] { obj });
                if (status)
                {
                    success?.Invoke(handler, null);
                }
                else
                {
                    failed?.Invoke(handler, null);
                }
            }
            else
            {
                Console.WriteLine($"未找到 {type.Name} 對應的處理者");
            }
        }

        
    }
}

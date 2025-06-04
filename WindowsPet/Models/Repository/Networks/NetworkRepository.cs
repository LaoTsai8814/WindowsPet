using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Ocsp;
using WindowsPet.Models.ConvertTools;
using WindowsPet.Models.RepositoryInterface.Network;
using WindowsPet.Views;
using WindowsPet.VM;

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

            var filemanager = App.ServiceProvider.GetRequiredService<FileManager>();
            await filemanager.DownloadAllUserPets(Command.UserToken.ToString());


            ViewManager.Instance.GetView<HomeView>();
        }

        public void OnFailedStatus()
        {
            throw new NotImplementedException();
        }

        public void OnSuccessStatus()
        {
           
        }
    }
    public class UserDataRequest : INetworkRepository<UserDataRequestCommand>
    {
        public async void Handler(UserDataRequestCommand Command)
        {
            

            
            
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

        public void OnFailedStatus()
        {
            throw new NotImplementedException();
        }

        public void OnSuccessStatus()
        {
            throw new NotImplementedException();
        }
    }
}

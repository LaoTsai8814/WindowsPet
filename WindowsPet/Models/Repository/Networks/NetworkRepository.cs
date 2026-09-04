using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WindowsPet.Models.ConvertTools;
using WindowsPet.Models.RepositoryInterface.Database;
using WindowsPet.Models.RepositoryInterface.Network;
using WindowsPet.Models.ServiceInterface;
using WindowsPet.Views;
using WindowsPet.VM;

namespace WindowsPet.Models.Repository.Networks
{
    public class RegisterAccount : INetworkRepository<RegisterCommand>
    {
        private readonly LoginVM _loginVM;

        public RegisterAccount(LoginVM loginVM)
        {
            _loginVM = loginVM;
        }

        public void Handler(RegisterCommand command)
        {
            // Handle the register command here
        }

        public void OnFailedStatus()
        {
        }

        public void OnSuccessStatus()
        {
            _loginVM.ChangeTab?.Invoke();
        }
    }

    public class Login : INetworkRepository<LoginCommand>
    {
        private readonly ILoginManager _loginManager;
        private readonly IFileManager _fileManager;
        private readonly INavigationService _navigationService;
        private readonly INetworkManager _networkManager;

        public Login(
            ILoginManager loginManager,
            IFileManager fileManager,
            INavigationService navigationService,
            INetworkManager networkManager)
        {
            _loginManager = loginManager;
            _fileManager = fileManager;
            _navigationService = navigationService;
            _networkManager = networkManager;
        }

        public async void Handler(LoginCommand command)
        {
            _loginManager.UserLoggedInSuccess(Request2PersonalData.CommandPersonalDataConvertion(command));
            CurrentUser.Token = command.UserToken;

            // Request for current popular pet list
            await _networkManager.SendJsonAsync(new PetListRequestCommand
            {
                UserToken = CurrentUser.Token,
                categories = new PetCategories(4)
            });

            _navigationService.NavigateTo<HomeView>();
        }
    }

    public class PetListRequest : INetworkRepository<PetListRequestCommand>
    {
        private readonly IPetService _petService;
        private readonly IPetRepository _petRepository;
        private readonly IFileManager _fileManager;

        public PetListRequest(IPetService petService, IPetRepository petRepository, IFileManager fileManager)
        {
            _petService = petService;
            _petRepository = petRepository;
            _fileManager = fileManager;
        }

        public async void Handler(PetListRequestCommand command)
        {
            if (command == null)
            {
                return;
            }

            _petService.AddSpecificPetListToTable(command.values, command.categories);

            try
            {
                var tasks = new List<Task>();

                foreach (var pet in command.values)
                {
                    var files = await _fileManager.GetAllFilesInFolderAsync(pet.PetToken.ToString());
                    var categoryDir = Path.Combine(FileManager.LocalStorageSetting.LocalCache, command.categories.Type, pet.PetToken.ToString());

                    if (!Directory.Exists(categoryDir))
                    {
                        Directory.CreateDirectory(categoryDir);
                    }

                    var petEntity = _petRepository.GetById(pet.PetToken);
                    if (petEntity != null)
                    {
                        var pngFile = files.FirstOrDefault(u => u.EndsWith(".png", StringComparison.OrdinalIgnoreCase));
                        if (pngFile != null)
                        {
                            petEntity.ImagePath = Path.Combine(categoryDir, Path.GetFileName(pngFile));
                            _petRepository.Save();
                        }
                    }

                    foreach (var f in files)
                    {
                        string fileName = Path.GetFileName(f);
                        tasks.Add(_fileManager.DownloadFileAsync(f, categoryDir, fileName));
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
        public void Handler(UserDataRequestCommand command)
        {
        }
    }

    public class PetPurchase : INetworkRepository<PetPurchaseCommand>
    {
        public void Handler(PetPurchaseCommand command)
        {
        }

        public void OnFailedStatus()
        {
        }

        public void OnSuccessStatus()
        {
        }
    }

    public class ServerRespond : INetworkRepository<ServerRespondStatus>
    {
        private readonly IServiceProvider _serviceProvider;

        public ServerRespond(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Handler(ServerRespondStatus command)
        {
            if (command == null)
            {
                Console.WriteLine($"Error Occur On {typeof(ServerRespond)}");
                return;
            }
            Dispatch(command.RespondParameter, command.RequestStatus);
        }

        private void Dispatch(object? obj, bool status)
        {
            if (obj == null) return;

            var type = obj.GetType();
            var handlerType = typeof(INetworkRepository<>).MakeGenericType(type);
            var handler = _serviceProvider.GetService(handlerType);

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

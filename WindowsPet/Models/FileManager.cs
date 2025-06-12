using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using WindowsPet.Models.Repository;
using WindowsPet.Models.ServiceInterface;

namespace WindowsPet.Models
{
    public class FileManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPetService _petService;

        public FileManager(IServiceProvider serviceProvider, IPetService petService)
        {
            _serviceProvider = serviceProvider;
            _petService = petService;
        }
        private readonly HttpClient _http = new();

        
        //Get All File in Pet Directory
        public async Task<List<string>> GetAllFilesInFolderAsync(string petId)
        {
            using var httpClient = new HttpClient();
            string url = $"http://{RestApiSetting.IpAddress}:{RestApiSetting.Port}/api/file/listpet?petid={petId}";
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var files = JArray.Parse(content).ToObject<List<string>>();
                return files ?? new List<string>();
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return new List<string>();
            }
        }


        //Download  File
        public async Task DownloadFileAsync(string url, string savePath, string filename)
        {
            
            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync($"http://{RestApiSetting.IpAddress}:{RestApiSetting.Port}/api/file/download?name={url}");

            response.EnsureSuccessStatusCode();

            byte[] data = await response.Content.ReadAsByteArrayAsync();

            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            if (data == null)
            {
                Console.WriteLine("data 為 null，無法寫入");
                return;
            }

            try
            {
                await File.WriteAllBytesAsync(Path.Combine(savePath, filename), data);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("權限不足: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("其他錯誤: " + ex.Message);
            }

        }
        



        internal static class RestApiSetting
        {
            public readonly static string IpAddress = "192.168.0.104";
            public readonly static int Port = 5225;
        }
        internal static class LocalStorageSetting
        {
            public static string UserOwnPetLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserPet");
            public static string LocalCache = Path.Combine(Path.GetTempPath(), "PetCache");
            public static List<string> GetAllFileFromDirectory(Guid PetId)
            {
                using(var scope = App.ServiceProvider.CreateScope())
                {
                    var petService = scope.ServiceProvider.GetRequiredService<IPetRepository>();
                    return Directory.GetFiles(Path.Combine(LocalStorageSetting.LocalCache, petService.GetById(PetId).PetCategories.FirstOrDefault().Type, PetId.ToString())).ToList();
                }
            }
            public static List<string> GetAllGIFFileFromDirectory(Guid PetId)
            {
                using (var scope = App.ServiceProvider.CreateScope())
                {
                    var petService = scope.ServiceProvider.GetRequiredService<IPetRepository>();
                    return Directory.GetFiles(Path.Combine(LocalStorageSetting.LocalCache, petService.GetById(PetId).PetCategories.FirstOrDefault().Type, PetId.ToString())).Where(u=>u.EndsWith(".gif")).ToList();
                }
            }

        }
        internal static class FileUriSetting
        {
            public static string GetSpecificFileUri(Guid PetId)
            {
                return $"http://{RestApiSetting.IpAddress}:{RestApiSetting.Port}/api/file/downloadspecific?name={PetId}";
            }

        }

    }
}

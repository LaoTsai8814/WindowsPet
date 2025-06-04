using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
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

        //GetUserPetList
        public async Task<List<string>> GetUserFilesAsync(string token)
        {
            using var httpClient = new HttpClient();

            string url = $"http://{RestApiSetting.IpAddress}:{RestApiSetting.Port}/api/file/list?user={token}";
            var files = await httpClient.GetFromJsonAsync<List<string>>(url);

            return files ?? new List<string>();
        }
        //Download  User Pets
        public async Task DownloadFileAsync(string userId, string url, string savePath,string filename)
        {
            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(url);

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
                await File.WriteAllBytesAsync(Path.Combine(LocalStorageSetting.UserOwnPetLocation,filename), data);
                
                ZipFile.ExtractToDirectory(Path.Combine(LocalStorageSetting.UserOwnPetLocation, filename), Path.Combine(LocalStorageSetting.UserOwnPetLocation), overwriteFiles: true);

                File.Delete(Path.Combine(LocalStorageSetting.UserOwnPetLocation, filename));

                Console.WriteLine($"檔案已下載並解壓縮到 {LocalStorageSetting.UserOwnPetLocation}");              

                Console.WriteLine("非同步寫入成功");
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
        //Download All User Pets
        public async Task DownloadAllUserPets(string userId)
        {
            var remotepetlist = await GetUserFilesAsync(userId);

            if (!Directory.Exists(LocalStorageSetting.UserOwnPetLocation))
            {
                Directory.CreateDirectory(LocalStorageSetting.UserOwnPetLocation);
            }
            foreach (var petdir in remotepetlist)
            {
                string url = $"http://{RestApiSetting.IpAddress}:{RestApiSetting.Port}/api/file/download?user={userId}&name={Uri.EscapeDataString(petdir)}";
                Console.WriteLine($"Downloading {petdir}");
                await DownloadFileAsync(userId, url, LocalStorageSetting.UserOwnPetLocation,petdir);
            }
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
    }

}

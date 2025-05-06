using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static System.Net.WebRequestMethods;

namespace WindowsPet.Models
{
    internal class FileManager
    {
        private static FileManager? _instance;

        private CancellationToken UserPetCancelToken;
        private CancellationToken PopularPetCancelToken;

        public event Action<string,string>? OnPopluarPetDownloadComplete;
        public event Action<string,string>? OnUserPetDownloadComplete;
        public static FileManager Instance => _instance ??= new();
        
        
        readonly SftpClient sftp = new SftpClient(FTPSetting.host, FTPSetting.port, FTPSetting.username, FTPSetting.password);

        
        public void OpenFileManager()
        {

        }


        /// <summary>
        /// Download the pet data from the server
        /// and it will download the folder and all the files in it
        /// </summary>
        /// <param FileFolderName="name"></param>
        public async Task DownloadPoplarPet()
        {
            await sftp.ConnectAsync(PopularPetCancelToken);
            IEnumerable<Renci.SshNet.Sftp.ISftpFile> Folder
                = sftp.ListDirectory(@"/PopularPet");

            foreach (var folder in Folder)
            {
                if (!folder.IsDirectory)
                {
                    continue;
                }
                IEnumerable<Renci.SshNet.Sftp.ISftpFile> PetFolder = sftp.ListDirectory(@$"/PopularPet/{folder.Name}");
                if (folder.Name != "." && folder.Name != "..")
                {
                    if (!Directory.Exists(Path.Combine(Path.GetTempPath(), "PopularPet", folder.Name)))
                    {
                        Directory.CreateDirectory(Path.Combine(Path.GetTempPath(),"PopularPet", folder.Name));
                        Console.WriteLine("資料夾已建立：" + @$"/PopularPet/{folder.Name}");
                    }
                    foreach (var file in PetFolder)
                    {
                        // 忽略"." 和 ".."
                        if (file.Name != "." && file.Name != "..")
                        {
                            try
                            {
                                using (var fileStream = System.IO.File.Create(Path.Combine(Path.GetTempPath(), "PopularPet", folder.Name, file.Name)))
                                {
                                    sftp.DownloadFile(@$"{file.FullName}", fileStream);
                                    if (file.FullName.EndsWith(".png"))
                                    {
                                        OnPopluarPetDownloadComplete?.Invoke(folder.Name, Path.Combine(Path.GetTempPath(), "PopularPet", folder.Name, file.Name));
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error: " + ex.Message);
                                Console.WriteLine("Current remote directory: " + sftp.WorkingDirectory);
                                Console.WriteLine("Error: " + ex.Message);
                                sftp.Disconnect();

                            }
                        }
                    }
                }
            }
            
            // 遍历并打印文件名

            sftp.Disconnect();


        }



        /// <summary>
        /// UserPetList Should be a Directory
        /// </summary>
        /// <param name="UserPetList"></param>
        public async Task DownloadUserPet(List<Pet> UserPetList)
        {
            
            await sftp.ConnectAsync(UserPetCancelToken);
            IEnumerable<Renci.SshNet.Sftp.ISftpFile> Folder
                = sftp.ListDirectory(@"/AllPet");

            // 下載使用者的寵物資料
            foreach (var pet in UserPetList)
            {
                if(RemoteDirectoryExists(sftp, $@"/AllPet/{pet.Name}"))
                {
                    IEnumerable<Renci.SshNet.Sftp.ISftpFile> PetFolder
                = sftp.ListDirectory($@"/AllPet/{pet.Name}");
                    if (!Directory.Exists(Path.Combine(Path.GetTempPath(), "UserPet", pet.Name)))
                    {
                        Directory.CreateDirectory(Path.Combine(Path.GetTempPath(),"UserPet", pet.Name));
                        Console.WriteLine("資料夾已建立：" + @$"{pet.Name}");
                    }
                    foreach (var file in PetFolder)
                    {
                        // 忽略"." 和 ".."
                        if (file.Name == "." || file.Name == "..")
                        {
                            continue;
                        }
                        try
                        {
                            using (var fileStream = System.IO.File.Create(Path.Combine(Path.GetTempPath(), "UserPet", pet.Name, file.Name)))
                            {
                                sftp.DownloadFile(@$"{file.FullName}", fileStream);
                                if(file.FullName.EndsWith(".png"))
                                {
                                     OnUserPetDownloadComplete?.Invoke(pet.Name, Path.Combine(Path.GetTempPath(), "UserPet", pet.Name, file.Name));
                                    
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                            
                            Console.WriteLine("Current remote directory: " + sftp.WorkingDirectory);
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                }
                    
            }
            
            sftp.Disconnect();
        }




        /// <summary>
        /// Check if the remote directory exists
        /// </summary>
        /// <param name="sftp"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool RemoteDirectoryExists(SftpClient sftp, string path)
        {
            try
            {
                if (sftp.Exists(path))
                {
                    var attr = sftp.GetAttributes(path);
                    return attr.IsDirectory;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking directory: {ex.Message}");
                return false;
            }
        }



        static class FTPSetting
        {
            public static string host = "192.168.0.104";
            public static int port = 22; // Default SSH port
            public static string username = "iantsai05";
            public static string password = "Biglattes@61834";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WindowsPet.Models.ServiceInterface
{
    public interface IFileManager
    {
        Task<List<string>> GetAllFilesInFolderAsync(string petId);
        Task DownloadFileAsync(string url, string savePath, string filename);
        List<string> GetAllFileFromDirectory(Guid petId);
        List<string> GetAllGIFFileFromDirectory(Guid petId);
        string GetSpecificFileUri(Guid petId);
    }
}

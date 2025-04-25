using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces.FileStorage
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName);
        Task<bool> DeleteFileAsync(string fileUrl);
        Task<string> GetFileUrlAsync(string fileName);
        Task<bool> FileExistsAsync(string fileUrl);
        string GetStoragePath();
    }
}

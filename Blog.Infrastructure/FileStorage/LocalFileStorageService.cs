using Blog.Application.Interfaces.FileStorage;
using Blog.Infrastructure.FileStorage.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.FileStorage
{
    public class LocalFileStorageService : IFileStorageService
    {

        private readonly FileStorageSettings _settings;

        public LocalFileStorageService(IOptions<FileStorageSettings> settings)
        {
            _settings = settings.Value;

            if (!Directory.Exists(_settings.Local.UploadFolderPath))
            {
                Directory.CreateDirectory(_settings.Local.UploadFolderPath);
            }
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var filePath = Path.Combine(_settings.Local.UploadFolderPath, fileName);
            using var fileStreamOutput = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(fileStreamOutput);
            return $"/uploads/{fileName}";
        }

        public Task<bool> DeleteFileAsync(string fileUrl)
        {
            var filePath = Path.Combine(_settings.Local.UploadFolderPath, Path.GetFileName(fileUrl));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<string> GetFileUrlAsync(string fileName)
        {
            var filePath = Path.Combine(_settings.Local.UploadFolderPath, fileName);
            if (File.Exists(filePath))
            {
                return Task.FromResult($"/uploads/{fileName}");
            }
            return Task.FromResult<string>(null);
        }

        public Task<bool> FileExistsAsync(string fileUrl)
        {
            var filePath = Path.Combine(_settings.Local.UploadFolderPath, Path.GetFileName(fileUrl));
            return Task.FromResult(File.Exists(filePath));
        }

        public string GetStoragePath()
        {

            return _settings.BaseUrl;
        }
    }
}

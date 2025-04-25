using Blog.Application.Interfaces.FileStorage;
using Blog.Infrastructure.FileStorage.Settings;
using Microsoft.Extensions.Options;
using FluentFTP;
using System.Net;


namespace Blog.Infrastructure.FileStorage
{
    public class FtpFileStorageService : IFileStorageService
    {
        private readonly string _ftpHost;
        private readonly string _ftpUser;
        private readonly string _ftpPassword;
        private readonly string _ftpBaseFolder;
        private readonly FileStorageSettings _settings;

        public FtpFileStorageService(IOptions<FileStorageSettings> settings)
        {
            _settings = settings.Value;
            _ftpHost = _settings.FTP.Host;
            _ftpUser = _settings.FTP.Username;
            _ftpPassword = _settings.FTP.Password;
            _ftpBaseFolder = _settings.FTP.BaseFolder ?? "/";

        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            string remoteFilePath = $"{_ftpBaseFolder}/{fileName}";

            using var ftpClient = new FtpClient(_ftpHost, new NetworkCredential(_ftpUser, _ftpPassword));
            ftpClient.Connect();

            using (var ftpStream = ftpClient.OpenWrite(remoteFilePath))
            {
                await fileStream.CopyToAsync(ftpStream);
            }

            ftpClient.Disconnect();
            return $"{_ftpHost}/{remoteFilePath}";
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            string fileName = Path.GetFileName(fileUrl);
            string remoteFilePath = $"{_ftpBaseFolder}/{fileName}";

            using var ftpClient = new FtpClient(_ftpHost, new NetworkCredential(_ftpUser, _ftpPassword));
            ftpClient.Connect();

            if (ftpClient.FileExists(remoteFilePath))
            {
                ftpClient.DeleteFile(remoteFilePath);

                bool isDeleted = !ftpClient.FileExists(remoteFilePath);

                ftpClient.Disconnect();
                return isDeleted;
            }

            ftpClient.Disconnect();
            return false;
        }



        public async Task<string> GetFileUrlAsync(string fileName)
        {
            string remoteFilePath = $"{_ftpBaseFolder}/{fileName}";

            using var ftpClient = new FtpClient(_ftpHost, new NetworkCredential(_ftpUser, _ftpPassword));
            ftpClient.Connect();

            bool exists = ftpClient.FileExists(remoteFilePath);
            ftpClient.Disconnect();

            return exists ? $"{_ftpHost}/{remoteFilePath}" : null;
        }

        public async Task<bool> FileExistsAsync(string fileUrl)
        {
            string fileName = Path.GetFileName(fileUrl);
            string remoteFilePath = $"{_ftpBaseFolder}/{fileName}";

            using var ftpClient = new FtpClient(_ftpHost, new NetworkCredential(_ftpUser, _ftpPassword));
            ftpClient.Connect();

            bool exists = ftpClient.FileExists(remoteFilePath);
            ftpClient.Disconnect();
            return exists;
        }


        public string GetStoragePath()
        {
            return _settings.BaseUrl;
        }

    }
}

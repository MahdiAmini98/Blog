using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.FileStorage.Settings
{
    public class FileStorageSettings
    {
        public string BaseUrl { get; set; }
        public string StorageType { get; set; }
        public LocalStorageSettings Local { get; set; }
        public FtpStorageSettings FTP { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.FileStorage.Settings
{
    public class FtpStorageSettings
    {
        public bool Enabled { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string BaseFolder { get; set; }
    }
}

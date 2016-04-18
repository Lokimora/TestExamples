using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpHtml5Upload
{
    struct DirectoryItem
    {
        public Uri BaseUri;

        public string AbsolutePath => $"{BaseUri}/{Name}";

        public DateTime DateCreated;
        public bool IsDirectory;
        public string Name;
        public List<DirectoryItem> Items;
    }
}

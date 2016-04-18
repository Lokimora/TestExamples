using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public struct DirectoryItem
    {
        public Uri BaseUri { get; set; }
        public DateTime DateCreated;
        public bool IsDirectory;
        public string Name;
        public List<DirectoryItem> Items;
        
        public string AbsolutePath => $"{BaseUri}/{Name}";
    }
}

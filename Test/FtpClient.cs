using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class FtpClient
    {
        private FtpClient() { }

        private readonly string _userName;
        private readonly string _pass;
        private readonly string _baseAddress;
            

        public FtpClient(string baseAddress, string userName, string pass)
        {
            _baseAddress = baseAddress;
            _userName = userName;
            _pass = pass;
           
        }


        public  List<DirectoryItem> GetDirectoryInformation(string path)
        {
            FtpWebRequest request = (FtpWebRequest) FtpWebRequest.Create(_baseAddress);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(_userName, _pass);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            List<DirectoryItem> returnValue = new List<DirectoryItem>();
            string[] list = null;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                list = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            }

            foreach (string line in list)
            {
                // Windows FTP Server Response Format
                // DateCreated    IsDirectory    Name
                string data = line;

                // Parse date
                string date = data.Substring(0, 17);
                DateTime dateTime = DateTime.Parse(date);
                data = data.Remove(0, 24);

                // Parse <DIR>
                string dir = data.Substring(0, 5);
                bool isDirectory = dir.Equals("<dir>", StringComparison.InvariantCultureIgnoreCase);
                data = data.Remove(0, 5);
                data = data.Remove(0, 10);

                // Parse name
                string name = data;

                // Create directory info
                DirectoryItem item = new DirectoryItem();
                item.BaseUri = new Uri(_baseAddress);
                item.DateCreated = dateTime;
                item.IsDirectory = isDirectory;
                item.Name = name;

                Debug.WriteLine(item.AbsolutePath);
                item.Items = item.IsDirectory ? GetDirectoryInformation(item.AbsolutePath) : null;

                returnValue.Add(item);
            }

            return returnValue;
        }


    }
}

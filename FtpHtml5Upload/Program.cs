using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FtpHtml5Upload
{
    class Program
    {
        static void Main(string[] args)
        {
            Url url = new Url("ftp://localhost:21/240x400_targetex");



            string zipPaph = @"F:/html5.zip";

            WebClient wc = new WebClient();

            var directories = GetDirectoryInformation(url.Value, null, null);

            List<byte[]> bytes = new List<byte[]>();

            foreach (var v in directories)
            {
                bytes.Add(DownloadFtpFileds(v));
            }

            var data = bytes.SelectMany(p => p).ToArray();

            //using (MemoryStream stream = new MemoryStream(data))
            //{
            //    using (FileStream fs = new FileStream("F:/testZip.zip", FileMode.Create))
            //    {

            //        foreach (var VA)
            //        {
                        
            //        }

            //        using(ZipArchive archive )
            //    }
                
            //}


        }


        static byte[] DownloadFtpFileds(DirectoryItem item)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(item.AbsolutePath);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            List<byte[]> byteList = new List<byte[]>();

            if (item.IsDirectory)
            {
                foreach (var v in item.Items)
                {
                    byteList.Add(DownloadFtpFileds(v));
                }
            }

            else
            {
                using (var response = (FtpWebResponse) request.GetResponse())
                {
                    using (var mem = new MemoryStream())
                    {
                        response.GetResponseStream().CopyTo(mem);
                        byteList.Add(mem.ToArray());
                    }
                }
            }


            return byteList.SelectMany(p => p).ToArray();


        }


        static List<DirectoryItem> GetDirectoryInformation(string address, string username, string password)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(address);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;           
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
                item.BaseUri = new Uri(address);
                item.DateCreated = dateTime;
                item.IsDirectory = isDirectory;
                item.Name = name;

                Debug.WriteLine(item.AbsolutePath);
                item.Items = item.IsDirectory ? GetDirectoryInformation(item.AbsolutePath, username, password) : null;

                returnValue.Add(item);
            }

            return returnValue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Policy;


namespace FtpHtml5Upload
{
    class Program
    {
        private static void Main(string[] args)
        {
            Url url = new Url("ftp://localhost:21/240x400_targetex");



            string zipPaph = @"F:/html5.zip";

            WebClient wc = new WebClient();

            var directories = GetDirectoryInformation(url.Value, null, null).SelectMany(Expand).ToArray();



            using (FileStream fs = new FileStream(@"F:\banners.zip", FileMode.OpenOrCreate))
            {
                using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create))
                {                          
                    foreach (var v in directories)
                    {

                        var path = String.Join("/", v.BaseUri.AbsolutePath.Split('/').Skip(2));
                        var entry = archive.CreateEntry($"{path}/{v.Name}");
                        var data = DownloadFile(v.AbsolutePath);

                        
                        using (var stream = entry.Open())
                        {
                            using (var mem = new MemoryStream(data))
                            {
                              
                                mem.CopyTo(stream);
                            }
                        }

                       

                    }
                }
            }


        }

        static byte[] DownloadFile(string path)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(path);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.UsePassive = true;  
            request.UseBinary = true;
            request.KeepAlive = false;

            using (var mem = new MemoryStream())
            {
                using(var stream = request.GetResponse().GetResponseStream())
                {
                    stream.CopyTo(mem);
                }

                return mem.ToArray();

            }
           


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
                item.Items = item.IsDirectory ? GetDirectoryInformation(item.AbsolutePath, username, password) : new List<DirectoryItem>();

                returnValue.Add(item);
            }

            return returnValue;
        }


        public static List<DirectoryItem> Expand(DirectoryItem directory)
        {
            List<DirectoryItem> items = new List<DirectoryItem>();

            if (directory.Items.Any())
            {
                foreach (var v in directory.Items)
                {
                    items.AddRange(Expand(v));
                }

            }

            else
            {
                items.Add(directory);
            }

            return items;
        }

    }
}

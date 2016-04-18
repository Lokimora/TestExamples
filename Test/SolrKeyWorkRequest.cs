using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MicroSolr.Connectors;
using MicroSolr.Core.Clients;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Test
{
    public static class SolrKeyWorkRequest
    {
        public static long Request(string collectionName, string keyWord)
        {
            string[] urls = 
            {
              
            };

            if(urls == null || !urls.Any())
                throw new Exception("Необходимо передать хотябы один ");

            string availableUrl = null;

            foreach (var url in urls)
            {
                try
                {
                    var client = new WebClient();
                    client.DownloadString(url);

                    availableUrl = url;
                    break;
                }

                catch (Exception e)
                {
                    
                }

            }

            if(availableUrl == null)
                throw new Exception("Не один из удаленных ресурсов не ответил :(");

            var query = String.Format("visitor_action/select?q=keywords_day_array:{0}&indent=true&rows=0&group=true&group.field=vid_real&group.ngroups=true&group.limit=0&wt=json", keyWord);
            var solrUrl = new Uri(String.Format("{0}/{1}", availableUrl, query));
            

            WebClient clientSolr = new WebClient();


            JsonSerializer serializer = new JsonSerializer();
            var strJson = clientSolr.DownloadString(solrUrl);

            dynamic jObj  = JObject.Parse(strJson);

            dynamic  matches = jObj.grouped.vid_real.ngroups;

            return (long) matches;
         
        }
    }
}

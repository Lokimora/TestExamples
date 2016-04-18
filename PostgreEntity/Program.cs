using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceKit.OrmLite;

namespace PostgreEntity
{
    class Program
    {
        public static void Main(string[] args)
        {

            var crmHybridContext =
                new HybridCrmContext(
                   "connectionString");



            crmHybridContext.Db.CreateTable<Client>();
            crmHybridContext.Db.CreateTable<Contact>();




            //var client = new List<Client>
            //{
            //    new Client() { Id = 1, Name = "THEO NE DSSADSADSAD"},
            //    new Client() { Id  = 2, Name = "Two"}
            //};


            //var contact = new List<Contact>
            //{
            //    new Contact() {Id = 1,ClientId = 1},
            //    new Contact() { Id = 2 ,ClientId = 2}
            //};


            //crmHybridContext.Db.Insert(new Client(){Id = 1, Name = "One"});

            //crmHybridContext.Db.InsertAll(client);
            //crmHybridContext.Db.InsertAll(contact);

        }
    }
}

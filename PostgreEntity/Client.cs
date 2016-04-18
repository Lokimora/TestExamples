using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceKit.DataAnnotations;

namespace PostgreEntity
{
    [Table("NewClients")]
    public class Client
    {
        [PrimaryKey]
        public int ClientId { get; set; }

        [Column]
        public string Name { get; set; }

        /// <summary>
        /// Поле для связи
        /// </summary>
        [Column]
        public string InventoryId { get; set; }

        [Column]
        public string UserName { get; set; }

        [Column]
        public string Email { get; set; }

        [Column]
        public string Phone { get; set; }

        [Column]
        public DateTime RegistrationDate { get; set; }

        [Column]
        public int? Comission { get; set; }

        [Column]
        public int? Discount { get; set; }

        [Column]
        public string Comment { get; set; }

        [Column]
        public decimal LendingLimit { get; set; }

        [Column]
        public long? Responsible { get; set; }

        [Column]
        public DateTime LastUpdate { get; set; }

        [Column]
        public bool? IsSmartPixelSet { get; set; }

        [Column]
        [Association("client-contact", "ClientId", "ContactId")]
        public IEnumerable<Contact> Contacts { get; set; }
    }
}

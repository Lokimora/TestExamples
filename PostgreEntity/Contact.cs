using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceKit.DataAnnotations;

namespace PostgreEntity
{
    [Table("NewContacts")]
    public class Contact
    {
        [PrimaryKey]
        public int Id { get; set; }

        [Column]
        public string Fio { get; set; }

        [Column]
        public int ClientId { get; set; } 

    }
}

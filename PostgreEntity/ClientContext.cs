using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgreEntity
{
    internal class HybridCrmContext : IDbContext  {
        public HybridCrmContext(string connection) : base(connection)
        {
            
        }

       
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net.Models
{
    [Table("supermasters")]
    public class SuperMaster
    {
        public string IP { get; set; }
        public string Nameserver { get; set; }
        public string Account { get; set; }

    }
}

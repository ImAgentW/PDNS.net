using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static PDNS.net.Models.Domain;

namespace PDNS.net.Models
{
    public class DomainViewModel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Primary { get; set; }
        public string Email { get; set; }
        public int Refresh { get; set; }
        public int Retry { get; set; }
        public int Expire { get; set; }
        public int TTL { get; set; }
        public DomainType Type { get; set; }
    }
}

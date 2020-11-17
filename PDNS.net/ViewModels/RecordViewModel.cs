using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static PDNS.net.Models.Record;

namespace PDNS.net.Models
{
    public class RecordViewModel
    {
        [Key]
        public int ID { get; set; }
        public int DomainID { get; set; }
        public string Name { get; set; }
        public RecordType Type { get; set; }
        public string Content { get; set; }
        public int TTL { get; set; }
    }
}

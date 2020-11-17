using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net.Models
{
    [Table("domainmetadata")]
    public class DomainMetaData
    {
        [Key]
        public int ID { get; set; }
        public int Domain_ID { get; set; }
        public string Kind { get; set; }
        public string Content { get; set; }
    }
}

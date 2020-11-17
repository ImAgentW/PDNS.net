using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net.Models
{
    [Table("tsigkeys")]
    public class TSIGKey
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Algorithm { get; set; }
        public string Secret { get; set; }
    }
}

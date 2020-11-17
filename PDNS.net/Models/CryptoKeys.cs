using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net.Models
{
    [Table("cryptokeys")]
    public class CryptoKeys
    {
        public int ID { get; set; }
        public int Domain_ID { get; set; }
        public int Flags { get; set; }
        public bool Active { get; set; }

        [DefaultValue(true)]
        public bool Published { get; set; }
        public string Content { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net.Models
{
    [Table("Domains")]
    public class Domain
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Master { get; set; }
        public int Last_Check { get; set; }
        public string Type { get; set; }
        public string Notified_Serial { get; set; }
        public string Account { get; set; }

        public enum DomainType
        {
            MASTER,
            NATIVE,
            SLAVE
        }
    }
}

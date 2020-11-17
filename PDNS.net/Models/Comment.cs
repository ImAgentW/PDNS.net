using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net.Models
{
    [Table("comments")]
    public class Comments
    {
        [Key]
        public int ID { get; set; }
        public int Domain_ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Modified_at { get; set; }
        public int Account { get; set; }
        public int Comment { get; set; }
    }
}

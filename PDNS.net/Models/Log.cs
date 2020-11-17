using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net.Models
{
    [Table("Logs")]
    public class Log
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Data { get; set; }
        public string Type { get; set; }
        public DateTime Time { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [ForeignKey("Record")]
        public int RecordID { get; set; }

        public User User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net.Models
{
    public class LogViewModel
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Data { get; set; }
        public LogType Type { get; set; }
        public DateTime Time { get; set; }
        public int UserID { get; set; }
        public int RecordID { get; set; }

        public enum LogType
        {
            INSERT, UPDATE, DELETE
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PDNS.net.Data;
using PDNS.net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using static PDNS.net.Models.Log;

namespace PDNS.net
{
    public class Logger
    {
        private DBContext _context;

        public Logger(DBContext context)
        {
            _context = context;
        }

        public void Log(LogType type, string data, string title, int recordID)
        {
            _context.Logs.Add(new Log()
            {
                Time = DateTime.Now,
                Type = type.ToString(),
                UserID = 1,
                Data = data,
                Title = title,
                RecordID = recordID
            });
        }

        public enum LogType
        {
            INSERT, UPDATE, DELETE
        }
    }
}

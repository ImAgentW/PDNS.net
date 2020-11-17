using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net.Models
{
    public class UserViewModel
    {
        [Key]
        public int ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ModifyPassword { get; set; }
        public IFormFile? Profile { get; set; }

        private DateTime _birthday;
        public string Birthday
        {
            get => Tools.GetDate(_birthday);
            set => _birthday = Tools.GetDate(value);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Profile { get; set; }
        public DateTime Birthday { get; set; }
    }
}

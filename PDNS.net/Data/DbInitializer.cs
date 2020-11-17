using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PDNS.net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net.Data
{
    public class DbInitializer
    {
        //public static UserManager<User> UserManager { get; private set; }
        public static async void Initialize(DBContext context, IServiceProvider services)
        {


            var a = context.Domains.FromSqlRaw(@"CREATE TABLE IF NOT EXISTS `comments` (
  `id` int(11) NOT NULL,
  `domain_id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `type` varchar(10) NOT NULL,
  `modified_at` int(11) NOT NULL,
  `account` varchar(40) CHARACTER SET utf8 DEFAULT NULL,
  `comment` text CHARACTER SET utf8 NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;");
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }
            /*
            context.Users.Add(new User
            {
                ID = 1,
                Name = "Admin",
                Email = "admin@pdns.net",
                Username = "admin",
                Password = "admin",
                Birthday = new DateTime(2000, 3, 23),
                Profile = "assets/img/profiles/user.jpg"
            });
            */
            //context.SaveChanges();

            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var manager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                var user = new User
                {
                    Id = 1,
                    Name = "Admin",
                    Email = "admin@pdns.net",
                    UserName = "admin",
                    Birthday = new DateTime(2000, 3, 23),
                    Profile = "assets/img/profiles/user.jpg"
                };
                var result = await manager.CreateAsync(user, "@dm!N2020");
            }
        }
    }
}

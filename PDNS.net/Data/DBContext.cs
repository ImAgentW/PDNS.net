using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PDNS.net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net.Data
{
    public class DBContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Domain> Domains { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Indexes
            modelBuilder.Entity<Domain>(entity =>
            {
                entity.HasIndex(x => x.Name).HasName("name_index").IsUnique();
            });

            modelBuilder.Entity<Record>(entity =>
            {
                entity.HasIndex(x => new { x.Name, x.Type }).HasName("nametype_index");
                entity.HasIndex(x => x.Domain_ID).HasName("domain_id");
                entity.HasIndex(x => x.OrderName).HasName("ordername");
            });

            modelBuilder.Entity<Record>(entity =>
            {
                entity.HasIndex(x => new { x.Name, x.Type }).HasName("nametype_index");
                entity.HasIndex(x => x.Domain_ID).HasName("domain_id");
                entity.HasIndex(x => x.OrderName).HasName("ordername");
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.HasIndex(x => new { x.Name, x.Type }).HasName("comments_name_type_idx");
                entity.HasIndex(x => new { x.Domain_ID, x.Modified_at }).HasName("comments_order_idx");
            });

            modelBuilder.Entity<DomainMetaData>(entity =>
            {
                entity.HasIndex(x => new { x.Domain_ID, x.Kind }).HasName("domainmetadata_idx");
            });

            modelBuilder.Entity<CryptoKeys>(entity =>
            {
                entity.HasIndex(x => x.Domain_ID).HasName("domainidindex");
            });

            modelBuilder.Entity<TSIGKey>(entity =>
            {
                entity.HasIndex(x => new { x.Name, x.Algorithm }).HasName("namealgoindex").IsUnique();
            });
            #endregion

            #region Multi Column Keys
            modelBuilder.Entity<SuperMaster>(entity =>
            {
                entity.HasKey(x => new { x.IP, x.Nameserver });
            });
            #endregion

            /*
            //modelBuilder.Entity<IdentityUser>().ToTable("Users");
            modelBuilder.Entity<IdentityUser<int>>(entity =>
            {
                entity.ToTable(name: "Users");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.UserName).IsUnique();
            });
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");*/
        }
    }
}
/*
            PowerDNS Tables
migrationBuilder.Sql(Properties.Resources.unusedtables);
*/
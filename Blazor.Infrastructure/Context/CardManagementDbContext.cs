using CardManagement.Core.Domain.Activities;
using CardManagement.Core.Domain.Common;
using CardManagement.Core.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CardManagement.Infrastructure.Context
{
    public class CardManagementDbDbContext : IdentityDbContext<User, UserRole, Guid>
    {
        public CardManagementDbDbContext(DbContextOptions<CardManagementDbDbContext> options)
       : base(options)
        {
        }
     
        public DbSet<Accessibility> Accessibilities { get; set; }
  
        public DbSet<UserRoleAccessibility> UserRoleAccessibilities { get; set; }
        public DbSet<UserLoginInfo> UserLoginInfos { get; set; }
        public DbSet<UserOTP> UserOTP { get; set; }

        public DbSet<Contact> contacts { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique(false);
            });

        }
    }
}

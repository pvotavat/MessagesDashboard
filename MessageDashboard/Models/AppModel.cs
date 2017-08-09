using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class ApplicationUser : IdentityUser
    {

        public virtual ICollection<Messages> Messages { get; set; }
  
        // FirstName & LastName will be stored in a different table called MyUserInfo
        public virtual MyUserInfo MyUserInfo { get; set; }
    }

    public class MyUserInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Messages
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsActive { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
    public class MyDbContext : IdentityDbContext<ApplicationUser>
    {
        public MyDbContext()
            : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<IdentityUser>()
                .ToTable("Users");
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users");
        }

        public DbSet<Messages> Messages { get; set; }

        public DbSet<MyUserInfo> MyUserInfo { get; set; }
    }


}
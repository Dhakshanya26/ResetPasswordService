using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ResetPasswordApp
{
    public class MyDbContext : DbContext
    {
        public DbSet<UserDto> Users { get; set; }
        public DbSet<SentEmailDto> SentEmailDtos { get; set; }
        

        public MyDbContext(DbContextOptions builder):base(builder)
        {
            
        }
        protected virtual void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected  internal virtual void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SentEmailDto>().ToTable("SentEmails").HasKey(x => x.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}

using DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options)
        {

        }

        // Cretes a user table for storing all users list
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    //SeedData(modelBuilder);
        //}

        //private void SeedData(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().HasData(
        //        new User { Id = 1, Name = "Aman" },
        //        new User { Id = 2, Name = "Naman" },
        //        new User { Id = 3, Name = "Ankit" },
        //        new User { Id = 4, Name = "Sumit" });
        //}
    }
}

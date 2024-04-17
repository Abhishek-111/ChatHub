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
        // Message table for storing Conversation between two users 
        public DbSet<Message> Messages { get; set; }
        // Files table for storing users shared files 
        public DbSet<File> Files { get; set; }
        // FileBytesArray for storing the content of file in bytes array.
        public DbSet<FileByteArray> FileByteArrays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>()
                .HasMany(f => f.Messages)
                .WithOne(m => m.FileAttached)
                .HasForeignKey(m => m.FileId);

        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Entities;
using Task.Model;

namespace Task
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbName = "task_test";
            string dbUser = "root";
            string dbHost = "localhost";

            optionsBuilder.UseMySql(
                $"server={dbHost};UserId={dbUser};database={dbName};"
            );
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Blogpost> Blogposts { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<User> Users { get; set; }

    }
}

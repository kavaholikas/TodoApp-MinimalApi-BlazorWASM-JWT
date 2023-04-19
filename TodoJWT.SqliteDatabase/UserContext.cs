using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoJWT.Models;

namespace TodoJWT.SqliteDatabase
{
    internal class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TodoTask> Tasks { get; set; }

        public string DbPath { get; }

        public UserContext()
        {
            var folder = Environment.SpecialFolder.MyDocuments;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "TodoJwtUser.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}

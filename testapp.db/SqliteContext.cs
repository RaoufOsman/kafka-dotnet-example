using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace testapp.db
{
    public class SqliteContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public string DbPath { get; }

        public SqliteContext()
        {
            var path = Directory.GetCurrentDirectory();
            DbPath = Path.Join("C:\\sqlite", "kafka-messages.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}

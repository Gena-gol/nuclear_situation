using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqliteLib.Model
{
    public class NoteContext : DbContext
    {
        public static string Path = @"Data Source=Notes.db";
        public static string FilePath = @"Notes.db";

        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        public NoteContext(bool NeedCreate = false)
        {
            SQLitePCL.Batteries_V2.Init();
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_winsqlite3());

            if (NeedCreate)
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>().HasKey(n => n.ID);
            modelBuilder.Entity<User>().HasKey(n => n.ID);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite(Path);
        }
    }
}

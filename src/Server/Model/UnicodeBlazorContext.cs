using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnicodeBlazor.Shared;

namespace UnicodeBlazor.Server.Model
{
    public class UnicodeBlazorContext : DbContext
    {
        public UnicodeBlazorContext()
        {
            // moq
        }

        public UnicodeBlazorContext(DbContextOptions<UnicodeBlazorContext> options) : base(options)
        { }

        public virtual DbSet<UnicodeCharacterEntry> Entries { get; set; }
        public virtual DbSet<UnicodeBlockEntry> Blocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UnicodeCharacterEntry>().HasKey(e => new { e.Name, e.Codepos });
            modelBuilder.Entity<UnicodeBlockEntry>().HasKey(e => e.Name);
        }
    }
}

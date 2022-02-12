using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace codeTestTgx.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<Hotels> hotelsItems { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Hotels>(entity =>
            {
                entity.HasNoKey();

            });
        }*/
    }
}

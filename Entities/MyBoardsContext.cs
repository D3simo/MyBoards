using Microsoft.EntityFrameworkCore;

namespace MyBoards.Entities
{
    public class MyBoardsContext : DbContext
    {
        // construct
        public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
        {
            
        }
        // WorkItem tab with WorkItem properties
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }


        // konfiguracja modelu bazy danych
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItem>()
                .Property(x => x.State)
                .IsRequired();

            modelBuilder.Entity<WorkItem>()
                .Property(x => x.IterationPath)
                .HasColumnName("Iteration_Path");

            modelBuilder.Entity<WorkItem>()
                .Property(x => x.Area)
                .HasColumnType("varchar(200)");

            modelBuilder.Entity<WorkItem>(eb =>
            {
                eb.Property(wi => wi.IterationPath).HasColumnName("Iteration_Path");
                eb.Property(wi => wi.Effort).HasColumnType("decimal(5, 2)");
                eb.Property(wi => wi.EndDate).HasPrecision(3);
                eb.Property(wi => wi.Activity).HasMaxLength(200);
                eb.Property(wi => wi.RemainingWork).HasPrecision(14, 2);
            });
        }
    }
}

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


        // database model configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItem>(eb =>
            {
                eb.Property(wi => wi.State).IsRequired();
                eb.Property(wi => wi.Area).HasColumnType("varchar(200)");
                eb.Property(wi => wi.IterationPath).HasColumnName("Iteration_Path");
                eb.Property(wi => wi.Effort).HasColumnType("decimal(5, 2)");
                eb.Property(wi => wi.EndDate).HasPrecision(3);
                eb.Property(wi => wi.Activity).HasMaxLength(200);
                eb.Property(wi => wi.RemainingWork).HasPrecision(14, 2);

                // adding default values
                eb.Property(wi => wi.Priority).HasDefaultValue(1);
            });

            // 
            modelBuilder.Entity<Comment>(eb =>
            {
                // assign default current date using sql server
                eb.Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");

                // using EF to assign UpdatedDate
                eb.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate();
            });

            modelBuilder.Entity<User>()

                // configure 1:1 relation
                .HasOne(u => u.Address)
                .WithOne(a => a.User)

                // configure foreign key
                .HasForeignKey<Address>(a => a.UserId);
        }
    }
}

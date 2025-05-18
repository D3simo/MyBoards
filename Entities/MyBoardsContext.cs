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
        public DbSet<WorkItemState> WorkItemStates { get; set; }

        // database model configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItem>(eb =>
            {
                eb.Property(wi => wi.Area).HasColumnType("varchar(200)");
                eb.Property(wi => wi.IterationPath).HasColumnName("Iteration_Path");
                eb.Property(wi => wi.Effort).HasColumnType("decimal(5, 2)");
                eb.Property(wi => wi.EndDate).HasPrecision(3);
                eb.Property(wi => wi.Activity).HasMaxLength(200);
                eb.Property(wi => wi.RemainingWork).HasPrecision(14, 2);

                // configure relation with Comment entity
                eb.HasMany(w => w.Comments)
                .WithOne(c => c.WorkItem)
                .HasForeignKey(c => c.WorkItemId);

                // configure relations with User entity
                eb.HasOne(w => w.User)
                .WithMany(c => c.WorkItem)
                .HasForeignKey(c => c.UserId);

                // adding default values
                eb.Property(wi => wi.Priority).HasDefaultValue(1);

                // configure relationship between Tags and WorkItem
                eb.HasMany(w => w.Tags)
                .WithMany(c => c.WorkItems)
                // which entity do we use for this relationship
                .UsingEntity<WorkItemTag>(
                    w => w.HasOne(wit => wit.Tag)
                    .WithMany()
                    .HasForeignKey(wit => wit.TagId),

                    w => w.HasOne(wit => wit.WorkItem)
                    .WithMany()
                    .HasForeignKey(wit => wit.WorkItemId),

                    wit =>
                    {
                        wit.HasKey(x => new { x.TagId, x.WorkItemId });
                        wit.Property(x => x.PublicationDate).HasDefaultValueSql("getutcdate()");
                    });

                // relationship with WorkItemState
                eb.HasOne(w => w.State)
                .WithMany()
                .HasForeignKey(c => c.StateId);
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

            modelBuilder.Entity<WorkItemState>(eb =>
            {
                eb.Property(x => x.State).HasMaxLength(50);
                eb.Property(x => x.State).IsRequired();
            });
        }
    }
}

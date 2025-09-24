using Microsoft.EntityFrameworkCore;
using MyBoards.Entities.ViewModels;

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
        public DbSet<Epic> Epics { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<WorkItemState> WorkItemStates { get; set; }
        public DbSet<WorkItemTag> WorkItemTag { get; set; }
        public DbSet<TopAuthor> ViewTopAuthors { get; set; } // ViewModel for SQL View

        // database model configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItem>(eb =>
            {
                // relation with WorkItemState
                eb.HasOne(w => w.State)
                .WithMany()
                .HasForeignKey(w => w.StateId);

                eb.Property(wi => wi.Area).HasColumnType("varchar(200)");
                eb.Property(wi => wi.IterationPath).HasColumnName("Iteration_Path");

                // configure relation with Comment entity
                eb.HasMany(w => w.Comments)
                .WithOne(c => c.WorkItem)
                .HasForeignKey(c => c.WorkItemId);

                // configure relation with User entity
                eb.HasOne(w => w.Author)
                .WithMany(c => c.WorkItem)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete for WorkItems

                eb.Property(wi => wi.Priority).HasDefaultValue(1);

                // configure relation between Tags and WorkItem
                eb.HasMany(w => w.Tags)
                .WithMany(c => c.WorkItems)
                // which entity do we use for this relationship
                .UsingEntity<WorkItemTag>(
                    //for older .NET versions
                    //.HasKey(c => new { c.TagId, c.WorkItemId });

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
            });

            modelBuilder.Entity<Epic>() 
            
                .Property(wi => wi.EndDate)
                .HasPrecision(3);

            modelBuilder.Entity<Issue>()

                .Property(wi => wi.Efford)
                .HasColumnType("decimal(5, 2)");

            modelBuilder.Entity<Task>()

                .Property(wi => wi.Activity)
                .HasMaxLength(200);

            modelBuilder.Entity<Task>()
                .Property(wi => wi.RemainingWork)
                .HasPrecision(14, 2);

            modelBuilder.Entity<Comment>(eb =>
            {
                eb.Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");

                // using EF to assign UpdatedDate
                eb.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate();

                eb.HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AuthorId)
                // Cascade
                .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<User>()

                // configure 1:1 relation
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId);

            modelBuilder.Entity<WorkItemState>(eb =>
            {
                eb.Property(x => x.Value).HasMaxLength(50);
                eb.Property(x => x.Value).IsRequired();
            });

            //Database Seeding
            modelBuilder.Entity<WorkItemState>()
                .HasData(
                new WorkItemState() { Id = 1, Value = "To Do" },
                new WorkItemState() { Id = 2, Value = "Doing" },
                new WorkItemState() { Id = 3, Value = "Done" },
                new WorkItemState() { Id = 4, Value = "On Hold" },
                new WorkItemState() { Id = 5, Value = "Rejected" }
            );

            modelBuilder.Entity<Tag>()
                .HasData(
                new Tag() { Id = 1, Value = "Web", Category = "IT" },
                new Tag() { Id = 2, Value = "UI", Category = "IT" },
                new Tag() { Id = 3, Value = "Desktop", Category = "IT" },
                new Tag() { Id = 4, Value = "API", Category = "IT" },
                new Tag() { Id = 5, Value = "Service", Category = "IT" }
            );

            modelBuilder.Entity<TopAuthor>(eb =>
            {
                eb.ToView("View_TopAuthors");
                eb.HasNoKey();
            });
        }
    }
}

using Microsoft.EntityFrameworkCore;
using MyBoards.Entities.Configurations;
using MyBoards.Entities.ViewModels;

namespace MyBoards.Entities
{
    // class derived from System.Data.Entity.DbContext
    public class MyBoardsContext : DbContext
    {
        // construct
        public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
        {
            
        }
        // Entity sets (tables in the database)
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
            // Apply all configurations from the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            //or 1 by 1
            //new AddressConfiguration().Configure(modelBuilder.Entity<Address>());
            //new WorkItemConfiguration().Configure(modelBuilder.Entity<WorkItem>());
            //new WorkItemStateConfiguration().Configure(modelBuilder.Entity<WorkItemState>());
        }
    }
}

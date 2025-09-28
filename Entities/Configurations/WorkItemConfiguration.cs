using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBoards.Entities;

namespace MyBoards.Entities.Configurations
{
    public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> eb)
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
        }
    }
}
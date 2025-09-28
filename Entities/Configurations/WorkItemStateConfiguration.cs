using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBoards.Entities;
using System.Reflection.Emit;

namespace MyBoards.Entities.Configurations
{
    public class WorkItemStateConfiguration : IEntityTypeConfiguration<WorkItemState>
    {
        public void Configure(EntityTypeBuilder<WorkItemState> eb)
        {
            eb.Property(x => x.Value).HasMaxLength(50);
            eb.Property(x => x.Value).IsRequired();
            eb.HasData(
                new WorkItemState() { Id = 1, Value = "To Do" },
                new WorkItemState() { Id = 2, Value = "Doing" },
                new WorkItemState() { Id = 3, Value = "Done" },
                new WorkItemState() { Id = 4, Value = "On Hold" },
                new WorkItemState() { Id = 5, Value = "Rejected" }
             );
        }
    }
}
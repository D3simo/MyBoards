using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBoards.Entities.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> task)
        {
            task.Property(wi => wi.Activity)
                .HasMaxLength(200);
            task.Property(wi => wi.RemainingWork)
                .HasPrecision(14, 2);
        }
    }
}

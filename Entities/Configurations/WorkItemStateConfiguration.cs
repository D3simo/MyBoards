using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBoards.Entities.Configurations
{
    public class WorkItemStateConfiguration : IEntityTypeConfiguration<WorkItemState>
    {
        public void Configure(EntityTypeBuilder<WorkItemState> eb)
        {
            eb.Property(x => x.Value).HasMaxLength(50);
            eb.Property(x => x.Value).IsRequired();
        }
    }
}

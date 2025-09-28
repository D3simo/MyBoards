using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBoards.Entities.Configurations
{
    public class IssueConfiguration : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> issue)
        {
            issue.Property(wi => wi.Efford)
                .HasColumnType("decimal(5, 2)");
        }
    }
}

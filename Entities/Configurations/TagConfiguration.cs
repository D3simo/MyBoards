using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBoards.Entities.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> tag)
        {
            // Seeding
            tag.HasData(
                new Tag() { Id = 1, Value = "Web", Category = "IT" },
                new Tag() { Id = 2, Value = "UI", Category = "IT" },
                new Tag() { Id = 3, Value = "Desktop", Category = "IT" },
                new Tag() { Id = 4, Value = "API", Category = "IT" },
                new Tag() { Id = 5, Value = "Service", Category = "IT" }
            );
        }
    }
}

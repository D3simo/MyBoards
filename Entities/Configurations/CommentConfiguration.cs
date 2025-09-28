using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBoards.Entities.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> eb)
        {
            eb.Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");

            // using EF to assign UpdatedDate
            eb.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate();

            eb.HasOne(c => c.Author)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.AuthorId)
            // Cascade
            .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}

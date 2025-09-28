using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBoards.Entities;
using System.Reflection.Emit;

namespace MyBoards.Entities.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.OwnsOne(a => a.Coordinate, cb =>
            {
                cb.Property(c => c.Longtitude).HasPrecision(18,7);
                cb.Property(c => c.Latitude).HasPrecision(18, 7);
            });
        }
    }
}
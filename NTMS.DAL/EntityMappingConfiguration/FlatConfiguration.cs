using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTMS.Model;

namespace NTMS.DAL.EntityMappingConfiguration
{
    public class FlatConfiguration : IEntityTypeConfiguration<Flat>, INtmsMapping
    {
        public void Configure(EntityTypeBuilder<Flat> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_dbo.Flats");

            builder.Property(e => e.CleanerBill).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.GasBill).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Rent).HasColumnType("decimal(18, 2)");
        }
    }
}

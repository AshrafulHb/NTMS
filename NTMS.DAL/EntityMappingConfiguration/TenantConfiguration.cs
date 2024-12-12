using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTMS.Model;

namespace NTMS.DAL.EntityMappingConfiguration
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>, INtmsMapping
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_dbo.Tenants");

            builder.HasIndex(e => e.FlatId, "IX_Flat_Id");

            builder.Property(e => e.FlatId).HasColumnName("Flat_Id");
            builder.Property(e => e.Paddress).HasColumnName("PAddress");
            builder.Property(e => e.SecurityDeposit).HasColumnName("SecurityDeposit");

            builder.Property(e => e.StartDate).HasColumnType("datetime");

            builder.HasOne(d => d.Flat).WithMany(p => p.Tenants)
                .HasForeignKey(d => d.FlatId)
                .HasConstraintName("FK_dbo.Tenants_dbo.Flats_Flat_Id");
        }
    }
}

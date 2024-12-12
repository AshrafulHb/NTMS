using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTMS.Model;

namespace NTMS.DAL.EntityMappingConfiguration
{
    public class EbillingRuleConfiguration : IEntityTypeConfiguration<EbillingRule>, INtmsMapping
    {
        public void Configure(EntityTypeBuilder<EbillingRule> builder)
        {

            builder.HasKey(e => e.Id).HasName("PK_dbo.EBillingRules");

            builder.ToTable("EBillingRules");

            builder.Property(e => e.CommercialDc).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.CommercialRate).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.DemandCharge).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.MeterRent).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Rate1).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Rate2).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Rate3).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Rate4).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Vat).HasColumnType("decimal(18, 2)");
        }


    }
}

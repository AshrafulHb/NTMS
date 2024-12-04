using Microsoft.EntityFrameworkCore;
using NTMS.DAL.EntityMappingConfiguration;
using NTMS.Model;

namespace NTMS.DAL.DBContext;

public class NtmsContext : DbContext
{
    public NtmsContext()
    {
    }

    public NtmsContext(DbContextOptions<NtmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EbillingRule> EbillingRules { get; set; }

    public virtual DbSet<Emeter> Emeters { get; set; }

    public virtual DbSet<Ereading> Ereadings { get; set; }

    public virtual DbSet<Flat> Flats { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<TmpMonthlyBill> TmpMonthlyBills { get; set; }

    public virtual DbSet<UtilityOption> UtilityOptions { get; set; }

    public virtual DbSet<WbillingRule> WbillingRules { get; set; }

    public virtual DbSet<Wmeter> Wmeters { get; set; }

    public virtual DbSet<Wreading> Wreadings { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmeterConfiguration).Assembly, t => typeof(INtmsMapping).IsAssignableFrom(t));
    }

    /*   protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           modelBuilder.Entity<EbillingRule>(entity =>
           {
               entity.HasKey(e => e.Id).HasName("PK_dbo.EBillingRules");

               entity.ToTable("EBillingRules");

               entity.Property(e => e.DemandCharge).HasColumnType("decimal(18, 2)");
               entity.Property(e => e.MinimumCharge).HasColumnType("decimal(18, 2)");
               entity.Property(e => e.Rate1).HasColumnType("decimal(18, 2)");
               entity.Property(e => e.Rate2).HasColumnType("decimal(18, 2)");
               entity.Property(e => e.Rate3).HasColumnType("decimal(18, 2)");
               entity.Property(e => e.Rate4).HasColumnType("decimal(18, 2)");
               entity.Property(e => e.ServiceCharge).HasColumnType("decimal(18, 2)");
               entity.Property(e => e.Vat).HasColumnType("decimal(18, 2)");
           });

           modelBuilder.Entity<Emeter>(entity =>
           {
               entity.HasKey(e => e.Id).HasName("PK_dbo.EMeters");

               entity.ToTable("EMeters");

               entity.HasIndex(e => e.FlatId, "IX_Flat_Id");

               entity.Property(e => e.FlatId).HasColumnName("Flat_Id");

               entity.HasOne(d => d.Flat).WithMany(p => p.Emeters)
                   .HasForeignKey(d => d.FlatId)
                   .HasConstraintName("FK_dbo.EMeters_dbo.Flats_Flat_Id");
           });

           modelBuilder.Entity<Ereading>(entity =>
           {
               entity.HasKey(e => e.Id).HasName("PK_dbo.EReadings");

               entity.ToTable("EReadings");

               entity.HasIndex(e => e.EmeterId, "IX_EMeter_Id");

               entity.Property(e => e.EmeterId).HasColumnName("EMeter_Id");
               entity.Property(e => e.EndDate).HasColumnType("datetime");
               entity.Property(e => e.StartDate).HasColumnType("datetime");

               entity.HasOne(d => d.Emeter).WithMany(p => p.Ereadings)
                   .HasForeignKey(d => d.EmeterId)
                   .HasConstraintName("FK_dbo.EReadings_dbo.EMeters_EMeter_Id");
           });

           modelBuilder.Entity<Flat>(entity =>
           {
               entity.HasKey(e => e.Id).HasName("PK_dbo.Flats");

               entity.Property(e => e.Rent).HasColumnType("decimal(18, 2)");
           });

           modelBuilder.Entity<Tenant>(entity =>
           {
               entity.HasKey(e => e.Id).HasName("PK_dbo.Tenants");

               entity.HasIndex(e => e.FlatId, "IX_Flat_Id");

               entity.Property(e => e.FlatId).HasColumnName("Flat_Id");
               entity.Property(e => e.Paddress).HasColumnName("PAddress");
               entity.Property(e => e.StartDate).HasColumnType("datetime");

               entity.HasOne(d => d.Flat).WithMany(p => p.Tenants)
                   .HasForeignKey(d => d.FlatId)
                   .HasConstraintName("FK_dbo.Tenants_dbo.Flats_Flat_Id");
           });

           modelBuilder.Entity<TmpMonthlyBill>(entity =>
           {
               entity
                   .HasNoKey()
                   .ToTable("tmpMonthlyBills");

               entity.Property(e => e.BillEndDate).HasMaxLength(50);
               entity.Property(e => e.BillStartDate).HasMaxLength(50);
               entity.Property(e => e.BillingPeriod).HasMaxLength(50);
               entity.Property(e => e.CleanerBill).HasMaxLength(50);
               entity.Property(e => e.ConsumedElectricUnit).HasMaxLength(50);
               entity.Property(e => e.DemandCharge).HasMaxLength(50);
               entity.Property(e => e.ElectricMeterCurrentReading).HasMaxLength(50);
               entity.Property(e => e.ElectricMeterLastReading).HasMaxLength(50);
               entity.Property(e => e.ElectricMeterNo).HasMaxLength(50);
               entity.Property(e => e.ElectricityBill).HasMaxLength(50);
               entity.Property(e => e.ElectricityCharge).HasMaxLength(50);
               entity.Property(e => e.FlatCode).HasMaxLength(10);
               entity.Property(e => e.GasBill).HasMaxLength(50);
               entity.Property(e => e.HouseRent).HasMaxLength(50);
               entity.Property(e => e.Id).ValueGeneratedOnAdd();
               entity.Property(e => e.MinimumCharge).HasMaxLength(50);
               entity.Property(e => e.PrincipalAmount).HasMaxLength(50);
               entity.Property(e => e.ServiceCharge).HasMaxLength(50);
               entity.Property(e => e.TenantName).HasMaxLength(256);
               entity.Property(e => e.Total).HasMaxLength(50);
               entity.Property(e => e.TotalConsumedWaterUnit).HasMaxLength(50);
               entity.Property(e => e.Vat)
                   .HasMaxLength(50)
                   .HasColumnName("VAT");
               entity.Property(e => e.WaterBill).HasMaxLength(50);
               entity.Property(e => e.WaterMeter1ConsumedUnit).HasMaxLength(50);
               entity.Property(e => e.WaterMeter1CurrentReading).HasMaxLength(50);
               entity.Property(e => e.WaterMeter1LastReading).HasMaxLength(50);
               entity.Property(e => e.WaterMeter1No).HasMaxLength(50);
               entity.Property(e => e.WaterMeter2ConsumedUnit).HasMaxLength(50);
               entity.Property(e => e.WaterMeter2CurrentReading).HasMaxLength(50);
               entity.Property(e => e.WaterMeter2LastReading).HasMaxLength(50);
               entity.Property(e => e.WaterMeter2No).HasMaxLength(50);
               entity.Property(e => e.WaterUnitPrice).HasMaxLength(50);
           });

           modelBuilder.Entity<UtilityOption>(entity =>
           {
               entity.HasKey(e => e.Id).HasName("PK_dbo.UtilityOptions");

               entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");

               entity.HasMany(d => d.Flats).WithMany(p => p.UtilityOptions)
                   .UsingEntity<Dictionary<string, object>>(
                       "UtilityOptionFlat",
                       r => r.HasOne<Flat>().WithMany()
                           .HasForeignKey("FlatId")
                           .HasConstraintName("FK_dbo.UtilityOptionFlats_dbo.Flats_Flat_Id"),
                       l => l.HasOne<UtilityOption>().WithMany()
                           .HasForeignKey("UtilityOptionId")
                           .HasConstraintName("FK_dbo.UtilityOptionFlats_dbo.UtilityOptions_UtilityOption_Id"),
                       j =>
                       {
                           j.HasKey("UtilityOptionId", "FlatId").HasName("PK_dbo.UtilityOptionFlats");
                           j.ToTable("UtilityOptionFlats");
                           j.HasIndex(new[] { "FlatId" }, "IX_Flat_Id");
                           j.HasIndex(new[] { "UtilityOptionId" }, "IX_UtilityOption_Id");
                           j.IndexerProperty<int>("UtilityOptionId").HasColumnName("UtilityOption_Id");
                           j.IndexerProperty<int>("FlatId").HasColumnName("Flat_Id");
                       });
           });

           modelBuilder.Entity<WbillingRule>(entity =>
           {
               entity.HasKey(e => e.Id).HasName("PK_dbo.WBillingRules");

               entity.ToTable("WBillingRules");

               entity.Property(e => e.ServiceCharge).HasColumnType("decimal(18, 2)");
               entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
               entity.Property(e => e.Vat).HasColumnType("decimal(18, 2)");
           });

           modelBuilder.Entity<Wmeter>(entity =>
           {
               entity.HasKey(e => e.Id).HasName("PK_dbo.WMeters");

               entity.ToTable("WMeters");

               entity.HasIndex(e => e.FlatId, "IX_Flat_Id");

               entity.Property(e => e.FlatId).HasColumnName("Flat_Id");

               entity.HasOne(d => d.Flat).WithMany(p => p.Wmeters)
                   .HasForeignKey(d => d.FlatId)
                   .HasConstraintName("FK_dbo.WMeters_dbo.Flats_Flat_Id");
           });

           modelBuilder.Entity<Wreading>(entity =>
           {
               entity.HasKey(e => e.Id).HasName("PK_dbo.WReadings");

               entity.ToTable("WReadings");

               entity.HasIndex(e => e.WmeterId, "IX_EMeter_Id");

               entity.Property(e => e.EndDate).HasColumnType("datetime");
               entity.Property(e => e.StartDate).HasColumnType("datetime");
               entity.Property(e => e.WmeterId).HasColumnName("WMeter_Id");

               entity.HasOne(d => d.Wmeter).WithMany(p => p.Wreadings)
                   .HasForeignKey(d => d.WmeterId)
                   .HasConstraintName("FK_dbo.WReadings_dbo.WMeters_WMeter_Id");
           });

           OnModelCreatingPartial(modelBuilder);
       }

       partial void OnModelCreatingPartial(ModelBuilder modelBuilder);*/
}

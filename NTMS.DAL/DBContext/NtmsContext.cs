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

}

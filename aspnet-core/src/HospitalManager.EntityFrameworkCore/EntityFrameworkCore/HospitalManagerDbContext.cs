using HospitalManager.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace HospitalManager.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class HospitalManagerDbContext :
    AbpDbContext<HospitalManagerDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    #region Entities Hospital
    public DbSet<District> Districts { get; set; }

    public DbSet<Province> Provinces { get; set; }

    public DbSet<Ward> Wards { get; set; }

    public DbSet<Hospital> Hospitals { get; set; }

    public DbSet<Patient> Patients { get; set; }

    public DbSet<UserHospital> UserHospitals { get; set; }
    #endregion
    public HospitalManagerDbContext(DbContextOptions<HospitalManagerDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(HospitalManagerConsts.DbTablePrefix + "YourEntities", HospitalManagerConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        //{
        //    foreach (var entityType in builder.Model.GetEntityTypes())
        //    {
        //        var tableName = entityType.GetTableName();
        //        if (tableName.StartsWith("Abp")) entityType.SetTableName(tableName.Substring(3));
        //    }
        //}

        builder.Entity<Province>()
            .Property(x => x.Code)
            .IsRequired()
            .IsUnicode();
        builder.Entity<Province>()
            .HasIndex(x => x.Code)
            .IsUnique();

        builder.Entity<District>()
            .Property(x => x.Code)
            .IsRequired()
            .IsUnicode();
        builder.Entity<District>()
            .HasIndex(x => x.Code)
            .IsUnique();

        builder.Entity<Ward>()
            .Property(x => x.Code)
            .IsRequired()
            .IsUnicode();
        builder.Entity<Ward>()
            .HasIndex(x => x.Code)
            .IsUnique();

        builder.Entity<Patient>()
            .HasIndex(x => x.Code)
            .IsUnique();
        builder.Entity<Hospital>()
            .HasIndex(x => x.Code)
            .IsUnique();
    }
}

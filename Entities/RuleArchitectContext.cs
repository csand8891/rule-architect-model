using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions; // For removing pluralizing convention
using RuleArchitect.Entities; // Updated to use the new entities namespace

namespace RuleArchitect.Data // Suggested new namespace for the DbContext
{
    // Inherit from System.Data.Entity.DbContext for EF6
    public class RuleArchitectContext : DbContext
    {
        // DbSet properties represent the tables in your database
        public DbSet<SoftwareOption> SoftwareOptions { get; set; } = null!;
        public DbSet<OptionNumberRegistry> OptionNumberRegistries { get; set; } = null!;
        public DbSet<SoftwareOptionSpecificationCode> SoftwareOptionSpecificationCodes { get; set; } = null!;
        public DbSet<Requirement> Requirements { get; set; } = null!;
        public DbSet<ParameterMapping> ParameterMappings { get; set; } = null!;
        public DbSet<ControlSystem> ControlSystems { get; set; } = null!;
        public DbSet<SpecCodeDefinition> SpecCodeDefinitions { get; set; } = null!;
        public DbSet<MachineType> MachineTypes { get; set; } = null!;

        // Constructor for EF6.
        // "name=RuleArchitectSqliteConnection" refers to a connection string name in App.config or Web.config
        public RuleArchitectContext() : base("name=RuleArchitectSqliteConnection")
        {
            // Disable lazy loading if preferred (can improve performance in some scenarios)
            // this.Configuration.LazyLoadingEnabled = false;

            // Could also disable proxy creation if not using lazy loading or change tracking proxies
            // this.Configuration.ProxyCreationEnabled = false;

            // Database initialization strategy (e.g., CreateDatabaseIfNotExists, MigrateDatabaseToLatestVersion)
            // For SQLite with EF6, migrations can be tricky. Often CreateDatabaseIfNotExists is used for simpler setups or manual schema management.
            // Database.SetInitializer<RuleArchitectContext>(new CreateDatabaseIfNotExists<RuleArchitectContext>());
            // Or for migrations:
            // Database.SetInitializer(new MigrateDatabaseToLatestVersion<RuleArchitectContext, Migrations.Configuration>("RuleArchitectSqliteConnection"));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Remove pluralizing table name convention if you want table names to match DbSet names exactly
            // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // --- Configure Relationships and Constraints using EF6 Fluent API ---

            // SoftwareOption and ControlSystem (One-to-Many, ControlSystemId is Nullable)
            modelBuilder.Entity<SoftwareOption>()
                .HasOptional(so => so.ControlSystem)  // ControlSystem is optional for a SoftwareOption
                .WithMany(cs => cs.SoftwareOptions)  // A ControlSystem can have many SoftwareOptions
                .HasForeignKey(so => so.ControlSystemId)
                .WillCascadeOnDelete(false); // SetNull equivalent: if ControlSystem is deleted, set FK to null.

            // OptionNumberRegistry and SoftwareOption (Many-to-One, SoftwareOptionId is Required)
            modelBuilder.Entity<OptionNumberRegistry>()
                .HasRequired(onr => onr.SoftwareOption)
                .WithMany(so => so.RegisteredOptionNumbers)
                .HasForeignKey(onr => onr.SoftwareOptionId)
                .WillCascadeOnDelete(true); // Cascade delete

            // ParameterMapping and SoftwareOption (One-to-Many, SoftwareOptionId is Nullable)
            modelBuilder.Entity<ParameterMapping>().HasOptional(pm => pm.SoftwareOption)
                .WithMany(so => so.ParameterMappings)  // ParameterMappings collection might be null, handle with caution.
                .HasForeignKey(pm => pm.SoftwareOptionId).WillCascadeOnDelete(false);
            //Alternatively, a more verbose version with null check for WithMany can avoid CS8602 but might reduce readability slightly:
            //modelBuilder.Entity<ParameterMapping>().HasOptional(pm => pm.SoftwareOption).WithMany().HasForeignKey(pm => pm.SoftwareOptionId).WillCascadeOnDelete(false);
            //Then in SoftwareOption.cs, ensure ParameterMappings is initialized in the constructor or make it nullable ICollection<ParameterMapping>?

            // SpecCodeDefinition and MachineType (Many-to-One, MachineTypeId is Required)
            modelBuilder.Entity<SpecCodeDefinition>()
                .HasRequired(scd => scd.MachineType)
                .WithMany(mt => mt.SpecCodeDefinitions)
                .HasForeignKey(scd => scd.MachineTypeId)
                .WillCascadeOnDelete(false); // Restrict: Prevent deleting MachineType if used. EF6 default for required FK is cascade, so false makes it no-action/restrict.

            // SoftwareOptionSpecificationCode relationships
            modelBuilder.Entity<SoftwareOptionSpecificationCode>()
                .HasRequired(sosc => sosc.SoftwareOption)
                .WithMany(so => so.SpecificationCodes) //SpecificationCodes might be null, handle with caution.
                .HasForeignKey(sosc => sosc.SoftwareOptionId)
                .WillCascadeOnDelete(true); // Cascade

            modelBuilder.Entity<SoftwareOptionSpecificationCode>()
                .HasRequired(sosc => sosc.SpecCodeDefinition)
                .WithMany(scd => scd.SoftwareOptionSpecificationCodes) //SoftwareOptionSpecificationCodes might be null, handle with caution.
                .HasForeignKey(sosc => sosc.SpecCodeDefinitionId)
                .WillCascadeOnDelete(false); // Restrict: Prevent deleting SpecCodeDefinition if used.
            //Again, consider using a more verbose WithMany() with no argument to skip the back-navigation collection for one of these if still getting errors.

            // Requirement relationships
            modelBuilder.Entity<Requirement>()
                .HasRequired(r => r.SoftwareOption) // The SoftwareOption that HAS this requirement
                .WithMany(so => so.Requirements)    // Requirements collection might be null, handle with caution.
                .HasForeignKey(r => r.SoftwareOptionId)
                .WillCascadeOnDelete(true); // Cascade

            modelBuilder.Entity<Requirement>()
                .HasOptional(r => r.RequiredSoftwareOption) // Requirement MAY link to another SoftwareOption
                .WithMany(so => so.DependentRequirements)   // DependentRequirements collection might be null, handle with caution.
                .HasForeignKey(r => r.RequiredSoftwareOptionId)
                .WillCascadeOnDelete(false); // Restrict/No Action

            modelBuilder.Entity<Requirement>()
                .HasOptional(r => r.RequiredSpecCodeDefinition) // Requirement MAY link to a SpecCodeDefinition
                .WithMany(scd => scd.DependentRequirements)     // DependentRequirements collection might be null, handle with caution.
                .HasForeignKey(r => r.RequiredSpecCodeDefinitionId)
                .WillCascadeOnDelete(false); // Restrict/No Action

            // Note: Unique constraints for ControlSystem.Name and MachineType.Name
            // are handled by [Index(IsUnique=true)] attribute.
            // For the composite unique index on SpecCodeDefinition (SpecCodeNo, SpecCodeBit, MachineTypeId),
            // the multiple [Index("IX_SpecCodeNoBitMachineType", order, IsUnique = true)] attributes on properties.
            // are the EF6 way to define a composite index.
        }
    }
}
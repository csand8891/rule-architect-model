namespace RuleArchitect.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Data.SQLite.EF6;
    using System.Data.SQLite; // Add this using directive
    using System.Data.SQLite.EF6.Migrations;
    using RuleArchitect.Data; // Add this for RuleArchitectContext


    internal sealed class Configuration : DbMigrationsConfiguration<RuleArchitect.Data.RuleArchitectContext> // Ensure this matches your DbContext namespace and class
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false; // Recommended to keep this false for control
            ContextKey = "RuleArchitect.Data.RuleArchitectContext"; // Ensure this matches your DbContext's full name

            // Register the SQLite SQL generator.
            // The "System.Data.SQLite" invariant name should match what EF is looking for.
            SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator());
            SetSqlGenerator("System.Data.SQLite.EF6", new SQLiteMigrationSqlGenerator()); // Also register for the EF6 specific provider name
        }

        protected override void Seed(RuleArchitect.Data.RuleArchitectContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
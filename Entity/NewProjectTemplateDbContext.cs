using System;
using System.Data.Entity;
using Havit.Data.Entity;
using Havit.NewProjectTemplate.Entity.Migrations;

namespace Havit.NewProjectTemplate.Entity
{
	public class NewProjectTemplateDbContext : Havit.Data.Entity.DbContext
	{
        /// <summary>
        /// Nastav� (na �rovni aplikace) pou�it� Code Migrations strategie.
        /// </summary>
		public static void SetEntityFrameworkMigrations()
		{
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<NewProjectTemplateDbContext, Configuration>());
		}

	    /// <summary>
	    /// Spou�t� se z gener�toru k�du p�ed vytvo�en�m instance datab�zov�ho kontextu.
	    /// </summary>
	    public static void ConfigureForCodeGenerator(string connectionString)
	    {
	        DbConfiguration.SetConfiguration(new NewProjectTemplateDbConfiguration(connectionString, String.Empty));
	    }

        /// <summary>
        /// Konstruktor pro unit testy, nem� jin� vyu�it�.
        /// </summary>
	    internal NewProjectTemplateDbContext()
	    {
	        // NOOP
	    }

        /// <summary>
        /// Konstruktor.
        /// </summary>
        public NewProjectTemplateDbContext(string connectionString) : base(connectionString)
	    {
	        // NOOP
	    }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			
			modelBuilder.Configurations.AddFromAssembly(this.GetType().Assembly);
			modelBuilder.RegisterModelFromAssembly(typeof(Havit.NewProjectTemplate.Model.Localizations.Language).Assembly);
		}
	}
}
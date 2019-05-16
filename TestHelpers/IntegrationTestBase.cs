using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Havit.Data.EntityFrameworkCore;
using Havit.Data.Patterns.DataSeeds;
using Havit.NewProjectTemplate.DataLayer.Seeds.Core;
using Havit.NewProjectTemplate.WindsorInstallers;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.NewProjectTemplate.TestHelpers
{
	public class IntegrationTestBase
	{
		private IDisposable scope;

		protected IWindsorContainer Container { get; private set; }

		protected virtual bool UseLocalDb => false;
		protected virtual bool DeleteDbData => true;

		protected virtual bool SeedData => true;

		[TestInitialize]
		public virtual void TestInitialize()
		{
			IWindsorContainer container = new WindsorContainer();
			container.ConfigureForTests(useInMemoryDb: !UseLocalDb);
			// TODO container.Register(Component.For<IApplicationAuthenticationService>().ImplementedBy<FakeApplicationAuthenticationService>().LifeStyle.Transient);

			scope = container.BeginScope();

			var dbContext = container.Resolve<IDbContext>();
			if (DeleteDbData)
			{
				dbContext.Database.EnsureDeleted();
			}
			if (this.UseLocalDb)
			{
				dbContext.Database.Migrate();
				container.Release(dbContext);
			}

			if (this.SeedData)
			{
				var dataSeedRunner = container.Resolve<IDataSeedRunner>();
				dataSeedRunner.SeedData<CoreProfile>();
				container.Release(dataSeedRunner);
			}

			this.Container = container;
		}

		[TestCleanup]
		public virtual void TestCleanup()
		{
			scope.Dispose();
			this.Container.Dispose();
			this.Container = null;
		}
	}
}

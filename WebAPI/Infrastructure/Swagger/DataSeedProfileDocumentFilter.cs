using System.Collections.Generic;
using System.Linq;
using Havit.NewProjectTemplate.Facades.System;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Havit.NewProjectTemplate.WebAPI.Infrastructure.Swagger
{
	public class DataSeedProfileDocumentFilter : IDocumentFilter
	{
		private const string SystemSeedProfilePath = "/api/system/seed/{profile}";

		private readonly ILogger<DataSeedProfileDocumentFilter> logger;
		private readonly IDataSeedFacade dataSeedFacade;

		public DataSeedProfileDocumentFilter(ILogger<DataSeedProfileDocumentFilter> logger, IDataSeedFacade dataSeedFacade)
		{
			this.logger = logger;
			this.dataSeedFacade = dataSeedFacade;
		}

		public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
		{
			if (!swaggerDoc.Paths.TryGetValue(SystemSeedProfilePath, out PathItem item))
			{
				logger.LogTrace($"API operation {SystemSeedProfilePath} not found, cannot set list of available data seed profiles to its parameter");

				return;
			}

			if (item.Post.Parameters[0] is PartialSchema profileParameter)
			{
				logger.LogTrace($"Found API operation {SystemSeedProfilePath}, setting list of available data seed profiles as its parameter");

				IList<string> types = dataSeedFacade.GetDataSeedProfiles();

				logger.LogTrace($"Available data seed profiles: {string.Join(", ", types)}");
				profileParameter.Enum = types.Cast<object>().ToList();
			}
		}
	}
}
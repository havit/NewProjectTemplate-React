using System.Collections.Generic;

namespace Havit.NewProjectTemplate.Facades.System
{
    public interface IDataSeedFacade
    {
		void SeedDataProfile(string profileName);

        IList<string> GetDataSeedProfiles();
    }
}
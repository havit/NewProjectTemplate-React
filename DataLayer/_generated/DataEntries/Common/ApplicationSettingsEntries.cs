﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Data.EntityFrameworkCore.Patterns;
using Havit.Data.Patterns.DataEntries;
using Havit.Data.Patterns.Repositories;

namespace Havit.NewProjectTemplate.DataLayer.DataEntries.Common
{
	[System.CodeDom.Compiler.GeneratedCode("Havit.Data.EntityFrameworkCore.CodeGenerator", "1.0")]
	public class ApplicationSettingsEntries : DataEntries<Havit.NewProjectTemplate.Model.Common.ApplicationSettings>, IApplicationSettingsEntries 
	{
		public Havit.NewProjectTemplate.Model.Common.ApplicationSettings Current
        {
            get
            {
				if (current == null)
				{
					current = GetEntry(Havit.NewProjectTemplate.Model.Common.ApplicationSettings.Entry.Current);
				}
				return current;
            }
        }
		private Havit.NewProjectTemplate.Model.Common.ApplicationSettings current;

		public ApplicationSettingsEntries(IRepository<Havit.NewProjectTemplate.Model.Common.ApplicationSettings> repository)
			: base(repository)
		{
		}
	}
}
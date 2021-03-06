﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Havit.NewProjectTemplate.Web.Controllers.ViewModels;
using Microsoft.Extensions.Options;
using Havit.NewProjectTemplate.Web.Settings;

namespace Havit.NewProjectTemplate.Web.Controllers
{
    public class DefaultController : Controller
    {
        private readonly WebApiSettings webApiSettings;

        public DefaultController(IOptions<WebApiSettings> webApiOptions)
        {
            this.webApiSettings = webApiOptions.Value;
        }

        public IActionResult Index()
        {
            return View(GetViewModel());
        }

        private IndexViewModel GetViewModel()
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                AspPrerenderDataObject = new IndexViewModel.AspPrerenderData
                {
                    WebApiUrl = webApiSettings.WebApiUrl,
                }
            };
            return viewModel;
        }

        public IActionResult Error()
		{            
			return Index();
		}
	}
}

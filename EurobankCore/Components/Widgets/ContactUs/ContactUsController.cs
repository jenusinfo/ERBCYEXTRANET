using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Eurobank.Components.Widgets.ContactUs
{
	public class ContactUsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

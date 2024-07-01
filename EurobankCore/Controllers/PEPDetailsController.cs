using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Controllers
{
	public class PEPDetailsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

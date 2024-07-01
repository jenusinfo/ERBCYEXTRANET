using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.CustomTables;
using CMS.CustomTables.Types.Eurobank;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Globalization;
using CMS.Helpers;
using Eurobank.Models;
using Eurobank.Models.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eurobank.Controllers
{
	public class ApplicationController : Controller
	{
		public IActionResult Index(int ApplicationID = 0)
		{
			var countries = CountryInfoProvider.GetCountries().ToList();
			var data = countries.Select(x => new SelectListItem()
			{
				Text = x.CountryName,
				Value = x.CountryGUID.ToString()
			});
			ViewBag.Country = data;

			
			var lookupAddressType = new MultiDocumentQuery()
					.OnCurrentSite()
					.Path("/Lookups/General/Address-Type", PathTypeEnum.Children)
					.Culture("en-us")
					.PublishedVersion();
			var addressType = lookupAddressType.Select(x => new SelectListItem()
			{
				Text = x.DocumentName,
				Value = x.NodeGUID.ToString()
			});
			ViewBag.addressType = addressType;
			var genderData = new MultiDocumentQuery()
				.OnCurrentSite()
				.Path("/Lookups/General/GENDER", PathTypeEnum.Children)
				.Culture("en-us")
				.PublishedVersion();
			var gender = genderData.Select(x => new SelectListItem()
			{
				Text = x.DocumentName,
				Value = x.NodeGUID.ToString()
			});
			ViewBag.gender = gender;
			var titlesData = new MultiDocumentQuery()
				.OnCurrentSite()
				.Path("/Lookups/General/TITLES", PathTypeEnum.Children)
				.Culture("en-us")
				.PublishedVersion();
			var titles = titlesData.Select(x => new SelectListItem()
			{
				Text = x.DocumentName,
				Value = x.NodeGUID.ToString()
			});
			ViewBag.titles = titles;
			if(ApplicationID > 0)
			{
				TempData["IsEdit"] = ApplicationID;
				TempData["ApplicationID"] = ApplicationID;
				string customTableClassName = "Eurobank.TempPersonalDetails";
				DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
				if(customTable != null)
				{

					// Gets the first custom table record whose value in the 'ItemName' field is equal to "SampleName"
					var addressData = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("ItemID", ApplicationID).FirstOrDefault();

					ContactDetailViewModel contactDetailViewModel = new ContactDetailViewModel();
					contactDetailViewModel.Title = ValidationHelper.GetString(addressData.GetValue("Title"), "");
					contactDetailViewModel.FirstName = ValidationHelper.GetString(addressData.GetValue("FirstName"), "");
					contactDetailViewModel.LastName = ValidationHelper.GetString(addressData.GetValue("LastName"), "");
					contactDetailViewModel.FatherName = ValidationHelper.GetString(addressData.GetValue("FatherName"), "");
					contactDetailViewModel.DateofBirth = ValidationHelper.GetString(addressData.GetValue("DateofBirth"), "");
					contactDetailViewModel.Gender = ValidationHelper.GetString(addressData.GetValue("Gender"), "");
					contactDetailViewModel.ApplicationID = ValidationHelper.GetString(addressData.GetValue("ItemID"), "");

					return View(contactDetailViewModel);
				}
			}
			return View();
		}
		[HttpPost]
		public ActionResult Index(ContactDetailViewModel model)
		{
			bool active = false;
			if(model.Isactive=="on")
			{
				active = true;
			}
			if(!ModelState.IsValid)
			{

				return View(model);
			}
			else
			{
				if(Convert.ToInt32(model.ApplicationID) > 0)
				{

					string customTableClassName = "Eurobank.TempPersonalDetails";

					// Gets the custom table
					DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
					if(customTable != null)
					{
						// Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
						var customTableData = CustomTableItemProvider.GetItems(customTableClassName)
																			.WhereStartsWith("ItemID", model.ApplicationID);

						// Loops through individual custom table records
						foreach(CustomTableItem item in customTableData)
						{
							item.SetValue("Title", model.Title);
							item.SetValue("FirstName", model.FirstName);
							item.SetValue("LastName", model.LastName);
							item.SetValue("FatherName", model.FatherName);
							item.SetValue("Gender", model.Gender);
							item.SetValue("DateofBirth", model.DateofBirth);
							item.SetValue("Isactive", active);
							// Saves the changes to the database
							item.Update();
						}
					}
					return RedirectToAction("Index","Home");
				}

				else
				{
					TempPersonalDetailsItem tempPersonalDetailsItem = new TempPersonalDetailsItem();
					tempPersonalDetailsItem.Title = model.Title;
					tempPersonalDetailsItem.FirstName = model.FirstName;
					tempPersonalDetailsItem.LastName = model.LastName;
					tempPersonalDetailsItem.FatherName = model.FatherName;
					tempPersonalDetailsItem.Gender = model.Gender;
					tempPersonalDetailsItem.DateofBirth = model.DateofBirth;
					tempPersonalDetailsItem.SetValue("Isactive", active);
					tempPersonalDetailsItem.Insert();
					return RedirectToAction("Index", "Home");
				}


				
			}
		}

		public IActionResult Edit(int id)
		{
			if(id > 0)
			{

			}
			//var countries = CountryInfoProvider.GetCountries().ToList();
			//var data = countries.Select(x => new SelectListItem()
			//{
			//	Text = x.CountryName,
			//	Value = x.CountryGUID.ToString()
			//});
			//ViewBag.Country = data;


			//var lookupAddressType = new MultiDocumentQuery()
			//		.OnCurrentSite()
			//		.Path("/Lookups/General/Address-Type", PathTypeEnum.Children)
			//		.Culture("en-us")
			//		.PublishedVersion();
			//var addressType = lookupAddressType.Select(x => new SelectListItem()
			//{
			//	Text = x.DocumentName,
			//	Value = x.NodeGUID.ToString()
			//});
			//ViewBag.addressType = addressType;
			//var genderData = new MultiDocumentQuery()
			//	.OnCurrentSite()
			//	.Path("/Lookups/General/GENDER", PathTypeEnum.Children)
			//	.Culture("en-us")
			//	.PublishedVersion();
			//var gender = genderData.Select(x => new SelectListItem()
			//{
			//	Text = x.DocumentName,
			//	Value = x.NodeGUID.ToString()
			//});
			//ViewBag.gender = gender;
			//var titlesData = new MultiDocumentQuery()
			//	.OnCurrentSite()
			//	.Path("/Lookups/General/TITLES", PathTypeEnum.Children)
			//	.Culture("en-us")
			//	.PublishedVersion();
			//var titles = titlesData.Select(x => new SelectListItem()
			//{
			//	Text = x.DocumentName,
			//	Value = x.NodeGUID.ToString()
			//});
			//ViewBag.titles = titles;
			//if(ApplicationID > 0)
			//{
			//	TempData["IsEdit"] = ApplicationID;
			//	TempData["ApplicationID"] = ApplicationID;
			//	string customTableClassName = "Eurobank.TempPersonalDetails";
			//	DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
			//	if(customTable != null)
			//	{

			//		// Gets the first custom table record whose value in the 'ItemName' field is equal to "SampleName"
			//		var addressData = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("ItemID", ApplicationID).FirstOrDefault();

			//		ContactDetailViewModel contactDetailViewModel = new ContactDetailViewModel();
			//		contactDetailViewModel.Title = ValidationHelper.GetString(addressData.GetValue("Title"), "");
			//		contactDetailViewModel.FirstName = ValidationHelper.GetString(addressData.GetValue("FirstName"), "");
			//		contactDetailViewModel.LastName = ValidationHelper.GetString(addressData.GetValue("LastName"), "");
			//		contactDetailViewModel.FatherName = ValidationHelper.GetString(addressData.GetValue("FatherName"), "");
			//		contactDetailViewModel.DateofBirth = ValidationHelper.GetString(addressData.GetValue("DateofBirth"), "");
			//		contactDetailViewModel.Gender = ValidationHelper.GetString(addressData.GetValue("Gender"), "");
			//		contactDetailViewModel.ApplicationID = ValidationHelper.GetString(addressData.GetValue("ItemID"), "");

			//		return View(contactDetailViewModel);
			//	}
			//}
			return View();
		}

		[HttpPost]
		public ActionResult Edit(ApplicationViewModel model)
		{
			if(!ModelState.IsValid)
			{

				return View(model);
			}
			else
			{
				//if(Convert.ToInt32(model.ApplicationID) > 0)
				//{

				//	string customTableClassName = "Eurobank.TempPersonalDetails";

				//	// Gets the custom table
				//	DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
				//	if(customTable != null)
				//	{
				//		// Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
				//		var customTableData = CustomTableItemProvider.GetItems(customTableClassName)
				//															.WhereStartsWith("ItemID", model.ApplicationID);

				//		// Loops through individual custom table records
				//		foreach(CustomTableItem item in customTableData)
				//		{
				//			item.SetValue("Title", model.Title);
				//			item.SetValue("FirstName", model.FirstName);
				//			item.SetValue("LastName", model.LastName);
				//			item.SetValue("FatherName", model.FatherName);
				//			item.SetValue("Gender", model.Gender);
				//			item.SetValue("DateofBirth", model.DateofBirth);
				//			item.SetValue("Isactive", active);
				//			// Saves the changes to the database
				//			item.Update();
				//		}
				//	}
				//	return RedirectToAction("Index", "Home");
				//}

				//else
				//{
				//	TempPersonalDetailsItem tempPersonalDetailsItem = new TempPersonalDetailsItem();
				//	tempPersonalDetailsItem.Title = model.Title;
				//	tempPersonalDetailsItem.FirstName = model.FirstName;
				//	tempPersonalDetailsItem.LastName = model.LastName;
				//	tempPersonalDetailsItem.FatherName = model.FatherName;
				//	tempPersonalDetailsItem.Gender = model.Gender;
				//	tempPersonalDetailsItem.DateofBirth = model.DateofBirth;
				//	tempPersonalDetailsItem.SetValue("Isactive", active);
				//	tempPersonalDetailsItem.Insert();
				//	return RedirectToAction("Index", "Home");
				//}


				return View(model);

			}
		}
	}
}

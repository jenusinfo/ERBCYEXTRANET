using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Applicant
{
	public class OriginOfTotalAssetsModel
	{
		public int SlNo { get; set; }

		public int Id { get; set; }

		//[Required(ErrorMessageResourceName = "OriginOfTotalAssets", ErrorMessageResourceType = typeof(OriginOfTotalAssetsErrorMassage))]
		public string OriginOfTotalAssets { get; set; }

		public string OriginOfTotalAssetsName { get; set; }

		public string SpecifyOtherOrigin { get; set; }

		[RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "Only two digits allowed after decimal point")] //Accepts type 123, 123.4, 123.45
		public double? AmountOfTotalAsset { get; set; }

		//public double? AmountOfTotalAssetLong { get; set; }

		public string StatusName { get; set; }

		public bool Status { get; set; }

	}

	public class OriginOfTotalAssetsErrorMassage
	{
		public static string OriginOfTotalAssets
		{
			get
			{
				return ResHelper.GetString("Eurobank.Application.Applicant.OriginOfTotalAssets.Error.OriginOfTotalAssets");
			}
		}
		//public const string OriginOfTotalAssets = "Eurobank.Application.Applicant.OriginOfTotalAssets.Error.OriginOfTotalAssets";
		public const string SpecifyOtherOrigin = "Eurobank.Application.Applicant.OriginOfTotalAssets.Error.SpecifyOtherSource";
		public const string AmountOfTotalAsset = "Eurobank.Application.Applicant.OriginOfTotalAssets.Error.AmountOfIncome";
	}
}

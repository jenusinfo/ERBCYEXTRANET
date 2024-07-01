using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Eurobank.Models.Application.Applicant.LegalEntity
{
	public class CompanyFinancialInformationModel
	{
		public int Id { get; set; }

		//public decimal? Turnover { get; set; }

		////public decimal? NetProfitAndLoss { get; set; }

		//public decimal? NetProfitLoss { get; set; }

		//public decimal? TotalAssets { get; set; }
		//[RegularExpression(@"^\d{0,17}(\.\d{1,2})?$", ErrorMessage = " ")] 
		//[RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "Negative value or More than two digit after decimal is not allowed !")]
		public string Turnover { get; set; }

		//public decimal? NetProfitAndLoss { get; set; }
		//[RegularExpression(@"^-?\d{0,17}(\.\d{1,2})?$", ErrorMessage = " ")]
		public string NetProfitLoss { get; set; }

		//[RegularExpression(@"^\d{0,17}(\.\d{1,2})?$", ErrorMessage = " ")]
		public string TotalAssets { get; set; }
	}
}

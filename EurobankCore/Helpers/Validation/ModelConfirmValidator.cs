using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
	public class ModelConfirmValidator
	{
		public static ModelStateDictionary ValidateModel(ref ModelStateDictionary modelState, ValidationResultModel validationResultModel)
		{
			if (validationResultModel != null)
			{
				foreach (var item in validationResultModel.Errors)
				{
					modelState.AddModelError(item.PropertyName,item.ErrorMessage);
				}
			}
			return modelState;
		}
	}
}

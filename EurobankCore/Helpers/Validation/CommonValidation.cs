using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
	public class CommonValidation
	{
		public static bool IsEmailValid(string email)
		{
			bool retVal = false;
			string regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

			if(!string.IsNullOrEmpty(email))
			{
				retVal = Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
			}
			
			return retVal;
		}

		public static bool IsOnlyAlphabetsWithSpacesValid(string input)
		{
			bool retVal = true;
			string regex = @"^[a-zA-Z ]*$";

			if(!string.IsNullOrEmpty(input))
			{
				retVal = Regex.IsMatch(input, regex, RegexOptions.IgnoreCase);
			}

			return retVal;
		}

		public static bool IsOnlyAlphaNumericWithSpacesValid(string input)
		{
			bool retVal = true;
			string regex = @"^[a-zA-Z][a-zA-Z0-9 ]*$";

			if(!string.IsNullOrEmpty(input))
			{
				retVal = Regex.IsMatch(input, regex, RegexOptions.IgnoreCase);
			}

			return retVal;
		}
	}
}

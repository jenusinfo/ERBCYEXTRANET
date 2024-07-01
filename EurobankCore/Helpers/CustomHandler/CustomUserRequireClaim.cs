using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.CustomHandler
{
	public class CustomUserRequireClaim: IAuthorizationRequirement
	{
		public string ClaimType { get; }
		public CustomUserRequireClaim(string claimType)
		{
			ClaimType = claimType;
		}
	}
}

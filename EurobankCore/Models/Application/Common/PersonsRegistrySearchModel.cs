using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Common
{
	public class PersonsRegistrySearchModel
	{
		public int PersonRegistryId { get; set; }
		public string ApplicationNumber { get; set; }

		public string FullName { get; set; }

		public string NodeGUID { get; set; }
		public string NodeID { get; set; }
		public string Name { get; set; }
		public string ApplicationType { get; set; }
		public string ApplicationTypeName { get; set; }
		public string Title { get; set; }
		public string TitleName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FatherName { get; set; }
		public string Gender { get; set; }
		public string GenderName { get; set; }
		public string DateofBirth { get; set; }
		public string PlaceofBirth { get; set; }
		public string CountryofBirth { get; set; }
		public string CountryofBirthName { get; set; }
		public string EducationLevel { get; set; }
		public string EducationLevelName { get; set; }
		public string Citizenship { get; set; }
		public string CitizenshipName { get; set; }
		public string TypeofIdentification { get; set; }
		public string TypeofIdentificationName { get; set; }
		public string IdentificationNumber { get; set; }
		public string IssuingCountry { get; set; }
		public string IssuingCountryName { get; set; }
		public string IssueDate { get; set; }
		public string ExpiryDate { get; set; }

		public Nullable<DateTime> IssueDateTime { get; set; }
		public Nullable<DateTime> ExpiryDateTime { get; set; }

		public string MobileTelNoCountryCode { get; set; }
		public string MobileTelNoNumber { get; set; }
		public string HomeTelNoCountryCode { get; set; }
		public string HomeTelNoNumber { get; set; }
		public string WorkTelNoCountryCode { get; set; }
		public string WorkTelNoNumber { get; set; }
		public string FaxNoCountryCode { get; set; }
		public string EmailAddress { get; set; }
		public string FaxNoFaxNumber { get; set; }
		public string PreferredCommunicationLanguage { get; set; }
		public string PreferredCommunicationLanguageName { get; set; }
		public string ConsentforMarketingPurposes { get; set; }
		public string CreatedDate { get; set; }
		public string ModyfiedDate { get; set; }
		public string NodeAliaspath { get; set; }
	}
}

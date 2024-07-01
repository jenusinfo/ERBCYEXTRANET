using CMS.CustomTables;
using CMS.DataEngine;
using Eurobank.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
	public class PasswordHistoryProcess
	{
		public static bool CreatePasswordHistory(PasswordHistoryModel passwordHistory)
		{
			bool retVal = false;

			if(passwordHistory != null)
			{
				// Prepares the code name (class name) of the custom table to which the data record will be added
				string passwordHistoryTableClassName = "EuroBank.PasswordHistory";

				// Gets the custom table
				DataClassInfo passwordHistoryInfo = DataClassInfoProvider.GetDataClassInfo(passwordHistoryTableClassName);
				if(passwordHistoryInfo != null)
				{
					CustomTableItem passwordHistoryItem = CustomTableItem.New(passwordHistoryTableClassName);

					passwordHistoryItem.SetValue("UserId", passwordHistory.UserId);
					passwordHistoryItem.SetValue("Password", passwordHistory.Password);

					passwordHistoryItem.Insert();
				}

				List<PasswordHistoryModel> userPasswordHistories = GetPasswordHistoriesByUserId(passwordHistory.UserId);
				if(userPasswordHistories != null && userPasswordHistories.Count > 3)
				{
					var userPasswordHistoriesToDelete = userPasswordHistories.OrderBy(y => y.CreatedDateTime).Take(userPasswordHistories.Count - 3).ToList();

					userPasswordHistoriesToDelete.ForEach(u => DeletePasswordHistory(u.Id));
				}
			}

			return retVal;
		}

		public static List<PasswordHistoryModel> GetPasswordHistories()
		{
			List<PasswordHistoryModel> retVal = null;

			// Prepares the code name (class name) of the custom table
			string passwordHistoryTableClassName = "EuroBank.PasswordHistory";

			// Gets the custom table
			DataClassInfo passwordHistoryInfo = DataClassInfoProvider.GetDataClassInfo(passwordHistoryTableClassName);
			if(passwordHistoryInfo != null)
			{
				var allpasswordHistories = CustomTableItemProvider.GetItems(passwordHistoryTableClassName);
				if(allpasswordHistories != null)
				{
					retVal = allpasswordHistories.ToList().Select(h => new PasswordHistoryModel() { 
						Id = h.GetIntegerValue("ItemID", 0),
						UserId = h.GetIntegerValue("UserId", 0),
						Password = h.GetValue("Password", string.Empty),
						CreatedDateTime = h.GetDateTimeValue("ItemCreatedWhen", DateTime.MinValue)
					}).ToList();

				}
				//CustomTableItem item2 = CustomTableItemProvider.GetItems(passwordHistoryTableClassName)
				//													.WhereEquals("ItemName", "SampleName")
				//													.TopN(1)
				//													.FirstOrDefault();

				//string itemTextValue = ValidationHelper.GetString(item1.GetValue("ItemText"), "");
			}

			return retVal;
		}

		public static List<PasswordHistoryModel> GetPasswordHistoriesByUserId(int userId)
		{
			List<PasswordHistoryModel> retVal = null;

			if(userId > 0)
			{
				// Prepares the code name (class name) of the custom table
				string passwordHistoryTableClassName = "EuroBank.PasswordHistory";

				// Gets the custom table
				DataClassInfo passwordHistoryInfo = DataClassInfoProvider.GetDataClassInfo(passwordHistoryTableClassName);
				if(passwordHistoryInfo != null)
				{
					var allpasswordHistories = CustomTableItemProvider.GetItems(passwordHistoryTableClassName).WhereEquals("UserId", userId);
					if(allpasswordHistories != null)
					{
						retVal = allpasswordHistories.ToList().Select(h => new PasswordHistoryModel()
						{
							Id = h.GetIntegerValue("ItemID", 0),
							UserId = h.GetIntegerValue("ItemID", 0),
							Password = h.GetValue("Password", string.Empty),
							CreatedDateTime = h.GetDateTimeValue("ItemCreatedWhen", DateTime.MinValue)
						}).ToList();

					}
				}
			}
			

			return retVal;
		}

		public static bool DeletePasswordHistory(int passwordHistoryId)
		{
			bool retVal = false;

			if(passwordHistoryId > 0)
			{
				string passwordHistoryTableClassName = "EuroBank.PasswordHistory";

				DataClassInfo passwordHistoryInfo = DataClassInfoProvider.GetDataClassInfo(passwordHistoryTableClassName);
				if(passwordHistoryInfo != null)
				{
					CustomTableItem passwordHistoryItem = CustomTableItemProvider.GetItems(passwordHistoryTableClassName)
														.WhereEquals("ItemID", passwordHistoryId)
														.TopN(1)
														.FirstOrDefault();

					if(passwordHistoryItem != null)
					{
						passwordHistoryItem.Delete();
					}
				}
			}

			return retVal;
		}

		public static bool IsFoundPasswordHistories(int userId, string password)
		{
			bool retVal = false;

			if(userId > 0)
			{
				// Prepares the code name (class name) of the custom table
				string passwordHistoryTableClassName = "EuroBank.PasswordHistory";

				// Gets the custom table
				DataClassInfo passwordHistoryInfo = DataClassInfoProvider.GetDataClassInfo(passwordHistoryTableClassName);
				if(passwordHistoryInfo != null)
				{
					var allpasswordHistories = CustomTableItemProvider.GetItems(passwordHistoryTableClassName).WhereEquals("UserId", userId);
					if(allpasswordHistories != null)
					{
						retVal = allpasswordHistories.Any(r => string.Equals(r.GetValue("Password", string.Empty), password));

					}
				}
			}


			return retVal;
		}
	}
}

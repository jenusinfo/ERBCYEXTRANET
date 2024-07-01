using CMS.DocumentEngine.Types.Eurobank;
using DocumentFormat.OpenXml.Office2010.Excel;
using Eurobank.Helpers.Process;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Applications.DecisionHistory;
using Eurobank.Models.External;
using Eurobank.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.External.Application
{
	public class ApplicationApiProcess
	{
		public static ApplicationStatusResultModel ChangeApplicationStatus(string callerId, string applicationNumber, string status, string decision, string comments)
		{
			ApplicationStatusResultModel retval = new ApplicationStatusResultModel()
			{
				ErrorCode = "600",
				IsSuccess = false
			};

			if(!string.IsNullOrEmpty(applicationNumber))
			{
				if(!string.IsNullOrEmpty(status))
				{
					ApplicationWorkflowStatus managedStatus = ApplicationWorkFlowProcess.GetMatchedApplicationWorkflowStatusByStatusName(status);
					if(managedStatus != ApplicationWorkflowStatus.NONE)
					{
						int applicationId = ApplicationsProcess.GetApplicationId(applicationNumber);
						if(applicationId > 0)
						{
							ApplicationWorkFlowProcess.ChangeStatus(applicationId, managedStatus);


							//Save Status Change History

							ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);
							if(applicationDetails != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationNumber))
							{
								DecisionHistoryViewModel decisionHistoryModel = new DecisionHistoryViewModel()
								{
									DecisionHistory_Decision = ApplicationWorkFlowProcess.GetMatchedApplicationWorkflowStatus(managedStatus),
									//DecisionHistory_Stage = ApplicationWorkFlowProcess.GetNextStage
									//DecisionHistory_Comments = "Change done externally",  
									DecisionHistory_Comments = comments,  
									DecisionHistory_When = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
									DecisionHistory_Who = callerId
								};
								var decisionHistory = DecisionHistoryProcess.SaveDecisionHistorysModel(decisionHistoryModel, applicationDetails, "externalUser");
								retval.StatusCode = (int)ApplicationChangeStatusCode.SUCCESS;
								retval.Message = ApplicationChangeStatusResposeMsg.SUCCESS_MSG;
								retval.ErrorCode = "0";
								retval.IsSuccess = true;
							}
							else
							{
								retval.StatusCode = (int)ApplicationChangeStatusCode.APPLICATION_NUMBER_INVALID;
								retval.Message = ApplicationChangeStatusResposeMsg.APPLICATION_NUMBER_INVALID_MSG;
							}
						}
						else
						{
							retval.StatusCode = (int)ApplicationChangeStatusCode.APPLICATION_NUMBER_INVALID;
							retval.Message = ApplicationChangeStatusResposeMsg.APPLICATION_NUMBER_INVALID_MSG;
						}
					}
					else
					{
						retval.StatusCode = (int)ApplicationChangeStatusCode.STATUS_INVALID;
						retval.Message = ApplicationChangeStatusResposeMsg.STATUS_INVALID_MSG;
					}
				}
				else
				{
					retval.StatusCode = (int)ApplicationChangeStatusCode.STATUS_INVALID;
					retval.Message = ApplicationChangeStatusResposeMsg.STATUS_INVALID_MSG;
				}
			}
			else
			{
				retval.StatusCode = (int)ApplicationChangeStatusCode.APPLICATION_NUMBER_INVALID;
				retval.Message = ApplicationChangeStatusResposeMsg.APPLICATION_NUMBER_INVALID_MSG;
			}

			return retval;
		}

		public static ApplicationStatusResultModel ExecuteApplicationDecision(string userName, string applicationNumber, string decision, string comments)
		{
			ApplicationStatusResultModel retval = new ApplicationStatusResultModel()
			{
				ErrorCode = "600",
				IsSuccess = false
			};

			if(!string.IsNullOrEmpty(applicationNumber))
			{
				if(!string.IsNullOrEmpty(userName))
				{
					if(!string.IsNullOrEmpty(decision))
					{
						UserModel userInfo = UserProcess.GetUser(userName);
						if(userInfo != null && userInfo.UserInformation != null)
						{
							string managedDecision = ApplicationWorkFlowProcess.GetMatchedApplicationWorkflowDecisionGuid(decision);
							if(!string.IsNullOrEmpty(managedDecision))
							{                                
                                int applicationId = ApplicationsProcess.GetApplicationId(applicationNumber);
                                var AvlDecisionList = ApplicationWorkFlowProcess.GetApplicationWorkflowDecisions(userInfo, applicationId);

                                if (AvlDecisionList != null && AvlDecisionList.Any(x => x.Value == managedDecision))
                                {
									ApplicationWorkFlowResult resutl = ApplicationWorkFlowProcess.ExecuteWorkflow(userInfo, applicationId, managedDecision, string.Empty);
									if (resutl.IsSuccess)
									{
										//Save Status Change History

										ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);
										if (applicationDetails != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationNumber))
										{
											DecisionHistoryViewModel decisionHistoryModel = new DecisionHistoryViewModel()
											{
												DecisionHistory_Decision = managedDecision,
												DecisionHistory_Stage = ApplicationWorkFlowProcess.GetNextStage(userInfo, applicationId, managedDecision),
												DecisionHistory_Comments = comments,
												DecisionHistory_When = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
												DecisionHistory_Who = userName
											};
											var decisionHistory = DecisionHistoryProcess.SaveDecisionHistorysModel(decisionHistoryModel, applicationDetails, userName);
											if (decisionHistory != null)
											{
												retval.StatusCode = (int)ApplicationChangeStatusCode.SUCCESS;
												retval.IsSuccess = true;
												retval.Message = "Decision changed successfully.";
											}
										}
										else
										{
											retval.StatusCode = (int)ApplicationChangeStatusCode.APPLICATION_NUMBER_INVALID;
											retval.Message = "Invalid Application Number";
										}
									}
									else
									{
										retval.StatusCode = (int)ApplicationChangeStatusCode.DECISION_INVALID;
										retval.Message = "Invalid Decision";
									} 
								}
								else
								{
                                    retval.StatusCode = (int)ApplicationChangeStatusCode.DECISION_INVALID;
                                    retval.Message = "Invalid Decision";
                                }
							}
							else
							{
								retval.StatusCode = (int)ApplicationChangeStatusCode.DECISION_INVALID;
								retval.Message = "Invalid Decision";
							}
							
						}
						else
						{
							retval.StatusCode = (int)ApplicationChangeStatusCode.USER_NAME_INVALID;
							retval.Message = "Invalid User Id";
						}
						
					}
					else
					{
						retval.StatusCode = (int)ApplicationChangeStatusCode.DECISION_INVALID;
						retval.Message = "Invalid Decision";
					}
				}
				else
				{
					retval.StatusCode = (int)ApplicationChangeStatusCode.USER_NAME_INVALID;
					retval.Message = "Invalid User Id";
				}
				
			}
			else
			{
				retval.StatusCode = (int)ApplicationChangeStatusCode.APPLICATION_NUMBER_INVALID;
				retval.Message = "Invalid Application Number";
			}

			return retval;
		}

		public static ApplicationStatusResultModel ChangeApplicationStatus_BackUp(string userName, string applicationNumber, string status, string decision)
		{
			ApplicationStatusResultModel retval = new ApplicationStatusResultModel() { 
				ErrorCode = "600", IsSuccess = false 
			};

			if(!string.IsNullOrEmpty(applicationNumber))
			{
				if(!string.IsNullOrEmpty(status))
				{
					if(!string.IsNullOrEmpty(userName))
					{
						UserModel userInfo = UserProcess.GetUser(userName);
						if(userInfo != null)
						{
							ApplicationWorkflowStatus managedStatus = ApplicationWorkFlowProcess.GetMatchedApplicationWorkflowStatusByStatusName(decision);
							int applicationId = ApplicationsProcess.GetApplicationId(applicationNumber);
							ApplicationWorkFlowProcess.ChangeStatus(applicationId, managedStatus);


							//Save Status Change History

							ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);
							if(applicationDetails != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationNumber))
							{
								DecisionHistoryViewModel decisionHistoryModel = new DecisionHistoryViewModel()
								{
									DecisionHistory_Decision = ApplicationWorkFlowProcess.GetMatchedApplicationWorkflowStatus(managedStatus),
									//DecisionHistory_Stage = ApplicationWorkFlowProcess.GetNextStage
									DecisionHistory_Comments = "Change done externally",
									DecisionHistory_When = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
									DecisionHistory_Who = "externalUser"
								};
								var decisionHistory = DecisionHistoryProcess.SaveDecisionHistorysModel(decisionHistoryModel, applicationDetails, userName);
							}
						}
					}
					else
					{
						retval.StatusCode = (int)ApplicationChangeStatusCode.USER_NAME_INVALID;
					}
				}
				else
				{
					retval.StatusCode = (int)ApplicationChangeStatusCode.STATUS_INVALID;
				}
			}
			else
			{
				retval.StatusCode = (int)ApplicationChangeStatusCode.APPLICATION_NUMBER_INVALID;
			}

			return retval;
		}
	}
}

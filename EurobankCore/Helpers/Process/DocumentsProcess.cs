using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Applications;
using Eurobank.Models.Documents;
using EurobankAccountSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class DocumentsProcess
    {
        private static readonly string _ExpectedDocumentName = "Expected Doc";
        private static readonly string _BankDocumentName = "Bank Doc";
        public static List<DocumentsViewModel> GetBankDocuments(IEnumerable<TreeNode> treeNode, string entityType, string applicationNumber)
        {
            List<DocumentsViewModel> retVal = new List<DocumentsViewModel>();
            bool isLegalEntity = string.Equals(entityType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            if (isLegalEntity)
            {
                var documentsEntityName = CommonProcess.GetDocumentsEntityNameLegal(applicationNumber);
                //List<string> documentlist = new List<string>(new string[] { });
                foreach (var item in treeNode)
                {
                    //string DocumentType = ValidationHelper.GetString(item.GetValue("BankDocuments_DocumentType"), "");
                    //string PersonName = ValidationHelper.GetString(item.GetValue("BankDocuments_Entity"), "");
                    //if (!documentlist.Contains(DocumentType + PersonName))
                    //{
                    DocumentsViewModel documentsViewModel = new DocumentsViewModel();
                    documentsViewModel.DocId = ValidationHelper.GetInteger(item.GetValue("BankDocumentsID"), 0);

                    string entityValue = ValidationHelper.GetString(item.GetValue("BankDocuments_Entity"), "");
                    documentsViewModel.Entity_Name = (documentsEntityName != null && documentsEntityName.Count > 0 && entityValue != null && documentsEntityName.Any(f => f.Value == entityValue.ToString())) ? documentsEntityName.FirstOrDefault(f => f.Value == entityValue.ToString()).Text : string.Empty;
                    documentsViewModel.Entity = ValidationHelper.GetString(item.GetValue("BankDocuments_Entity"), "");
                    documentsViewModel.EntityType = ValidationHelper.GetString(item.GetValue("BankDocuments_EntityType"), "");
                    //documentsViewModel.EntityType_Name = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("BankDocuments_EntityType"), ""), Constants.CORPORATEACCOUNT_TYPE);
                    documentsViewModel.DocumentType = ValidationHelper.GetString(item.GetValue("BankDocuments_DocumentType"), "");
                    documentsViewModel.DocumentType_Name = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("BankDocuments_DocumentType"), ""), Constants.LegalBANK_DOCUMENT_TYPE);
                    documentsViewModel.FileUpload = ValidationHelper.GetString(item.GetValue("BankDocuments_FileUpload"), "");
                    documentsViewModel.EntityRole = ValidationHelper.GetString(item.GetValue("BankDocuments_EntityRole"), "");
                    documentsViewModel.EntityRole1 = ValidationHelper.GetString(item.GetValue("BankDocuments_EntityRole"), "");
                    documentsViewModel.EntityRole_Name = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("BankDocuments_EntityRole"), ""), Constants.LEGALPERSON_ROLE);
                    if (documentsViewModel.EntityRole_Name == "APPLICANT")
                    {
                        documentsViewModel.EntityType_Name = "APPLICANT";
                    }
                    else
                    {
                        documentsViewModel.EntityType_Name = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("BankDocuments_EntityType"), ""), Constants.Corporate_Entity_TYPE);
                    }
                    documentsViewModel.RequiresSignature = ValidationHelper.GetBoolean(item.GetValue("BankDocuments_RequiresSignature"), false);
                    documentsViewModel.RequiresSignatureStatus = ValidationHelper.GetBoolean(item.GetValue("BankDocuments_RequiresSignature"), false) == true ? "Yes" : "No";
                    documentsViewModel.uploadedBy = ServiceHelper.GetUserNameByID(ValidationHelper.GetInteger(item.DocumentCreatedByUserID, 0));
                    documentsViewModel.uploadedOn = Convert.ToDateTime(item.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                    documentsViewModel.Status = ValidationHelper.GetBoolean(item.GetValue("BankDocuments_Consent"), false) == true ? "Complete" : "Pending";
                    documentsViewModel.BankDocuments_Status_Name = ValidationHelper.GetBoolean(item.GetValue("BankDocuments_Status"), false) == true ? "Complete" : "Pending";
                    documentsViewModel.FileUpload = ValidationHelper.GetString(item.GetValue("BankDocuments_FileUpload"), "");
                    documentsViewModel.FileName = ValidationHelper.GetString(item.GetValue("FileName"), "");
                    documentsViewModel.ExternalFileGuid = ValidationHelper.GetString(item.GetValue("ExternalFileGuid"), "");
                    documentsViewModel.UploadFileName = ValidationHelper.GetString(item.GetValue("UploadFileName"), "");
                    retVal.Add(documentsViewModel);
                    //}
                    //documentlist.Add(DocumentType + PersonName);

                }
            }
            else
            {
                var documentsEntityName = CommonProcess.GetDocumentsEntityName(applicationNumber);
                //List<string> documentlist = new List<string>(new string[] { });
                foreach (var item in treeNode)
                {
                    //string DocumentType = ValidationHelper.GetString(item.GetValue("BankDocuments_DocumentType"), "");
                    //string PersonName = ValidationHelper.GetString(item.GetValue("BankDocuments_Entity"), "");
                    //if (!documentlist.Contains(DocumentType + PersonName))
                    //{
                    DocumentsViewModel documentsViewModel = new DocumentsViewModel();
                    documentsViewModel.DocId = ValidationHelper.GetInteger(item.GetValue("BankDocumentsID"), 0);
                    string entityValue = ValidationHelper.GetString(item.GetValue("BankDocuments_Entity"), "");
                    documentsViewModel.Entity_Name = (documentsEntityName != null && documentsEntityName.Count > 0 && entityValue != null) ? documentsEntityName.FirstOrDefault(f => f.Value == entityValue.ToString()).Text : string.Empty;
                    documentsViewModel.Entity = ValidationHelper.GetString(item.GetValue("BankDocuments_Entity"), "");
                    documentsViewModel.EntityType = ValidationHelper.GetString(item.GetValue("BankDocuments_EntityType"), "");
                    //documentsViewModel.EntityType_Name = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("BankDocuments_EntityType"), ""), Constants.Entity_TYPE);
                    documentsViewModel.DocumentType = ValidationHelper.GetString(item.GetValue("BankDocuments_DocumentType"), "");
                    documentsViewModel.DocumentType_Name = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("BankDocuments_DocumentType"), ""), Constants.BANK_DOCUMENT_TYPE);
                    documentsViewModel.FileUpload = ValidationHelper.GetString(item.GetValue("BankDocuments_FileUpload"), "");
                    documentsViewModel.EntityRole = ValidationHelper.GetString(item.GetValue("BankDocuments_EntityRole"), "");
                    documentsViewModel.EntityRole1 = ValidationHelper.GetString(item.GetValue("BankDocuments_EntityRole"), "");
                    documentsViewModel.EntityRole_Name = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("BankDocuments_EntityRole"), ""), Constants.PERSON_ROLE);
                    if (documentsViewModel.EntityRole_Name == "APPLICANT")
                    {
                        documentsViewModel.EntityType_Name = "APPLICANT";
                    }
                    else
                    {
                        documentsViewModel.EntityType_Name = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("BankDocuments_EntityType"), ""), Constants.Entity_TYPE);
                    }
                    documentsViewModel.RequiresSignature = ValidationHelper.GetBoolean(item.GetValue("BankDocuments_RequiresSignature"), false);
                    documentsViewModel.RequiresSignatureStatus = ValidationHelper.GetBoolean(item.GetValue("BankDocuments_RequiresSignature"), false) == true ? "Yes" : "No";
                    documentsViewModel.uploadedBy = ServiceHelper.GetUserNameByID(ValidationHelper.GetInteger(item.DocumentCreatedByUserID, 0));
                    documentsViewModel.uploadedOn = Convert.ToDateTime(item.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                    documentsViewModel.Status = ValidationHelper.GetBoolean(item.GetValue("BankDocuments_Consent"), false) == true ? "Complete" : "Pending";
                    documentsViewModel.BankDocuments_Status_Name = ValidationHelper.GetBoolean(item.GetValue("BankDocuments_Status"), false) == true ? "Complete" : "Pending";
                    documentsViewModel.FileUpload = ValidationHelper.GetString(item.GetValue("BankDocuments_FileUpload"), "");
                    documentsViewModel.FileName = ValidationHelper.GetString(item.GetValue("FileName"), "");
                    documentsViewModel.ExternalFileGuid = ValidationHelper.GetString(item.GetValue("ExternalFileGuid"), "");
                    documentsViewModel.UploadFileName = ValidationHelper.GetString(item.GetValue("UploadFileName"), "");
                    retVal.Add(documentsViewModel);
                    //}
                    //documentlist.Add(DocumentType + PersonName);
                }
            }

            return retVal;
        }

        public static DocumentsViewModel SaveBankDocumentsModel(DocumentsViewModel model, TreeNode treeNodeData, string applicationNumber, string applicationType)
        {
            DocumentsViewModel retVal = new DocumentsViewModel();
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            if (model != null)
            {
                if (treeNodeData != null)
                {
                    TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                    TreeNode documentsfoldernode_parent = tree.SelectNodes()
                        .Path(treeNodeData.NodeAliasPath + "/Documents")
                        .OnCurrentSite()
                        .Published(false)
                        .FirstOrDefault();
                    if (documentsfoldernode_parent == null)
                    {

                        documentsfoldernode_parent = TreeNode.New("CMS.Folder", tree);
                        documentsfoldernode_parent.DocumentName = "Documents";
                        documentsfoldernode_parent.DocumentCulture = "en-US";
                        documentsfoldernode_parent.Insert(treeNodeData);
                    }
                    TreeNode documentsBankfoldernode_parent = tree.SelectNodes()
                        .Path(documentsfoldernode_parent.NodeAliasPath + "/Bank-Documents")
                        .OnCurrentSite()
                        .Published(false)
                        .FirstOrDefault();
                    if (documentsBankfoldernode_parent == null)
                    {
                        documentsBankfoldernode_parent = TreeNode.New("CMS.Folder", tree);
                        documentsBankfoldernode_parent.DocumentName = "Bank Documents";
                        documentsBankfoldernode_parent.DocumentCulture = "en-US";
                        documentsBankfoldernode_parent.Insert(documentsfoldernode_parent);
                    }

                    TreeProvider tree1 = new TreeProvider(MembershipContext.AuthenticatedUser);
                    TreeNode bankDocumentsDetails = TreeNode.New("Eurobank.BankDocuments", tree);
                    string entityType = string.Empty;
                    string documentType = string.Empty;
                    if (isLegalEntity)
                    {
                        entityType = ServiceHelper.GetName(ValidationHelper.GetString(model.EntityType, ""), Constants.Corporate_Entity_TYPE);
                        documentType = ServiceHelper.GetName(ValidationHelper.GetString(model.DocumentType, ""), Constants.Corporate_BANK_DOCUMENT_TYPE);
                        model.EntityRole = CommonProcess.GetEntityRoleTypeLegal(model.Entity);

                    }
                    else
                    {
                        entityType = ServiceHelper.GetName(ValidationHelper.GetString(model.EntityType, ""), Constants.Entity_TYPE);
                        documentType = ServiceHelper.GetName(ValidationHelper.GetString(model.DocumentType, ""), Constants.BANK_DOCUMENT_TYPE);
                        model.EntityRole = CommonProcess.GetEntityRoleType(model.Entity);
                    }
                    //bankDocumentsDetails.DocumentName = ValidationHelper.GetString(entityType.Replace(" ", "") + " " + documentType.Replace(" ", ""), "");
                    bankDocumentsDetails.DocumentName = _BankDocumentName;

                    //string media = ServiceHelper.UploadFileToMedialibrary("Eurobank", "Eurobank", "", "", model.FileUpload, "BankDocuments");
                    string media = string.IsNullOrEmpty(model.FileUpload) ? "" : ServiceHelper.UploadFileToMedialibrary("Eurobank", "Eurobank", "", "", model.FileUpload, "BankDocuments");

                    bankDocumentsDetails.SetValue("BankDocuments_Entity", model.Entity);
                    bankDocumentsDetails.SetValue("BankDocuments_EntityType", model.EntityType);
                    bankDocumentsDetails.SetValue("BankDocuments_EntityRole", model.EntityRole);
                    bankDocumentsDetails.SetValue("BankDocuments_DocumentType", model.DocumentType);
                    bankDocumentsDetails.SetValue("BankDocuments_RequiresSignature", model.RequiresSignature);
                    bankDocumentsDetails.SetValue("BankDocuments_Consent", model.Consent);
                    bankDocumentsDetails.SetValue("BankDocuments_Status", model.BankDocuments_Status);
                    bankDocumentsDetails.SetValue("BankDocuments_FileUpload", media);
                    bankDocumentsDetails.SetValue("FileName", model.FileName);
                    bankDocumentsDetails.SetValue("ExternalFileGuid", model.ExternalFileGuid);
                    bankDocumentsDetails.SetValue("UploadFileName", model.UploadFileName);
                    bankDocumentsDetails.Insert(documentsBankfoldernode_parent);

                    retVal.DocId = ValidationHelper.GetInteger(bankDocumentsDetails.GetValue("BankDocumentsID"), 0);
                    retVal.uploadedBy = ServiceHelper.GetUserNameByID(ValidationHelper.GetInteger(bankDocumentsDetails.DocumentCreatedByUserID, 0));
                    retVal.uploadedOn = Convert.ToDateTime(bankDocumentsDetails.DocumentCreatedWhen).ToString("MM/dd/yyyy HH:mm:ss");
                    retVal.Status = ValidationHelper.GetBoolean(bankDocumentsDetails.GetValue("BankDocuments_Consent"), false) == true ? "Complete" : "Pending";
                }
            }

            if (isLegalEntity)
            {
                var documentsEntityName = CommonProcess.GetDocumentsEntityNameLegal(applicationNumber);
                var roleType = ServiceHelper.GetLegalPERSON_ROLE();
                retVal.Entity_Name = (documentsEntityName != null && documentsEntityName.Count > 0 && model.Entity != null) ? documentsEntityName.FirstOrDefault(f => f.Value == model.Entity.ToString()).Text : string.Empty;
                retVal.EntityRole_Name = (roleType != null && roleType.Count > 0 && model.EntityRole != null) ? roleType.FirstOrDefault(f => f.Value == model.EntityRole.ToString()).Text : string.Empty;
                if (retVal.EntityRole_Name == "APPLICANT")
                {
                    retVal.EntityType_Name = "APPLICANT";
                }
                else
                {
                    retVal.EntityType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.EntityType, ""), Constants.Corporate_Entity_TYPE);
                }

                retVal.DocumentType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.DocumentType, ""), Constants.LegalBANK_DOCUMENT_TYPE);
            }
            else
            {
                var documentsEntityName = CommonProcess.GetDocumentsEntityName(applicationNumber);
                var roleType = ServiceHelper.GetPERSON_ROLE();
                retVal.Entity_Name = (documentsEntityName != null && documentsEntityName.Count > 0 && model.Entity != null) ? documentsEntityName.FirstOrDefault(f => f.Value == model.Entity.ToString()).Text : string.Empty;
                retVal.EntityRole_Name = (roleType != null && roleType.Count > 0 && model.EntityRole != null) ? roleType.FirstOrDefault(f => f.Value == model.EntityRole.ToString()).Text : string.Empty;
                if (retVal.EntityRole_Name == "APPLICANT")
                {
                    retVal.EntityType_Name = "APPLICANT";
                }
                else
                {
                    retVal.EntityType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.EntityType, ""), Constants.Entity_TYPE);
                }
                retVal.DocumentType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.DocumentType, ""), Constants.BANK_DOCUMENT_TYPE);
            }

            retVal.Entity = model.Entity;
            retVal.EntityType = model.EntityType;
            retVal.EntityRole = model.EntityRole;
            retVal.EntityRole1 = model.EntityRole;
            retVal.RequiresSignatureStatus = ValidationHelper.GetBoolean(model.RequiresSignature, false) == true ? "Yes" : "No";
            retVal.DocumentType = model.DocumentType;
            retVal.BankDocuments_Status_Name = model.BankDocuments_Status == true ? "Complete" : "Pending";
            retVal.FileName = model.FileName;
            retVal.ExternalFileGuid = model.ExternalFileGuid;
            retVal.UploadFileName = model.UploadFileName;
            return retVal;
        }
        public static DocumentsViewModel UpdateBankDocumentsModel(DocumentsViewModel model, TreeNode bankDocumentsDetails, string applicationNumber, string applicationType)
        {
            DocumentsViewModel retVal = new DocumentsViewModel();


            if (bankDocumentsDetails != null)
            {

                string media = string.IsNullOrEmpty(model.FileUpload) ? "" : ServiceHelper.UploadFileToMedialibrary("Eurobank", "Eurobank", "", "", model.FileUpload, "BankDocuments");
                bankDocumentsDetails.SetValue("BankDocuments_Entity", model.Entity);
                bankDocumentsDetails.SetValue("BankDocuments_EntityType", model.EntityType);
                bankDocumentsDetails.SetValue("BankDocuments_EntityRole", model.EntityRole);
                bankDocumentsDetails.SetValue("BankDocuments_DocumentType", model.DocumentType);
                bankDocumentsDetails.SetValue("BankDocuments_RequiresSignature", model.RequiresSignature);
                bankDocumentsDetails.SetValue("BankDocuments_FileUpload", media);
                bankDocumentsDetails.SetValue("BankDocuments_Consent", model.Consent);
                bankDocumentsDetails.SetValue("BankDocuments_Status", model.BankDocuments_Status);
                bankDocumentsDetails.SetValue("FileName", model.FileName);
                bankDocumentsDetails.SetValue("ExternalFileGuid", model.ExternalFileGuid);
                bankDocumentsDetails.SetValue("UploadFileName", model.UploadFileName);
                bankDocumentsDetails.Update();
            }
            retVal.uploadedBy = ServiceHelper.GetUserNameByID(ValidationHelper.GetInteger(bankDocumentsDetails.DocumentCreatedByUserID, 0));
            retVal.uploadedOn = Convert.ToDateTime(bankDocumentsDetails.DocumentCreatedWhen).ToString("MM/dd/yyyy HH:mm:ss");
            retVal.Status = ValidationHelper.GetBoolean(bankDocumentsDetails.GetValue("BankDocuments_Consent"), false) == true ? "Complete" : "Pending";
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            if (isLegalEntity)
            {
                var documentsEntityName = CommonProcess.GetDocumentsEntityNameLegal(applicationNumber);
                var roleType = ServiceHelper.GetLegalPERSON_ROLE();
                //var roleType = ServiceHelper.GetPERSON_ROLE();
                retVal.Entity_Name = (documentsEntityName != null && documentsEntityName.Count > 0 && model.Entity != null) ? documentsEntityName.FirstOrDefault(f => f.Value == model.Entity.ToString()).Text : string.Empty;
                retVal.EntityRole_Name = (roleType != null && roleType.Count > 0 && model.EntityRole != null) ? roleType.FirstOrDefault(f => f.Value == model.EntityRole.ToString()).Text : string.Empty;
                if (retVal.EntityRole_Name == "APPLICANT")
                {
                    retVal.EntityType_Name = "APPLICANT";
                }
                else
                {
                    retVal.EntityType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.EntityType, ""), Constants.Corporate_Entity_TYPE);
                }

                retVal.DocumentType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.DocumentType, ""), Constants.LegalBANK_DOCUMENT_TYPE);
            }
            else
            {
                var documentsEntityName = CommonProcess.GetDocumentsEntityName(applicationNumber);
                var roleType = ServiceHelper.GetPERSON_ROLE();
                retVal.Entity_Name = (documentsEntityName != null && documentsEntityName.Count > 0 && model.Entity != null) ? documentsEntityName.FirstOrDefault(f => f.Value == model.Entity.ToString()).Text : string.Empty;
                retVal.EntityRole_Name = (roleType != null && roleType.Count > 0 && model.EntityRole != null) ? roleType.FirstOrDefault(f => f.Value == model.EntityRole.ToString()).Text : string.Empty;
                if (retVal.EntityRole_Name == "APPLICANT")
                {
                    retVal.EntityType_Name = "APPLICANT";
                }
                else
                {
                    retVal.EntityType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.EntityType, ""), Constants.Entity_TYPE);
                }
                retVal.DocumentType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.DocumentType, ""), Constants.BANK_DOCUMENT_TYPE);
            }

            retVal.Entity = model.Entity;
            retVal.EntityType = model.EntityType;
            retVal.EntityRole = model.EntityRole;
            retVal.FileName = model.FileName;
            retVal.ExternalFileGuid = model.ExternalFileGuid;
            retVal.UploadFileName = model.UploadFileName;
            retVal.RequiresSignatureStatus = ValidationHelper.GetBoolean(model.RequiresSignature, false) == true ? "Yes" : "No";
            retVal.DocumentType = model.DocumentType;
            retVal.DocId = model.DocId;
            retVal.BankDocuments_Status_Name = model.BankDocuments_Status == true ? "Complete" : "Pending";
            return retVal;
        }
        public static List<DocumentsViewModel> GetExpectedDocuments(IEnumerable<TreeNode> treeNode, string applicationNumber, string applicationType)
        {
            List<DocumentsViewModel> retVal = new List<DocumentsViewModel>();



            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            if (isLegalEntity)
            {
                var documentsEntityName = CommonProcess.GetDocumentsEntityNameLegal(applicationNumber);
                // List<string> documentlist = new List<string>(new string[] { });
                foreach (var item in treeNode)
                {
                    //string DocumentType = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_DocumentType"), "");
                    //string PersonName = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_Entity"), "");
                    //if (!documentlist.Contains(DocumentType + PersonName))
                    //{
                    DocumentsViewModel documentsViewModel = new DocumentsViewModel();
                    documentsViewModel.DocId = ValidationHelper.GetInteger(item.GetValue("ExpectedDocumentsID"), 0);
                    documentsViewModel.Entity = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_Entity"), "");
                    string entityValue = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_Entity"), "");
                    documentsViewModel.Entity_Name = (documentsEntityName != null && documentsEntityName.Count > 0 && entityValue != null && documentsEntityName.Any(f => f.Value == entityValue.ToString())) ? documentsEntityName.FirstOrDefault(f => f.Value == entityValue.ToString()).Text : string.Empty;

                    documentsViewModel.DocumentType = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_DocumentType"), "");
                    documentsViewModel.DocumentType_Name = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ExpectedDocuments_DocumentType"), ""), Constants.LEGALEXPECTED_DOCUMENT_TYPE);
                    documentsViewModel.FileUpload = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_FileUpload"), "");
                    documentsViewModel.EntityRole = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_EntityRole"), "");
                    documentsViewModel.FileName = ValidationHelper.GetString(item.GetValue("FileName"), "");
                    documentsViewModel.ExternalFileGuid = ValidationHelper.GetString(item.GetValue("ExternalFileGuid"), "");
                    documentsViewModel.UploadFileName = ValidationHelper.GetString(item.GetValue("UploadFileName"), "");
                    documentsViewModel.EntityRole_Name = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ExpectedDocuments_EntityRole"), ""), Constants.LEGALPERSON_ROLE);
                    if (documentsViewModel.EntityRole_Name == "APPLICANT")
                    {
                        documentsViewModel.EntityType_Name = "APPLICANT";
                    }
                    else
                    {
                        documentsViewModel.EntityType_Name = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ExpectedDocuments_EntityType"), ""), Constants.Corporate_Entity_TYPE);
                    }
                    documentsViewModel.EntityType = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_EntityType"), "");

                    documentsViewModel.RequiresSignature = ValidationHelper.GetBoolean(item.GetValue("ExpectedDocuments_RequiresSignature"), false);
                    documentsViewModel.RequiresSignatureStatus = ValidationHelper.GetBoolean(item.GetValue("ExpectedDocuments_RequiresSignature"), false) == true ? "Yes" : "No";
                    documentsViewModel.uploadedBy = ServiceHelper.GetUserNameByID(ValidationHelper.GetInteger(item.DocumentCreatedByUserID, 0));
                    documentsViewModel.uploadedOn = Convert.ToDateTime(item.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                    documentsViewModel.Status = ValidationHelper.GetBoolean(item.GetValue("ExpectedDocuments_Consent"), false) == true ? "Complete" : "Pending";
                    documentsViewModel.BankDocuments_Status_Name = ValidationHelper.GetBoolean(item.GetValue("ExpectedDocuments_Status"), false) == true ? "Complete" : "Pending";
                    retVal.Add(documentsViewModel);
                    //}
                    //documentlist.Add(DocumentType + PersonName);
                }
            }
            else
            {
                var documentsEntityName = CommonProcess.GetDocumentsEntityName(applicationNumber);
                // List<string> documentlist = new List<string>(new string[] { });
                foreach (var item in treeNode)
                {
                    //string DocumentType = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_DocumentType"), "");
                    //string PersonName = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_Entity"), "");
                    //if (!documentlist.Contains(DocumentType + PersonName))
                    //{
                    DocumentsViewModel documentsViewModel = new DocumentsViewModel();
                    documentsViewModel.DocId = ValidationHelper.GetInteger(item.GetValue("ExpectedDocumentsID"), 0);
                    documentsViewModel.Entity = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_Entity"), "");
                    string entityValue = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_Entity"), "");
                    documentsViewModel.Entity_Name = (documentsEntityName != null && documentsEntityName.Count > 0 && entityValue != null) ? documentsEntityName.FirstOrDefault(f => f.Value == entityValue.ToString()).Text : string.Empty;
                    documentsViewModel.EntityType = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_EntityType"), "");

                    documentsViewModel.DocumentType = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_DocumentType"), "");
                    documentsViewModel.DocumentType_Name = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ExpectedDocuments_DocumentType"), ""), Constants.EXPECTED_DOCUMENT_TYPE);
                    documentsViewModel.FileUpload = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_FileUpload"), "");
                    documentsViewModel.EntityRole = ValidationHelper.GetString(item.GetValue("ExpectedDocuments_EntityRole"), "");
                    documentsViewModel.FileName = ValidationHelper.GetString(item.GetValue("FileName"), "");
                    documentsViewModel.ExternalFileGuid = ValidationHelper.GetString(item.GetValue("ExternalFileGuid"), "");
                    documentsViewModel.UploadFileName = ValidationHelper.GetString(item.GetValue("UploadFileName"), "");
                    documentsViewModel.EntityRole_Name = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ExpectedDocuments_EntityRole"), ""), Constants.PERSON_ROLE);
                    if (documentsViewModel.EntityRole_Name == "APPLICANT")
                    {
                        documentsViewModel.EntityType_Name = "APPLICANT";
                    }
                    else
                    {
                        documentsViewModel.EntityType_Name = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ExpectedDocuments_EntityType"), ""), Constants.Entity_TYPE);
                    }
                    documentsViewModel.RequiresSignature = ValidationHelper.GetBoolean(item.GetValue("ExpectedDocuments_RequiresSignature"), false);
                    documentsViewModel.RequiresSignatureStatus = ValidationHelper.GetBoolean(item.GetValue("ExpectedDocuments_RequiresSignature"), false) == true ? "Yes" : "No";
                    documentsViewModel.uploadedBy = ServiceHelper.GetUserNameByID(ValidationHelper.GetInteger(item.DocumentCreatedByUserID, 0));
                    documentsViewModel.uploadedOn = Convert.ToDateTime(item.DocumentCreatedWhen).ToString("MM/dd/yyyy HH:mm:ss");
                    documentsViewModel.Status = ValidationHelper.GetBoolean(item.GetValue("ExpectedDocuments_Consent"), false) == true ? "Complete" : "Pending";
                    documentsViewModel.BankDocuments_Status_Name = ValidationHelper.GetBoolean(item.GetValue("ExpectedDocuments_Status"), false) == true ? "Complete" : "Pending";
                    retVal.Add(documentsViewModel);
                    //}

                    //documentlist.Add(DocumentType + PersonName);
                }
            }

            return retVal;
        }

        public static DocumentsViewModel SaveExpectedDocumentsModel(DocumentsViewModel model, TreeNode treeNodeData, string applicationNumber, string applicationType)
        {
            DocumentsViewModel retVal = new DocumentsViewModel();
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            if (model != null)
            {
                if (treeNodeData != null)
                {
                    TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                    TreeNode documentsfoldernode_parent = tree.SelectNodes()
                        .Path(treeNodeData.NodeAliasPath + "/Documents")
                        .OnCurrentSite()
                        .Published(false)
                        .FirstOrDefault();
                    if (documentsfoldernode_parent == null)
                    {

                        documentsfoldernode_parent = TreeNode.New("CMS.Folder", tree);
                        documentsfoldernode_parent.DocumentName = "Documents";
                        documentsfoldernode_parent.DocumentCulture = "en-US";
                        documentsfoldernode_parent.Insert(treeNodeData);
                    }
                    TreeNode documentsExpectedfoldernode_parent = tree.SelectNodes()
                        .Path(documentsfoldernode_parent.NodeAliasPath + "/Expected-Documents")
                        .OnCurrentSite()
                        .Published(false)
                        .FirstOrDefault();
                    if (documentsExpectedfoldernode_parent == null)
                    {
                        documentsExpectedfoldernode_parent = TreeNode.New("CMS.Folder", tree);
                        documentsExpectedfoldernode_parent.DocumentName = "Expected Documents";
                        documentsExpectedfoldernode_parent.DocumentCulture = "en-US";
                        documentsExpectedfoldernode_parent.Insert(documentsfoldernode_parent);
                    }
                    TreeNode expectedDocumentsDetails = TreeNode.New("Eurobank.ExpectedDocuments", tree);
                    string entityType = string.Empty;
                    string documentType = string.Empty;
                    if (isLegalEntity)
                    {
                        entityType = ServiceHelper.GetName(ValidationHelper.GetString(model.EntityType, ""), Constants.JURISDICTION);
                        documentType = ServiceHelper.GetName(ValidationHelper.GetString(model.DocumentType, ""), Constants.LEGALEXPECTED_DOCUMENT_TYPE);
                        model.EntityRole = CommonProcess.GetEntityRoleTypeLegal(model.Entity);
                    }
                    else
                    {
                        entityType = ServiceHelper.GetName(ValidationHelper.GetString(model.EntityType, ""), Constants.Entity_TYPE);
                        documentType = ServiceHelper.GetName(ValidationHelper.GetString(model.DocumentType, ""), Constants.EXPECTED_DOCUMENT_TYPE);
                        model.EntityRole = CommonProcess.GetEntityRoleType(model.Entity);
                    }
                    //expectedDocumentsDetails.DocumentName = ValidationHelper.GetString(entityType.Replace(" ", "") + " " + documentType.Replace(" ", ""), "");
                    expectedDocumentsDetails.DocumentName = _ExpectedDocumentName;


                    string media = string.IsNullOrEmpty(model.FileUpload) ? "" : ServiceHelper.UploadFileToMedialibrary("Eurobank", "Eurobank", "", "", model.FileUpload, "BankDocuments");
                    expectedDocumentsDetails.SetValue("ExpectedDocuments_Entity", model.Entity);
                    expectedDocumentsDetails.SetValue("ExpectedDocuments_EntityType", model.EntityType);
                    expectedDocumentsDetails.SetValue("ExpectedDocuments_EntityRole", model.EntityRole);
                    expectedDocumentsDetails.SetValue("ExpectedDocuments_DocumentType", model.DocumentType);
                    expectedDocumentsDetails.SetValue("ExpectedDocuments_RequiresSignature", model.RequiresSignature);
                    expectedDocumentsDetails.SetValue("ExpectedDocuments_Consent", model.Consent);
                    expectedDocumentsDetails.SetValue("ExpectedDocuments_Status", model.BankDocuments_Status);
                    expectedDocumentsDetails.SetValue("FileName", model.FileName);
                    expectedDocumentsDetails.SetValue("ExternalFileGuid", model.ExternalFileGuid);
                    expectedDocumentsDetails.SetValue("UploadFileName", model.UploadFileName);
                    expectedDocumentsDetails.SetValue("ExpectedDocuments_FileUpload", media);
                    expectedDocumentsDetails.Insert(documentsExpectedfoldernode_parent);
                    retVal.DocId = ValidationHelper.GetInteger(expectedDocumentsDetails.GetValue("ExpectedDocumentsID"), 0);
                    retVal.uploadedBy = ServiceHelper.GetUserNameByID(ValidationHelper.GetInteger(expectedDocumentsDetails.DocumentCreatedByUserID, 0));
                    retVal.Status = ValidationHelper.GetBoolean(expectedDocumentsDetails.GetValue("ExpectedDocuments_Consent"), false) == true ? "Complete" : "Pending";
                    retVal.uploadedOn = Convert.ToDateTime(expectedDocumentsDetails.DocumentCreatedWhen).ToString("MM/dd/yyyy HH:mm:ss");
                    retVal.FileUpload = media;

                }
            }

            if (isLegalEntity)
            {
                var documentsEntityName = CommonProcess.GetDocumentsEntityNameLegal(applicationNumber);
                var roleType = ServiceHelper.GetLegalPERSON_ROLE();
                retVal.Entity_Name = (documentsEntityName != null && documentsEntityName.Count > 0 && model.Entity != null) ? documentsEntityName.FirstOrDefault(f => f.Value == model.Entity.ToString()).Text : string.Empty;
                retVal.EntityRole_Name = (roleType != null && roleType.Count > 0 && model.EntityRole != null) ? roleType.FirstOrDefault(f => f.Value == model.EntityRole.ToString()).Text : string.Empty;
                if (retVal.EntityRole_Name == "APPLICANT")
                {
                    retVal.EntityType_Name = "APPLICANT";
                }
                else
                {
                    retVal.EntityType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.EntityType, ""), Constants.Corporate_Entity_TYPE);
                }
                retVal.DocumentType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.DocumentType, ""), Constants.LEGALEXPECTED_DOCUMENT_TYPE);
            }
            else
            {
                var documentsEntityName = CommonProcess.GetDocumentsEntityName(applicationNumber);
                var roleType = ServiceHelper.GetPERSON_ROLE();
                retVal.Entity_Name = (documentsEntityName != null && documentsEntityName.Count > 0 && model.Entity != null) ? documentsEntityName.FirstOrDefault(f => f.Value == model.Entity.ToString()).Text : string.Empty;
                retVal.EntityRole_Name = (roleType != null && roleType.Count > 0 && model.EntityRole != null) ? roleType.FirstOrDefault(f => f.Value == model.EntityRole.ToString()).Text : string.Empty;
                if (retVal.EntityRole_Name == "APPLICANT")
                {
                    retVal.EntityType_Name = "APPLICANT";
                }
                else
                {
                    retVal.EntityType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.EntityType, ""), Constants.Entity_TYPE);
                }
                retVal.DocumentType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.DocumentType, ""), Constants.EXPECTED_DOCUMENT_TYPE);
            }

            retVal.Entity = model.Entity;
            retVal.EntityType = model.EntityType;
            retVal.EntityRole = model.EntityRole;
            retVal.FileName = model.FileName;
            retVal.ExternalFileGuid = model.ExternalFileGuid;
            retVal.UploadFileName = model.UploadFileName;
            retVal.RequiresSignatureStatus = ValidationHelper.GetBoolean(model.RequiresSignature, false) == true ? "Yes" : "No";
            retVal.DocumentType = model.DocumentType;
            retVal.BankDocuments_Status_Name = model.BankDocuments_Status == true ? "Complete" : "Pending";
            return retVal;
        }
        public static DocumentsViewModel UpdateExpectedDocumentsModel(DocumentsViewModel model, TreeNode expectedDocumentsDetails, string applicationNumber, string applicationType)
        {
            DocumentsViewModel retVal = new DocumentsViewModel();
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            if (expectedDocumentsDetails != null)
            {
                if (isLegalEntity)
                {
                    model.EntityRole = CommonProcess.GetEntityRoleTypeLegal(model.Entity);
                }
                else
                {
                    model.EntityRole = CommonProcess.GetEntityRoleType(model.Entity);
                }

                string media = string.IsNullOrEmpty(model.FileUpload) ? "" : ServiceHelper.UploadFileToMedialibrary("Eurobank", "Eurobank", "", "", model.FileUpload, "BankDocuments");
                expectedDocumentsDetails.SetValue("ExpectedDocuments_Entity", model.Entity);
                expectedDocumentsDetails.SetValue("ExpectedDocuments_EntityType", model.EntityType);
                expectedDocumentsDetails.SetValue("ExpectedDocuments_EntityRole", model.EntityRole);
                expectedDocumentsDetails.SetValue("ExpectedDocuments_DocumentType", model.DocumentType);
                expectedDocumentsDetails.SetValue("ExpectedDocuments_RequiresSignature", model.RequiresSignature);
                expectedDocumentsDetails.SetValue("ExpectedDocuments_FileUpload", media);
                expectedDocumentsDetails.SetValue("ExpectedDocuments_Consent", model.Consent);
                expectedDocumentsDetails.SetValue("ExpectedDocuments_Status", model.BankDocuments_Status);
                expectedDocumentsDetails.SetValue("FileName", model.FileName);
                expectedDocumentsDetails.SetValue("ExternalFileGuid", model.ExternalFileGuid);
                expectedDocumentsDetails.SetValue("UploadFileName", model.UploadFileName);
                expectedDocumentsDetails.Update();
                retVal.FileUpload = media;

            }
            retVal.uploadedBy = ServiceHelper.GetUserNameByID(ValidationHelper.GetInteger(expectedDocumentsDetails.DocumentCreatedByUserID, 0));
            retVal.uploadedOn = Convert.ToDateTime(expectedDocumentsDetails.DocumentCreatedWhen).ToString("MM/dd/yyyy HH:mm:ss");
            retVal.Status = ValidationHelper.GetBoolean(expectedDocumentsDetails.GetValue("ExpectedDocuments_Consent"), false) == true ? "Complete" : "Pending";

            if (isLegalEntity)
            {
                var documentsEntityName = CommonProcess.GetDocumentsEntityNameLegal(applicationNumber);
                var roleType = ServiceHelper.GetLegalPERSON_ROLE();
                retVal.Entity_Name = (documentsEntityName != null && documentsEntityName.Count > 0 && model.Entity != null) ? documentsEntityName.FirstOrDefault(f => f.Value == model.Entity.ToString()).Text : string.Empty;
                retVal.EntityRole_Name = (roleType != null && roleType.Count > 0 && model.EntityRole != null) ? roleType.FirstOrDefault(f => f.Value == model.EntityRole.ToString()).Text : string.Empty;
                if (retVal.EntityRole_Name == "APPLICANT")
                {
                    retVal.EntityType_Name = "APPLICANT";
                }
                else
                {
                    retVal.EntityType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.EntityType, ""), Constants.Corporate_Entity_TYPE);
                }
                retVal.DocumentType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.DocumentType, ""), Constants.LEGALEXPECTED_DOCUMENT_TYPE);
            }
            else
            {
                var documentsEntityName = CommonProcess.GetDocumentsEntityName(applicationNumber);
                var roleType = ServiceHelper.GetPERSON_ROLE();
                retVal.Entity_Name = (documentsEntityName != null && documentsEntityName.Count > 0 && model.Entity != null) ? documentsEntityName.FirstOrDefault(f => f.Value == model.Entity.ToString()).Text : string.Empty;
                retVal.EntityRole_Name = (roleType != null && roleType.Count > 0 && model.EntityRole != null) ? roleType.FirstOrDefault(f => f.Value == model.EntityRole.ToString()).Text : string.Empty;
                if (retVal.EntityRole_Name == "APPLICANT")
                {
                    retVal.EntityType_Name = "APPLICANT";
                }
                else
                {
                    retVal.EntityType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.EntityType, ""), Constants.Entity_TYPE);
                }
                retVal.DocumentType_Name = ServiceHelper.GetName(ValidationHelper.GetString(model.DocumentType, ""), Constants.EXPECTED_DOCUMENT_TYPE);
            }

            retVal.Entity = model.Entity;
            retVal.EntityType = model.EntityType;
            retVal.EntityRole = model.EntityRole;
            retVal.FileName = model.FileName;
            retVal.ExternalFileGuid = model.ExternalFileGuid;
            retVal.UploadFileName = model.UploadFileName;
            retVal.RequiresSignatureStatus = ValidationHelper.GetBoolean(model.RequiresSignature, false) == true ? "Yes" : "No";
            retVal.DocumentType = model.DocumentType;
            retVal.DocId = model.DocId;
            retVal.BankDocuments_Status_Name = model.BankDocuments_Status == true ? "Complete" : "Pending";
            return retVal;
        }

        public static bool GenerateBankDocuments(int id, BankDocumentsRepository bankDocumentsRepository, ApplicationsRepository applicationsRepository, string applicationNumber, string applicationType, string personNodeGUID)
        {
            //Bank Document Delete
            var bankDocument = bankDocumentsRepository.GetBankDocuments(id).Where(x => x.BankDocuments_Entity.ToString() == personNodeGUID).ToList();
            if (bankDocument != null && bankDocument.Count > 0)
            {
                foreach (var item in bankDocument)
                {
                    item.Delete();
                }
            }
            //-----------------------------
            applicationType = ValidationHelper.GetString(ServiceHelper.GetEntityType(applicationType, Constants.APPLICATION_TYPE), "");
            bool isLegalEntity = string.Equals(applicationType, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase);

            int applicationID = ValidationHelper.GetInteger(id, 0);
            var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
            var bankDocuments = bankDocumentsRepository.GetBankDocuments(applicationID);
            IEnumerable<PersonalAndJointAccount_BankDocumentInfo> bankDoucumetsModulesIndividual = null;
            IEnumerable<CorporateAccount_BankDocumentInfo> bankDoucumetsModulesCorporate = null;
            if (isLegalEntity)
            {
                //Checking Person is Applicant OR Related Party
                string personRole = CommonProcess.GetEntityRoleTypeLegal(personNodeGUID);
                string personRoleName = ValidationHelper.GetString(ServiceHelper.GetName(personRole, Constants.LEGALPERSON_ROLE), "");
                if (string.Equals(personRoleName, "RELATED PARTY", StringComparison.OrdinalIgnoreCase))
                {
                    PersonalDetails personalDetails = PersonalDetailsProvider.GetPersonalDetails(new Guid(personNodeGUID), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    if (personalDetails != null)
                    {
                        //Check the Related party is INDIVIDUAL or LEGAL
                        string personType = personalDetails.PersonalDetails_Type;
                        string personTypeName = ServiceHelper.GetName(ValidationHelper.GetString(personType, ""), Constants.APPLICATION_TYPE);
                        string personTypeValue = CommonProcess.GetCorporateDocumentPersonTypeValue(personTypeName);
                        //if (personalDetails.PersonalDetails_IsRelatedPartyUBO == true)
                        //{
                        //    string documentTypeValue = CommonProcess.GetCorporateDocumentTypeValue("ULTIMATE BENEFICIAL OWNER");
                        //    bankDoucumetsModulesCorporate = bankDocumentsRepository.GetCorporateDocumentsModules().Where(x => x.CorporateAccount_BankDocument_PersonRole.ToString() == personRole && x.CorporateAccount_BankDocument_Type.ToString() == documentTypeValue && x.CorporateAccount_BankDocument_PersonType.ToString() == personTypeValue);
                        //    foreach (var item in bankDoucumetsModulesCorporate)
                        //    {
                        //        var checkBankDocuments = bankDocuments.ToList().Where(i => i.BankDocuments_EntityType == item.CorporateAccount_BankDocument_Type && i.BankDocuments_DocumentType == item.CorporateAccount_BankDocument_BankDocumentType && Convert.ToString(i.BankDocuments_Entity) == personNodeGUID);
                        //        if (checkBankDocuments.Count() == 0)
                        //        {
                        //            DocumentsViewModel documentsViewModel = new DocumentsViewModel();

                        //            documentsViewModel.Entity = personNodeGUID;
                        //            documentsViewModel.EntityType = ValidationHelper.GetString(item.CorporateAccount_BankDocument_Type, "");
                        //            //documentsViewModel.EntityRole = ValidationHelper.GetString(item.CorporateAccount_BankDocument_PersonRole, "");
                        //            documentsViewModel.DocumentType = ValidationHelper.GetString(item.CorporateAccount_BankDocument_BankDocumentType, "");

                        //            SaveBankDocumentsModel(documentsViewModel, applicatioData, applicationNumber, "LEGAL ENTITY");
                        //        }
                        //    }
                        //}
                        //else
                        //{

                        //string documentTypeValue = CommonProcess.GetCorporateDocumentTypeValue("OTHER THAN UBO/PARTNER");
                        if (personalDetails.PersonalDetails_IsRelatedPartyUBO == false)
                        {
                            string documentTypeValue = CommonProcess.GetCorporateDocumentTypeValue("OTHER THAN UBO/PARTNER");
                            bankDoucumetsModulesCorporate = bankDocumentsRepository.GetCorporateDocumentsModules().Where(x => x.CorporateAccount_BankDocument_PersonRole.ToString() == personRole && x.CorporateAccount_BankDocument_PersonType.ToString() == personTypeValue && x.CorporateAccount_BankDocument_Type.ToString() == documentTypeValue);
                            List<string> documentlist1 = new List<string>(new string[] { });
                            foreach (var item in bankDoucumetsModulesCorporate)
                            {
                                if (!documentlist1.Contains(item.CorporateAccount_BankDocument_BankDocumentType.ToString()))
                                {
                                    string relatedPartyroles = item.CorporateAccount_BankDocument_EntityRole.ToString();
                                    string relatedPartyrolesName = ValidationHelper.GetString(ServiceHelper.GetName(relatedPartyroles, Constants.Roles_Related_To_Entity), "");
                                        var checkBankDocuments = bankDocuments.ToList().Where(i => i.BankDocuments_EntityType == item.CorporateAccount_BankDocument_Type && i.BankDocuments_DocumentType == item.CorporateAccount_BankDocument_BankDocumentType && Convert.ToString(i.BankDocuments_Entity) == personNodeGUID);
                                        if (checkBankDocuments.Count() == 0)
                                        {
                                            DocumentsViewModel documentsViewModel = new DocumentsViewModel();

                                            documentsViewModel.Entity = personNodeGUID;
                                            documentsViewModel.EntityType = ValidationHelper.GetString(item.CorporateAccount_BankDocument_Type, "");
                                            //documentsViewModel.EntityRole = ValidationHelper.GetString(item.CorporateAccount_BankDocument_PersonRole, "");
                                            documentsViewModel.DocumentType = ValidationHelper.GetString(item.CorporateAccount_BankDocument_BankDocumentType, "");

                                            SaveBankDocumentsModel(documentsViewModel, applicatioData, applicationNumber, "LEGAL ENTITY");
                                        }
                                    
                                }
                                documentlist1.Add(item.CorporateAccount_BankDocument_BankDocumentType.ToString());
                            }
                        }
                        var roles = CommonProcess.GetEntitySeletedRolesLegal(personNodeGUID, personalDetails.PersonalDetails_IsRelatedPartyUBO);
                        bankDoucumetsModulesCorporate = bankDocumentsRepository.GetCorporateDocumentsModules().Where(x => x.CorporateAccount_BankDocument_PersonRole.ToString() == personRole && x.CorporateAccount_BankDocument_PersonType.ToString() == personTypeValue);
                        List<string> documentlist = new List<string>(new string[] { });
                        foreach (var item in bankDoucumetsModulesCorporate)
                        {
                            if (!documentlist.Contains(item.CorporateAccount_BankDocument_BankDocumentType.ToString()))
                            {
                                string relatedPartyroles = item.CorporateAccount_BankDocument_EntityRole.ToString();
                                string relatedPartyrolesName = ValidationHelper.GetString(ServiceHelper.GetName(relatedPartyroles, Constants.Roles_Related_To_Entity), "");
                                if (roles.Contains(relatedPartyrolesName))
                                {
                                    var checkBankDocuments = bankDocuments.ToList().Where(i => i.BankDocuments_EntityType == item.CorporateAccount_BankDocument_Type && i.BankDocuments_DocumentType == item.CorporateAccount_BankDocument_BankDocumentType && Convert.ToString(i.BankDocuments_Entity) == personNodeGUID);
                                    if (checkBankDocuments.Count() == 0)
                                    {
                                        DocumentsViewModel documentsViewModel = new DocumentsViewModel();

                                        documentsViewModel.Entity = personNodeGUID;
                                        documentsViewModel.EntityType = ValidationHelper.GetString(item.CorporateAccount_BankDocument_Type, "");
                                        //documentsViewModel.EntityRole = ValidationHelper.GetString(item.CorporateAccount_BankDocument_PersonRole, "");
                                        documentsViewModel.DocumentType = ValidationHelper.GetString(item.CorporateAccount_BankDocument_BankDocumentType, "");

                                        SaveBankDocumentsModel(documentsViewModel, applicatioData, applicationNumber, "LEGAL ENTITY");
                                    }
                                }
                            }
                            documentlist.Add(item.CorporateAccount_BankDocument_BankDocumentType.ToString());
                        }

                        // }
                    }
                    //Commented by SUBRAT as Legal entity Related party document will not generate
                    // CompanyDetailsRelatedParty companyDetailsRelatedParty = CompanyDetailsRelatedPartyProvider.GetCompanyDetailsRelatedParty(new Guid(personNodeGUID), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    //if (companyDetailsRelatedParty != null)
                    //{
                    //    //Check if if Person is Related to UBO?
                    //    if (companyDetailsRelatedParty.CompanyDetails_IsRelatedPartyUBO == true)
                    //    {
                    //        bankDoucumetsModulesCorporate = bankDocumentsRepository.GetCorporateDocumentsModules().Where(x => x.CorporateAccount_BankDocument_PersonRole.ToString() == personRole && x.CorporateAccount_BankDocument_Is_UBO == true);
                    //        foreach (var item in bankDoucumetsModulesCorporate)
                    //        {
                    //            var checkBankDocuments = bankDocuments.ToList().Where(i => i.BankDocuments_EntityType == item.CorporateAccount_BankDocument_Type && i.BankDocuments_DocumentType == item.CorporateAccount_BankDocument_BankDocumentType && Convert.ToString(i.BankDocuments_Entity) == personNodeGUID);
                    //            if (checkBankDocuments.Count() == 0)
                    //            {
                    //                DocumentsViewModel documentsViewModel = new DocumentsViewModel();

                    //                documentsViewModel.Entity = personNodeGUID;
                    //                documentsViewModel.EntityType = ValidationHelper.GetString(item.CorporateAccount_BankDocument_Type, "");
                    //                //documentsViewModel.EntityRole = ValidationHelper.GetString(item.CorporateAccount_BankDocument_PersonRole, "");
                    //                documentsViewModel.DocumentType = ValidationHelper.GetString(item.CorporateAccount_BankDocument_BankDocumentType, "");

                    //                SaveBankDocumentsModel(documentsViewModel, applicatioData, applicationNumber, "LEGAL ENTITY");
                    //            }
                    //        }
                    //    }
                    //    else if (companyDetailsRelatedParty.CompanyDetails_EntityType != null)
                    //    {
                    //        //Get Document as per the Entity type of Person
                    //        string entitySubTypeValue = CommonProcess.GetCompanyEntitySubTypeName(companyDetailsRelatedParty.CompanyDetails_EntityType);
                    //        bankDoucumetsModulesCorporate = bankDocumentsRepository.GetCorporateDocumentsModules().Where(x => x.CorporateAccount_BankDocument_PersonRole.ToString() == personRole && x.CorporateAccount_BankDocument_SubType.ToString() == entitySubTypeValue);
                    //        foreach (var item in bankDoucumetsModulesCorporate)
                    //        {
                    //            var checkBankDocuments = bankDocuments.ToList().Where(i => i.BankDocuments_EntityType == item.CorporateAccount_BankDocument_Type && i.BankDocuments_DocumentType == item.CorporateAccount_BankDocument_BankDocumentType && Convert.ToString(i.BankDocuments_Entity) == personNodeGUID);
                    //            if (checkBankDocuments.Count() == 0)
                    //            {
                    //                DocumentsViewModel documentsViewModel = new DocumentsViewModel();

                    //                documentsViewModel.Entity = personNodeGUID;
                    //                documentsViewModel.EntityType = ValidationHelper.GetString(item.CorporateAccount_BankDocument_Type, "");
                    //                //documentsViewModel.EntityRole = ValidationHelper.GetString(item.CorporateAccount_BankDocument_PersonRole, "");
                    //                documentsViewModel.DocumentType = ValidationHelper.GetString(item.CorporateAccount_BankDocument_BankDocumentType, "");

                    //                SaveBankDocumentsModel(documentsViewModel, applicatioData, applicationNumber, "LEGAL ENTITY");
                    //            }
                    //        }

                    //    }

                    //}
                }
                else
                {
                    //APPLICANT
                    string jurisdictionValue = string.Empty;
                    string entitySubTypeValue = string.Empty;
                    CompanyDetails companyDetails = CompanyDetailsProvider.GetCompanyDetails(new Guid(personNodeGUID), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName);
                    jurisdictionValue = CommonProcess.GetJurisdictionValue(companyDetails.CompanyDetails_CountryofIncorporation);
                    if (string.IsNullOrEmpty(jurisdictionValue))
                    {
                        jurisdictionValue = CommonProcess.GetOtherJurisdictionValue();
                    }
                    entitySubTypeValue = CommonProcess.GetEntitySubTypeValue(companyDetails.CompanyDetails_EntityType);
                    bankDoucumetsModulesCorporate = bankDocumentsRepository.GetCorporateDocumentsModules().Where(x => x.CorporateAccount_BankDocument_PersonRole.ToString() == personRole && ((x.CorporateAccount_BankDocument_SubType.ToString() == entitySubTypeValue || x.CorporateAccount_BankDocument_SubType.ToString() == "00000000-0000-0000-0000-000000000000") && (x.CorporateAccount_BankDocument_Jurisdiction.ToString() == jurisdictionValue || x.CorporateAccount_BankDocument_Jurisdiction.ToString() == "00000000-0000-0000-0000-000000000000")));
                    List<string> documentlist = new List<string>(new string[] { });
                    foreach (var item in bankDoucumetsModulesCorporate)
                    {
                        if (!documentlist.Contains(item.CorporateAccount_BankDocument_BankDocumentType.ToString()))
                        {
                            var checkBankDocuments = bankDocuments.ToList().Where(i => i.BankDocuments_EntityType == item.CorporateAccount_BankDocument_Type && i.BankDocuments_DocumentType == item.CorporateAccount_BankDocument_BankDocumentType && Convert.ToString(i.BankDocuments_Entity) == personNodeGUID);
                            if (checkBankDocuments.Count() == 0)
                            {
                                DocumentsViewModel documentsViewModel = new DocumentsViewModel();

                                documentsViewModel.Entity = personNodeGUID;
                                documentsViewModel.EntityType = ValidationHelper.GetString(item.CorporateAccount_BankDocument_Type, "");
                                //documentsViewModel.EntityRole = ValidationHelper.GetString(item.CorporateAccount_BankDocument_PersonRole, "");
                                documentsViewModel.DocumentType = ValidationHelper.GetString(item.CorporateAccount_BankDocument_BankDocumentType, "");

                                SaveBankDocumentsModel(documentsViewModel, applicatioData, applicationNumber, "LEGAL ENTITY");
                            }
                        }
                        documentlist.Add(item.CorporateAccount_BankDocument_BankDocumentType.ToString());
                    }
                }

            }
            else
            {
                //Checking Person is Applicant OR Related Party
                string documetTypeValue = CommonProcess.GetApplicationTypeForPersonalAndJoinAccount(applicationType);
                string personRole = CommonProcess.GetEntityRoleType(personNodeGUID);
                string personRoleName = ValidationHelper.GetString(ServiceHelper.GetName(personRole, Constants.PERSON_ROLE), "");
                if (string.Equals(personRoleName, "RELATED PARTY", StringComparison.OrdinalIgnoreCase))
                {
                    var roles = CommonProcess.GetEntitySeletedRolesIndividual(personNodeGUID);

                    bankDoucumetsModulesIndividual = bankDocumentsRepository.GetBankDocumentsModules().Where(x => x.PersonalAndJointAccount_BankDocument_PersonRole.ToString() == personRole);
                    List<string> documentlist = new List<string>(new string[] { });
                    foreach (var item in bankDoucumetsModulesIndividual)
                    {
                        if (!documentlist.Contains(item.PersonalAndJointAccount_BankDocument_BankDocumentType.ToString()))
                        {
                            string relatedPartyroles = item.PersonalAndJointAccount_BankDocument_Type.ToString();
                            string relatedPartyrolesName = ValidationHelper.GetString(ServiceHelper.GetName(relatedPartyroles, Constants.Entity_TYPE), "");
                            if (roles.Contains(relatedPartyrolesName))
                            {
                                var checkBankDocuments = bankDocuments.ToList().Where(i => i.BankDocuments_EntityType == item.PersonalAndJointAccount_BankDocument_Type && i.BankDocuments_DocumentType == item.PersonalAndJointAccount_BankDocument_BankDocumentType && Convert.ToString(i.BankDocuments_Entity) == personNodeGUID);
                                if (checkBankDocuments.Count() == 0)
                                {
                                    DocumentsViewModel documentsViewModel = new DocumentsViewModel();

                                    documentsViewModel.Entity = personNodeGUID;
                                    documentsViewModel.EntityType = ValidationHelper.GetString(item.PersonalAndJointAccount_BankDocument_Type, "");
                                    // documentsViewModel.EntityRole = ValidationHelper.GetString(item.PersonalAndJointAccount_BankDocument_PersonRole, "");
                                    documentsViewModel.DocumentType = ValidationHelper.GetString(item.PersonalAndJointAccount_BankDocument_BankDocumentType, "");
                                    SaveBankDocumentsModel(documentsViewModel, applicatioData, applicationNumber, "INDIVIDUAL");
                                }
                            }
                        }
                        documentlist.Add(item.PersonalAndJointAccount_BankDocument_BankDocumentType.ToString());
                    }
                }
                else
                {
                    bankDoucumetsModulesIndividual = bankDocumentsRepository.GetBankDocumentsModules().Where(x => x.PersonalAndJointAccount_BankDocument_PersonRole.ToString() == personRole && x.PersonalAndJointAccount_BankDocument_Type.ToString() == documetTypeValue);
                    List<string> documentlist = new List<string>(new string[] { });
                    foreach (var item in bankDoucumetsModulesIndividual)
                    {
                        if (!documentlist.Contains(item.PersonalAndJointAccount_BankDocument_BankDocumentType.ToString()))
                        {
                            var checkBankDocuments = bankDocuments.ToList().Where(i => i.BankDocuments_EntityType == item.PersonalAndJointAccount_BankDocument_Type && i.BankDocuments_DocumentType == item.PersonalAndJointAccount_BankDocument_BankDocumentType && Convert.ToString(i.BankDocuments_Entity) == personNodeGUID);
                            if (checkBankDocuments.Count() == 0)
                            {
                                DocumentsViewModel documentsViewModel = new DocumentsViewModel();

                                documentsViewModel.Entity = personNodeGUID;
                                documentsViewModel.EntityType = ValidationHelper.GetString(item.PersonalAndJointAccount_BankDocument_Type, "");
                                // documentsViewModel.EntityRole = ValidationHelper.GetString(item.PersonalAndJointAccount_BankDocument_PersonRole, "");
                                documentsViewModel.DocumentType = ValidationHelper.GetString(item.PersonalAndJointAccount_BankDocument_BankDocumentType, "");
                                SaveBankDocumentsModel(documentsViewModel, applicatioData, applicationNumber, "INDIVIDUAL");
                            }
                        }
                        documentlist.Add(item.PersonalAndJointAccount_BankDocument_BankDocumentType.ToString());
                    }

                }


            }

            return true;
        }

        public static bool GenerateExpectedDouments(int id, ExpectedDocumentsRepository expectedDocumentsRepository, ApplicationsRepository applicationsRepository, string applicationNumber, string applicationType, string personNodeGUID)
        {
            //Delete documents
            var expectedDocument = expectedDocumentsRepository.GetExpectedDocuments(id).Where(x => x.ExpectedDocuments_Entity.ToString() == personNodeGUID).ToList();
            if (expectedDocument != null && expectedDocument.Count > 0)
            {
                foreach (var item in expectedDocument)
                {
                    item.Delete();
                }
            }
            //------------------------------

            applicationType = ValidationHelper.GetString(ServiceHelper.GetEntityType(applicationType, Constants.APPLICATION_TYPE), "");
            bool isLegalEntity = string.Equals(applicationType, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase);
            int applicationID = ValidationHelper.GetInteger(id, 0);
            var ExpectedDocuments = expectedDocumentsRepository.GetExpectedDocuments(applicationID);
            if (isLegalEntity)
            {
                string personRole = CommonProcess.GetEntityRoleTypeLegal(personNodeGUID);
                string personRoleName = ValidationHelper.GetString(ServiceHelper.GetName(personRole, Constants.LEGALPERSON_ROLE), "");
                string jurisdictionValue = string.Empty;
                string entitySubTypeValue = string.Empty;
                int personalDetailsID = 0;
                bool IsRelatedPartyUBO = false;
                if (string.Equals(personRoleName, "APPLICANT", StringComparison.OrdinalIgnoreCase))
                {
                    int CompanyDetailsID = CommonProcess.GetCompanyDetailsIdLegalApplicant(personNodeGUID);
                    var companyDetails = CompanyDetailsProcess.GetCompanyDetailsModelById(CompanyDetailsID);
                    jurisdictionValue = CommonProcess.GetJurisdictionValue(companyDetails.CountryofIncorporation);
                    if (string.IsNullOrEmpty(jurisdictionValue))
                    {
                        jurisdictionValue = CommonProcess.GetOtherJurisdictionValue();
                    }
                    entitySubTypeValue = CommonProcess.GetEntitySubTypeValue(companyDetails.EntityType);
                }
                else
                {
                    personalDetailsID = CommonProcess.GetPersonalDetailsLegalRelatedParty(personNodeGUID);
                    //For Legal Individual Related party
                    if (personalDetailsID != 0)
                    {
                        var personalDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(personalDetailsID);
                        IsRelatedPartyUBO = personalDetails.IsRelatedPartyUBO;
                    }
                    if (personalDetailsID == 0)
                    {
                        int CompanyDetailsID = CommonProcess.GetCompanyDetailsIdLegalRelatedParty(personNodeGUID);
                        var companyDetails = CompanyDetailsRelatedPartyProcess.GetCompanyDetailsRelatedPartyModelById(CompanyDetailsID);
                        jurisdictionValue = CommonProcess.GetJurisdictionValue(companyDetails.CountryofIncorporation);
                        if (string.IsNullOrEmpty(jurisdictionValue))
                        {
                            jurisdictionValue = CommonProcess.GetOtherJurisdictionValue();
                        }
                        entitySubTypeValue = CommonProcess.GetEntitySubTypeValue(companyDetails.EntityType);
                    }
                }
                // Get Corporate expected documents
                if (personalDetailsID != 0)
                {
                    //if (IsRelatedPartyUBO)
                    //{
                    //    string documentTypeValue = CommonProcess.GetCorporateDocumentTypeValue("ULTIMATE BENEFICIAL OWNER");
                    //    var expectedDoucumetsModules = expectedDocumentsRepository.GetExpectedCorporateDocumentsModules().Where(x => x.CorporateAccount_ExpectedDocument_PersonRole.ToString() == personRole && x.CorporateAccount_ExpectedDocument_Type.ToString() == documentTypeValue);
                    //    foreach (var item in expectedDoucumetsModules)
                    //    {
                    //        var checkBankDocuments = ExpectedDocuments.ToList().Where(i => i.ExpectedDocuments_DocumentType == item.CorporateAccount_ExpectedDocument_ExpectedDocumentType && Convert.ToString(i.ExpectedDocuments_Entity) == personNodeGUID);
                    //        if (checkBankDocuments.Count() == 0)
                    //        {
                    //            DocumentsViewModel documentsViewModel = new DocumentsViewModel();
                    //            var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                    //            documentsViewModel.Entity = personNodeGUID;
                    //            documentsViewModel.EntityType = ValidationHelper.GetString(item.CorporateAccount_ExpectedDocument_Type, "");
                    //            //documentsViewModel.EntityRole = ValidationHelper.GetString(item.CorporateAccount_ExpectedDocument_PersonRole, "");
                    //            documentsViewModel.DocumentType = ValidationHelper.GetString(item.CorporateAccount_ExpectedDocument_ExpectedDocumentType, "");

                    //            SaveExpectedDocumentsModel(documentsViewModel, applicatioData, applicationNumber, "LEGAL ENTITY");
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //*************Subrat added Mandatory document for Legal Entity Individual Related Party*******************
                    var expectedDoucumetsMandatoryDoc = expectedDocumentsRepository.GetExpectedCorporateDocumentsModules().Where(x => x.CorporateAccount_ExpectedDocument_PersonRole.ToString() == personRole && x.CorporateAccount_ExpectedDocument_RolesRelatedToEntity.ToString() == Constants.BlankGuid && x.CorporateAccount_ExpectedDocument_Jurisdiction.ToString() == Constants.BlankGuid && x.CorporateAccount_ExpectedDocument_Type.ToString() == Constants.BlankGuid && x.CorporateAccount_ExpectedDocument_SubType.ToString() == Constants.BlankGuid && x.CorporateAccount_ExpectedDocument_PersonType.ToString() == Constants.PersonTypeGuidIndividual);
                    foreach (var item in expectedDoucumetsMandatoryDoc)
                    {
                        var checkBankDocuments = ExpectedDocuments.ToList().Where(i => i.ExpectedDocuments_DocumentType == item.CorporateAccount_ExpectedDocument_ExpectedDocumentType && Convert.ToString(i.ExpectedDocuments_Entity) == personNodeGUID);
                        if (checkBankDocuments.Count() == 0)
                        {
                            DocumentsViewModel documentsViewModel = new DocumentsViewModel();
                            var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                            documentsViewModel.Entity = personNodeGUID;
                            documentsViewModel.EntityType = ValidationHelper.GetString(item.CorporateAccount_ExpectedDocument_Jurisdiction, "");
                            documentsViewModel.DocumentType = ValidationHelper.GetString(item.CorporateAccount_ExpectedDocument_ExpectedDocumentType, "");
                            SaveExpectedDocumentsModel(documentsViewModel, applicatioData, applicationNumber, "LEGAL ENTITY");
                        }
                    }
                    //**************************END***************************
                    var roles = CommonProcess.GetEntitySeletedRolesLegal(personNodeGUID, IsRelatedPartyUBO);
                    var expectedDoucumetsModules = expectedDocumentsRepository.GetExpectedCorporateDocumentsModules().Where(x => x.CorporateAccount_ExpectedDocument_PersonRole.ToString() == personRole);
                    List<string> documentlist = new List<string>(new string[] { });
                    foreach (var item in expectedDoucumetsModules)
                    {
                        if (!documentlist.Contains(item.CorporateAccount_ExpectedDocument_ExpectedDocumentType.ToString()))
                        {
                            string relatedPartyroles = item.CorporateAccount_ExpectedDocument_RolesRelatedToEntity.ToString();
                            string relatedPartyrolesName = ValidationHelper.GetString(ServiceHelper.GetName(relatedPartyroles, Constants.Roles_Related_To_Entity), "");
                            if (roles.Contains(relatedPartyrolesName))
                            {
                                var checkBankDocuments = ExpectedDocuments.ToList().Where(i => i.ExpectedDocuments_DocumentType == item.CorporateAccount_ExpectedDocument_ExpectedDocumentType && Convert.ToString(i.ExpectedDocuments_Entity) == personNodeGUID);
                                if (checkBankDocuments.Count() == 0)
                                {
                                    DocumentsViewModel documentsViewModel = new DocumentsViewModel();
                                    var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                                    documentsViewModel.Entity = personNodeGUID;
                                    documentsViewModel.EntityType = ValidationHelper.GetString(item.CorporateAccount_ExpectedDocument_Jurisdiction, "");
                                    //documentsViewModel.EntityRole = ValidationHelper.GetString(item.CorporateAccount_ExpectedDocument_PersonRole, "");
                                    documentsViewModel.DocumentType = ValidationHelper.GetString(item.CorporateAccount_ExpectedDocument_ExpectedDocumentType, "");

                                    SaveExpectedDocumentsModel(documentsViewModel, applicatioData, applicationNumber, "LEGAL ENTITY");
                                }
                            }
                        }
                        documentlist.Add(item.CorporateAccount_ExpectedDocument_ExpectedDocumentType.ToString());
                    }
                    //}

                }
                else
                {
                    var expectedDoucumetsModules = expectedDocumentsRepository.GetExpectedCorporateDocumentsModules().Where(x => x.CorporateAccount_ExpectedDocument_PersonRole.ToString() == personRole && (x.CorporateAccount_ExpectedDocument_Jurisdiction.ToString() == jurisdictionValue || x.CorporateAccount_ExpectedDocument_SubType.ToString() == entitySubTypeValue));
                    List<string> documentlist = new List<string>(new string[] { });
                    foreach (var item in expectedDoucumetsModules)
                    {
                        if (!documentlist.Contains(item.CorporateAccount_ExpectedDocument_ExpectedDocumentType.ToString()))
                        {
                            var checkBankDocuments = ExpectedDocuments.ToList().Where(i => i.ExpectedDocuments_DocumentType == item.CorporateAccount_ExpectedDocument_ExpectedDocumentType && Convert.ToString(i.ExpectedDocuments_Entity) == personNodeGUID);
                            if (checkBankDocuments.Count() == 0)
                            {
                                DocumentsViewModel documentsViewModel = new DocumentsViewModel();
                                var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                                documentsViewModel.Entity = personNodeGUID;
                                documentsViewModel.EntityType = ValidationHelper.GetString(item.CorporateAccount_ExpectedDocument_Jurisdiction, "");
                                //documentsViewModel.EntityRole = ValidationHelper.GetString(item.CorporateAccount_ExpectedDocument_PersonRole, "");
                                documentsViewModel.DocumentType = ValidationHelper.GetString(item.CorporateAccount_ExpectedDocument_ExpectedDocumentType, "");

                                SaveExpectedDocumentsModel(documentsViewModel, applicatioData, applicationNumber, "LEGAL ENTITY");
                            }
                        }
                        documentlist.Add(item.CorporateAccount_ExpectedDocument_ExpectedDocumentType.ToString());
                    }
                }
            }
            else
            {
                string documetTypeValue = CommonProcess.GetApplicationTypeForPersonalAndJoinAccount(applicationType);
                string personRole = CommonProcess.GetEntityRoleType(personNodeGUID);
                string personRoleName = ValidationHelper.GetString(ServiceHelper.GetName(personRole, Constants.PERSON_ROLE), "");
                if (string.Equals(personRoleName, "RELATED PARTY", StringComparison.OrdinalIgnoreCase))
                {
                    List<string> documentlist = new List<string>(new string[] { });
                    var roles = CommonProcess.GetEntitySeletedRolesIndividual(personNodeGUID);
                    var expectedDoucumetsModules = expectedDocumentsRepository.GetExpectedDocumentsModules().Where(x => x.PersonalAndJointAccount_ExpectedDocument_PersonRole.ToString() == personRole);
                    foreach (var item in expectedDoucumetsModules)
                    {
                        if (!documentlist.Contains(item.PersonalAndJointAccount_ExpectedDocument_ExpectedDocumentType.ToString()))
                        {
                            string relatedPartyroles = item.PersonalAndJointAccount_ExpectedDocument_Type.ToString();
                            string relatedPartyrolesName = ValidationHelper.GetString(ServiceHelper.GetName(relatedPartyroles, Constants.Entity_TYPE), "");
                            if (roles.Contains(relatedPartyrolesName))
                            {
                                var checkBankDocuments = ExpectedDocuments.ToList().Where(i => i.ExpectedDocuments_EntityType == item.PersonalAndJointAccount_ExpectedDocument_Type && i.ExpectedDocuments_DocumentType == item.PersonalAndJointAccount_ExpectedDocument_ExpectedDocumentType && Convert.ToString(i.ExpectedDocuments_Entity) == personNodeGUID);
                                if (checkBankDocuments.Count() == 0)
                                {
                                    DocumentsViewModel documentsViewModel = new DocumentsViewModel();
                                    var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                                    documentsViewModel.Entity = personNodeGUID;
                                    documentsViewModel.EntityType = ValidationHelper.GetString(item.PersonalAndJointAccount_ExpectedDocument_Type, "");
                                    //documentsViewModel.EntityRole = ValidationHelper.GetString(item.PersonalAndJointAccount_ExpectedDocument_PersonRole, "");
                                    documentsViewModel.DocumentType = ValidationHelper.GetString(item.PersonalAndJointAccount_ExpectedDocument_ExpectedDocumentType, "");
                                    SaveExpectedDocumentsModel(documentsViewModel, applicatioData, applicationNumber, "INDIVIDUAL");
                                }
                            }
                        }
                        documentlist.Add(item.PersonalAndJointAccount_ExpectedDocument_ExpectedDocumentType.ToString());
                    }
                }
                else
                {
                    // Get individual expected documents
                    var expectedDoucumetsModules = expectedDocumentsRepository.GetExpectedDocumentsModules().Where(x => x.PersonalAndJointAccount_ExpectedDocument_PersonRole.ToString() == personRole && x.PersonalAndJointAccount_ExpectedDocument_Type.ToString() == documetTypeValue);
                    List<string> documentlist = new List<string>(new string[] { });
                    foreach (var item in expectedDoucumetsModules)
                    {
                        if (!documentlist.Contains(item.PersonalAndJointAccount_ExpectedDocument_ExpectedDocumentType.ToString()))
                        {
                            var checkBankDocuments = ExpectedDocuments.ToList().Where(i => i.ExpectedDocuments_EntityType == item.PersonalAndJointAccount_ExpectedDocument_Type && i.ExpectedDocuments_DocumentType == item.PersonalAndJointAccount_ExpectedDocument_ExpectedDocumentType && Convert.ToString(i.ExpectedDocuments_Entity) == personNodeGUID);
                            if (checkBankDocuments.Count() == 0)
                            {
                                DocumentsViewModel documentsViewModel = new DocumentsViewModel();
                                var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                                documentsViewModel.Entity = personNodeGUID;
                                documentsViewModel.EntityType = ValidationHelper.GetString(item.PersonalAndJointAccount_ExpectedDocument_Type, "");
                                //documentsViewModel.EntityRole = ValidationHelper.GetString(item.PersonalAndJointAccount_ExpectedDocument_PersonRole, "");
                                documentsViewModel.DocumentType = ValidationHelper.GetString(item.PersonalAndJointAccount_ExpectedDocument_ExpectedDocumentType, "");
                                SaveExpectedDocumentsModel(documentsViewModel, applicatioData, applicationNumber, "INDIVIDUAL");
                            }
                        }
                        documentlist.Add(item.PersonalAndJointAccount_ExpectedDocument_ExpectedDocumentType.ToString());
                    }
                }

            }
            return true;
        }

    }
}

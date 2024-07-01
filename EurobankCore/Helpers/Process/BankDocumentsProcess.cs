using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class BankDocumentsProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";
        private static readonly string _BankDocumentsFolderName = "Documents";
        private static readonly string _BankDocumentsDetailsFolderName = "Bank Documents";
        public static List<DocumentsViewModel> GetBankDocumentsDetailsByApplicationID(int applicationID)
        {
            List<DocumentsViewModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (applicationID > 0)
            {
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(ApplicationDetails.CLASS_NAME)
                    .WhereEquals("ApplicationDetailsID", applicationID)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode bankdocumentsDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BankDocumentsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        applicationDetailsNode=applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BankDocumentsFolderName, StringComparison.OrdinalIgnoreCase));
                        bankdocumentsDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BankDocumentsDetailsFolderName, StringComparison.OrdinalIgnoreCase));

                        if (bankdocumentsDetailsRoot != null)
                        {
                            List<TreeNode> bankDocumentsDetailsNodes = bankdocumentsDetailsRoot.Children.Where(u => u.ClassName == BankDocuments.CLASS_NAME).ToList();

                            if (bankDocumentsDetailsNodes != null && bankDocumentsDetailsNodes.Count > 0)
                            {
                                retVal = new List<DocumentsViewModel>();
                                bankDocumentsDetailsNodes.ForEach(t =>
                                {
                                    BankDocuments bankDocuments = BankDocumentsProvider.GetBankDocuments(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (bankDocuments != null)
                                    {
                                        DocumentsViewModel documentsViewModel = BindDocumentsViewModel(bankDocuments);
                                        if (documentsViewModel != null)
                                        {
                                            retVal.Add(documentsViewModel);
                                        }
                                    }
                                });
                            }
                        }

                    }
                }
            }

            return retVal;
        }
        private static DocumentsViewModel BindDocumentsViewModel(BankDocuments item)
        {
            DocumentsViewModel retVal = null;

            if (item != null)
            {
                var entity = ServiceHelper.GetEntity();
                var entityType = ServiceHelper.GetEntityType();
                var entityRole = ServiceHelper.GetPERSON_ROLE();
                var documentsType = ServiceHelper.GetDocumentsType();
                retVal = new DocumentsViewModel()
                {
                    DocId = item.BankDocumentsID,
                    Entity = (entity != null && entity.Count > 0 && item.BankDocuments_Entity != null) ? entity.FirstOrDefault(f => f.Value == item.BankDocuments_Entity.ToString()).Text : string.Empty,
                    EntityType = (entityType != null && entityType.Count > 0 && item.BankDocuments_EntityType != null) ? entityType.FirstOrDefault(f => f.Value == item.BankDocuments_EntityType.ToString()).Text : string.Empty,
                    EntityRole = (entityRole != null && entityRole.Count > 0 && item.BankDocuments_EntityRole != null) ? entityRole.FirstOrDefault(f => f.Value == item.BankDocuments_EntityRole.ToString()).Text : string.Empty,
                    DocumentType = (documentsType != null && documentsType.Count > 0 && item.BankDocuments_DocumentType != null) ? documentsType.FirstOrDefault(f => f.Value == item.BankDocuments_DocumentType.ToString()).Text : string.Empty,
                    RequiresSignature = item.BankDocuments_RequiresSignature,
                    BankDocuments_Status = item.BankDocuments_Status,
                    BankDocuments_Status_Name = item.BankDocuments_Status == true ? "COMPLETE" : "PENDING",
                };
            }
            return retVal;
        }
    }
}

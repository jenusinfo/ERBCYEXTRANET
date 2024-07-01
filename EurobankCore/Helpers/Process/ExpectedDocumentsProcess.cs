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
    public class ExpectedDocumentsProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";
        private static readonly string _ExpectedDocumentsFolderName = "Documents";
        private static readonly string _ExpectedDocumentsDetailsFolderName = "Expected Documents";
        public static List<DocumentsViewModel> GetExpectedDocumentsDetailsByApplicationID(int applicationID)
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
                    TreeNode expectedDocumentsDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ExpectedDocumentsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        applicationDetailsNode=applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ExpectedDocumentsFolderName, StringComparison.OrdinalIgnoreCase));
                        expectedDocumentsDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ExpectedDocumentsDetailsFolderName, StringComparison.OrdinalIgnoreCase));

                        if (expectedDocumentsDetailsRoot != null)
                        {
                            List<TreeNode> expectedDocumentsDetailsNodes = expectedDocumentsDetailsRoot.Children.Where(u => u.ClassName == ExpectedDocuments.CLASS_NAME).ToList();

                            if (expectedDocumentsDetailsNodes != null && expectedDocumentsDetailsNodes.Count > 0)
                            {
                                retVal = new List<DocumentsViewModel>();
                                expectedDocumentsDetailsNodes.ForEach(t =>
                                {
                                    ExpectedDocuments expectedDocuments = ExpectedDocumentsProvider.GetExpectedDocuments(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (expectedDocuments != null)
                                    {
                                        DocumentsViewModel documentsViewModel = BindDocumentsViewModel(expectedDocuments);
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
        private static DocumentsViewModel BindDocumentsViewModel(ExpectedDocuments item)
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
                    DocId = item.ExpectedDocumentsID,
                    Entity = (entity != null && entity.Count > 0 && item.ExpectedDocuments_Entity != null) ? entity.FirstOrDefault(f => f.Value == item.ExpectedDocuments_Entity.ToString()).Text : string.Empty,
                    EntityType = (entityType != null && entityType.Count > 0 && item.ExpectedDocuments_EntityType != null) ? entityType.FirstOrDefault(f => f.Value == item.ExpectedDocuments_EntityType.ToString()).Text : string.Empty,
                    EntityRole = (entityRole != null && entityRole.Count > 0 && item.ExpectedDocuments_EntityRole != null) ? entityRole.FirstOrDefault(f => f.Value == item.ExpectedDocuments_EntityRole.ToString()).Text : string.Empty,
                    //DocumentType = (documentsType != null && documentsType.Count > 0 && item.ExpectedDocuments_DocumentType != null) ? documentsType.FirstOrDefault(f => f.Value == item.ExpectedDocuments_DocumentType.ToString()).Text : string.Empty,
                    RequiresSignature = item.ExpectedDocuments_RequiresSignature,
                   
                };
            }
            return retVal;
        }
    }
}

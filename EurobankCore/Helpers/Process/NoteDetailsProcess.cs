using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class NoteDetailsProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

        private static readonly string _NoteDetailsFolderName = "Notes";
        private static readonly string _NoteDetailsDocumentName = "Note";

        public static List<NoteDetailsModel> GetNoteDetailsModels(string applicationNumber)
        {
            List<NoteDetailsModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (!string.IsNullOrEmpty(applicationNumber))
            {
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type("Eurobank.ApplicationDetails")
                    .WhereEquals("NodeName", applicationNumber)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    retVal = new List<NoteDetailsModel>();
                    TreeNode noteDetailsFolderRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _NoteDetailsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        noteDetailsFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _NoteDetailsFolderName, StringComparison.OrdinalIgnoreCase));

                        if (noteDetailsFolderRoot != null)
                        {
                            List<TreeNode> noteDetailsNodes = noteDetailsFolderRoot.Children.Where(u => u.ClassName == NoteDetails.CLASS_NAME).ToList();

                            if (noteDetailsNodes != null && noteDetailsNodes.Count > 0)
                            {
                                noteDetailsNodes.ForEach(t =>
                                {
                                    NoteDetails noteDetails = NoteDetailsProvider.GetNoteDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (noteDetails != null)
                                    {
                                        NoteDetailsModel noteDetailsModel = BindNoteDetailsModel(noteDetails);
                                        if (noteDetailsModel != null)
                                        {
                                            retVal.Add(noteDetailsModel);
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

        public static NoteDetailsModel SaveNoteDetailsModel(string applicationNumber, NoteDetailsModel model)
        {
            NoteDetailsModel retVal = null;

            if (model != null)
            {
                var subjects = ServiceHelper.GetDocumentSubjects();
                if (subjects != null && subjects.Any(t => !string.IsNullOrEmpty(model.Subject) && t.Value == model.Subject))
                {
                    model.SubjectName = subjects.FirstOrDefault(t => !string.IsNullOrEmpty(model.Subject) && t.Value == model.Subject).Text;
                }
            }
            if (model != null && model.Id > 0)
            {
                NoteDetails noteDetails = GetNoteDetailsById(model.Id);
                if (noteDetails != null)
                {
                    NoteDetails updatedNoteDetails = BindNoteDetails(noteDetails, model);
                    if (updatedNoteDetails != null)
                    {
                        updatedNoteDetails.Update();
                        retVal = BindNoteDetailsModel(updatedNoteDetails);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(applicationNumber) && model != null)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type("Eurobank.ApplicationDetails")
                    .WhereEquals("NodeName", applicationNumber)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode noteDetailsFolderRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _NoteDetailsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        noteDetailsFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _NoteDetailsFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        noteDetailsFolderRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        noteDetailsFolderRoot.DocumentName = _NoteDetailsFolderName;
                        noteDetailsFolderRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        noteDetailsFolderRoot.Insert(applicationDetailsNode);
                    }
                    NoteDetails noteDetails = BindNoteDetails(null, model);
                    if (noteDetails != null && noteDetailsFolderRoot != null)
                    {
                        //noteDetails.DocumentName = model.SubjectName;
                        //noteDetails.NodeName = model.SubjectName;
                        noteDetails.DocumentName = _NoteDetailsDocumentName;
                        noteDetails.NodeName = _NoteDetailsDocumentName;
                        noteDetails.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        noteDetails.Insert(noteDetailsFolderRoot);
                        model = BindNoteDetailsModel(noteDetails);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        private static NoteDetails GetNoteDetailsById(int noteDetailsId)
        {
            NoteDetails retVal = null;

            if (noteDetailsId > 0)
            {
                var noteDetails = NoteDetailsProvider.GetNoteDetails();
                if (noteDetails != null && noteDetails.Count > 0)
                {
                    retVal = noteDetails.FirstOrDefault(o => o.NoteDetailsID == noteDetailsId);
                }
            }

            return retVal;
        }

        private static NoteDetailsModel BindNoteDetailsModel(NoteDetails item)
        {
            NoteDetailsModel retVal = null;

            if (item != null)
            {
                var documentTypes = ServiceHelper.GetDocumentTypes();
                var pendingOnUsers = ServiceHelper.GetNoteDetailPendingOnUsers();
                //var subjects = ServiceHelper.GetDocumentSubjects();
                retVal = new NoteDetailsModel()
                {
                    Id = item.NoteDetailsID,
                    NoteDetailsType = item.NoteDetails_Type.ToString(),
                    NoteDetailsTypeName = (documentTypes != null && documentTypes.Any(l => l.Value == item.NoteDetails_Type.ToString())) ? documentTypes.FirstOrDefault(l => l.Value == item.NoteDetails_Type.ToString()).Text : string.Empty,
                    Subject = item.NoteDetails_Subject,
                    SubjectName = item.NoteDetails_Subject, //(subjects != null && subjects.Any(l => l.Value == item.NoteDetails_Subject.ToString())) ? subjects.FirstOrDefault(l => l.Value == item.NoteDetails_Subject.ToString()).Text : string.Empty,
                    Details = item.NoteDetails_Details,
                    PendingOn = item.NoteDetails_PendingOn.ToString(),
                    PendingOnName = (pendingOnUsers != null && pendingOnUsers.Any(l => l.Value == item.NoteDetails_PendingOn.ToString())) ? pendingOnUsers.FirstOrDefault(l => l.Value == item.NoteDetails_PendingOn.ToString()).Text : string.Empty,
                    ExpectedDate = item.NoteDetails_ExpectedDate,
                    NoteDetails_Status = item.NoteDetails_Status,
                    Status_Name = item.NoteDetails_Status == true ? "Complete" : "Pending"
                };
            }

            return retVal;
        }

        private static NoteDetails BindNoteDetails(NoteDetails noteDetails, NoteDetailsModel item)
        {
            NoteDetails retVal = new NoteDetails();
            if (noteDetails != null)
            {
                retVal = noteDetails;
            }
            if (item != null)
            {
				if(!string.IsNullOrEmpty(item.NoteDetailsType))
				{
                    retVal.NoteDetails_Type = new Guid(item.NoteDetailsType);
                }
                
                retVal.NoteDetails_Subject = item.Subject;
                retVal.NoteDetails_Details = item.Details;
                if (!string.IsNullOrEmpty(item.PendingOn))
                    retVal.NoteDetails_PendingOn = new Guid(item.PendingOn);
                retVal.NoteDetails_ExpectedDate =Convert.ToDateTime( item.ExpectedDate);
                retVal.NoteDetails_Status = item.NoteDetails_Status;
            }

            return retVal;
        }
    }
}

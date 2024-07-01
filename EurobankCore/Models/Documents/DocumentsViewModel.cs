using CMS.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Documents
{
	public class DocumentsViewModel
	{
		public int DocId { get; set; }
		//[Required(ErrorMessage = "Please select Entity Name.")]
		[Display(Name = "Entity Name")]
		public string Entity_Name { get; set; }
		public string Entity { get; set; }

		//[Required(ErrorMessage = "Please select Entity Type.")]
		[Display(Name = "Entity Role")]
		public string EntityType_Name { get; set; }
		public string EntityType { get; set; }
		public string EntityType1 { get; set; }
		//[Required(ErrorMessage = "Please select Entity Role.")]
		[Display(Name = "Entity Type")]
		public string EntityRole_Name { get; set; }
		public string EntityRole { get; set; }
		public string EntityRole1 { get; set; }
		//[Required(ErrorMessage = "Please select Document Type.")]
		[Display(Name = "Document Type")]
		public string DocumentType_Name { get; set; }
		public string DocumentType { get; set; }
		public string FileName { get; set; }
		public string ExternalFileGuid { get; set; }
		public string UploadFileName { get; set; }
		[Display(Name = "Requires Signature")]
		public bool RequiresSignature { get; set; }
		[Display(Name = "Requires Signature")]
		public string RequiresSignatureStatus { get; set; }
		public string files { get; set; }
		[Display(Name = "File upload")]
		public string FileUpload { get; set; }
		public IFormFile FileUpload1 { get; set; }
		[Display(Name = "Confirm validity of document and signature where it applies")]
		public bool Consent { get; set; }
		[Display(Name = "Uploaded By")]
		public string uploadedBy { get; set; }
		[Display(Name = "Uploaded On")]
		public string uploadedOn { get; set; }
		[Display(Name = "Status")]
		public string Status { get; set; }
		public bool BankDocuments_Status { get; set; }
		public string BankDocuments_Status_Name { get; set; }

		public string EntityTypeSection { get; set; }
	}
	public class DocumentsViewModelErrorMessage
	{
		public static string Entity
		{
			get
			{
				return ResHelper.GetString("Eurobank.Documents.Error.Entity");
			}
		}
		public const string EntityTypeError = "Eurobank.Documents.Error.EntityType";
		public const string EntityRoleError = "Eurobank.Documents.Error.EntityRole";
		public const string DocumentTypeError = "Eurobank.Documents.Error.DocumentType";

	}
}

﻿//--------------------------------------------------------------------------------------------------
// <auto-generated>
//
//     This code was generated by code generator tool.
//
//     To customize the code use your own partial class. For more info about how to use and customize
//     the generated code see the documentation at https://docs.xperience.io/.
//
// </auto-generated>
//--------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using CMS;
using CMS.Base;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;

[assembly: RegisterDocumentType(RelatedPartyRolesLegal.CLASS_NAME, typeof(RelatedPartyRolesLegal))]

namespace CMS.DocumentEngine.Types.Eurobank
{
	/// <summary>
	/// Represents a content item of type RelatedPartyRolesLegal.
	/// </summary>
	public partial class RelatedPartyRolesLegal : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Eurobank.RelatedPartyRolesLegal";


		/// <summary>
		/// The instance of the class that provides extended API for working with RelatedPartyRolesLegal fields.
		/// </summary>
		private readonly RelatedPartyRolesLegalFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// RelatedPartyRolesID.
		/// </summary>
		[DatabaseIDField]
		public int RelatedPartyRolesLegalID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("RelatedPartyRolesLegalID"), 0);
			}
			set
			{
				SetValue("RelatedPartyRolesLegalID", value);
			}
		}


		/// <summary>
		/// Alternate Secretary.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsAlternateSecretery
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsAlternateSecretery"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsAlternateSecretery", value);
			}
		}


		/// <summary>
		/// Chairman Of The Board Of Directors.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsChairmanOfTheBoardOfDirector
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsChairmanOfTheBoardOfDirector"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsChairmanOfTheBoardOfDirector", value);
			}
		}


		/// <summary>
		/// Director.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsDirector
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsDirector"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsDirector", value);
			}
		}


		/// <summary>
		/// Vice-Chairman Of The Board Of Directors.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector", value);
			}
		}


		/// <summary>
		/// Secretary Of The Board Of Directors.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector", value);
			}
		}


		/// <summary>
		/// Treasurer Of Board Of Directors.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsTreasurerOfBoardOfDirectors
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsTreasurerOfBoardOfDirectors"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsTreasurerOfBoardOfDirectors", value);
			}
		}


		/// <summary>
		/// Member Of Board Of Directors.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsMemeberOfBoardOfDirectors
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsMemeberOfBoardOfDirectors"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsMemeberOfBoardOfDirectors", value);
			}
		}


		/// <summary>
		/// Partner.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsPartner
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsPartner"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsPartner", value);
			}
		}


		/// <summary>
		/// General Partner.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_GeneralPartner
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_GeneralPartner"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_GeneralPartner", value);
			}
		}


		/// <summary>
		/// Limited Partner.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_LimitedPartner
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_LimitedPartner"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_LimitedPartner", value);
			}
		}


		/// <summary>
		/// President Of Committee.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsPresidentOfCommittee
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsPresidentOfCommittee"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsPresidentOfCommittee", value);
			}
		}


		/// <summary>
		/// Vice-President Of Committee.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsVicePresidentOfCommittee
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsVicePresidentOfCommittee"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsVicePresidentOfCommittee", value);
			}
		}


		/// <summary>
		/// Secretary Of Committee.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsSecretaryOfCommittee
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsSecretaryOfCommittee"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsSecretaryOfCommittee", value);
			}
		}


		/// <summary>
		/// Treasurer Of Committee.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsTreasurerOfCommittee
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsTreasurerOfCommittee"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsTreasurerOfCommittee", value);
			}
		}


		/// <summary>
		/// Member Of Committee.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsMemeberOfCommittee
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsMemeberOfCommittee"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsMemeberOfCommittee", value);
			}
		}


		/// <summary>
		/// Trustee.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsTrustee
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsTrustee"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsTrustee", value);
			}
		}


		/// <summary>
		/// Settlor.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsSettlor
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsSettlor"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsSettlor", value);
			}
		}


		/// <summary>
		/// Protector.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsProtector
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsProtector"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsProtector", value);
			}
		}


		/// <summary>
		/// Benificiary.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsBenificiary
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsBenificiary"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsBenificiary", value);
			}
		}


		/// <summary>
		/// Founder.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsFounder
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsFounder"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsFounder", value);
			}
		}


		/// <summary>
		/// President Of Council.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsPresidentOfCouncil
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsPresidentOfCouncil"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsPresidentOfCouncil", value);
			}
		}


		/// <summary>
		/// Vice-President Of Council.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsVicePresidentOfCouncil
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsVicePresidentOfCouncil"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsVicePresidentOfCouncil", value);
			}
		}


		/// <summary>
		/// Secretary Of Council.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsSecretaryOfCouncil
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsSecretaryOfCouncil"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsSecretaryOfCouncil", value);
			}
		}


		/// <summary>
		/// Treasurer Of Council.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsTreasurerOfCouncil
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsTreasurerOfCouncil"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsTreasurerOfCouncil", value);
			}
		}


		/// <summary>
		/// Member Of Council.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsMemberOfCouncil
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsMemberOfCouncil"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsMemberOfCouncil", value);
			}
		}


		/// <summary>
		/// Fund MLCO.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsFundMlco
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsFundMlco"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsFundMlco", value);
			}
		}


		/// <summary>
		/// Fund Administrator.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsFundAdministrator
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsFundAdministrator"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsFundAdministrator", value);
			}
		}


		/// <summary>
		/// Management Company.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsManagementCompany
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsManagementCompany"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsManagementCompany", value);
			}
		}


		/// <summary>
		/// Holder Of Management Shares.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsHolderOfManagementShares
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsHolderOfManagementShares"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsHolderOfManagementShares", value);
			}
		}


		/// <summary>
		/// Alternative Director.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsAlternativeDirector
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsAlternativeDirector"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsAlternativeDirector", value);
			}
		}


		/// <summary>
		/// Secretary.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsSecretary
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsSecretary"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsSecretary", value);
			}
		}


		/// <summary>
		/// Shareholder.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsShareholder
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsShareholder"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsShareholder", value);
			}
		}


		/// <summary>
		/// Ultimate Beneficiary Owner.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsUltimateBeneficiaryOwner
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsUltimateBeneficiaryOwner"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsUltimateBeneficiaryOwner", value);
			}
		}


		/// <summary>
		/// Authorised Signatory.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsAuthorisedSignatory
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsAuthorisedSignatory"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsAuthorisedSignatory", value);
			}
		}


		/// <summary>
		/// Authorised Person.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsAuthorisedPerson
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsAuthorisedPerson"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsAuthorisedPerson", value);
			}
		}


		/// <summary>
		/// Designated E-Banking User.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsDesignatedEBankingUser
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsDesignatedEBankingUser"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsDesignatedEBankingUser", value);
			}
		}


		/// <summary>
		/// Authorised Cardholder.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsAuthorisedCardholder
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsAuthorisedCardholder"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsAuthorisedCardholder", value);
			}
		}


		/// <summary>
		/// Authorised Contact Person.
		/// </summary>
		[DatabaseField]
		public bool RelatedPartyRoles_IsAuthorisedContactPerson
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("RelatedPartyRoles_IsAuthorisedContactPerson"), false);
			}
			set
			{
				SetValue("RelatedPartyRoles_IsAuthorisedContactPerson", value);
			}
		}


		/// <summary>
		/// Gets an object that provides extended API for working with RelatedPartyRolesLegal fields.
		/// </summary>
		[RegisterProperty]
		public RelatedPartyRolesLegalFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with RelatedPartyRolesLegal fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class RelatedPartyRolesLegalFields : AbstractHierarchicalObject<RelatedPartyRolesLegalFields>
		{
			/// <summary>
			/// The content item of type RelatedPartyRolesLegal that is a target of the extended API.
			/// </summary>
			private readonly RelatedPartyRolesLegal mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="RelatedPartyRolesLegalFields" /> class with the specified content item of type RelatedPartyRolesLegal.
			/// </summary>
			/// <param name="instance">The content item of type RelatedPartyRolesLegal that is a target of the extended API.</param>
			public RelatedPartyRolesLegalFields(RelatedPartyRolesLegal instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// RelatedPartyRolesID.
			/// </summary>
			public int ID
			{
				get
				{
					return mInstance.RelatedPartyRolesLegalID;
				}
				set
				{
					mInstance.RelatedPartyRolesLegalID = value;
				}
			}


			/// <summary>
			/// Alternate Secretary.
			/// </summary>
			public bool RelatedPartyRoles_IsAlternateSecretery
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsAlternateSecretery;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsAlternateSecretery = value;
				}
			}


			/// <summary>
			/// Chairman Of The Board Of Directors.
			/// </summary>
			public bool RelatedPartyRoles_IsChairmanOfTheBoardOfDirector
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsChairmanOfTheBoardOfDirector;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsChairmanOfTheBoardOfDirector = value;
				}
			}


			/// <summary>
			/// Director.
			/// </summary>
			public bool RelatedPartyRoles_IsDirector
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsDirector;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsDirector = value;
				}
			}


			/// <summary>
			/// Vice-Chairman Of The Board Of Directors.
			/// </summary>
			public bool RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector = value;
				}
			}


			/// <summary>
			/// Secretary Of The Board Of Directors.
			/// </summary>
			public bool RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector = value;
				}
			}


			/// <summary>
			/// Treasurer Of Board Of Directors.
			/// </summary>
			public bool RelatedPartyRoles_IsTreasurerOfBoardOfDirectors
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsTreasurerOfBoardOfDirectors;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsTreasurerOfBoardOfDirectors = value;
				}
			}


			/// <summary>
			/// Member Of Board Of Directors.
			/// </summary>
			public bool RelatedPartyRoles_IsMemeberOfBoardOfDirectors
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsMemeberOfBoardOfDirectors;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsMemeberOfBoardOfDirectors = value;
				}
			}


			/// <summary>
			/// Partner.
			/// </summary>
			public bool RelatedPartyRoles_IsPartner
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsPartner;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsPartner = value;
				}
			}


			/// <summary>
			/// General Partner.
			/// </summary>
			public bool RelatedPartyRoles_GeneralPartner
			{
				get
				{
					return mInstance.RelatedPartyRoles_GeneralPartner;
				}
				set
				{
					mInstance.RelatedPartyRoles_GeneralPartner = value;
				}
			}


			/// <summary>
			/// Limited Partner.
			/// </summary>
			public bool RelatedPartyRoles_LimitedPartner
			{
				get
				{
					return mInstance.RelatedPartyRoles_LimitedPartner;
				}
				set
				{
					mInstance.RelatedPartyRoles_LimitedPartner = value;
				}
			}


			/// <summary>
			/// President Of Committee.
			/// </summary>
			public bool RelatedPartyRoles_IsPresidentOfCommittee
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsPresidentOfCommittee;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsPresidentOfCommittee = value;
				}
			}


			/// <summary>
			/// Vice-President Of Committee.
			/// </summary>
			public bool RelatedPartyRoles_IsVicePresidentOfCommittee
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsVicePresidentOfCommittee;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsVicePresidentOfCommittee = value;
				}
			}


			/// <summary>
			/// Secretary Of Committee.
			/// </summary>
			public bool RelatedPartyRoles_IsSecretaryOfCommittee
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsSecretaryOfCommittee;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsSecretaryOfCommittee = value;
				}
			}


			/// <summary>
			/// Treasurer Of Committee.
			/// </summary>
			public bool RelatedPartyRoles_IsTreasurerOfCommittee
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsTreasurerOfCommittee;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsTreasurerOfCommittee = value;
				}
			}


			/// <summary>
			/// Member Of Committee.
			/// </summary>
			public bool RelatedPartyRoles_IsMemeberOfCommittee
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsMemeberOfCommittee;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsMemeberOfCommittee = value;
				}
			}


			/// <summary>
			/// Trustee.
			/// </summary>
			public bool RelatedPartyRoles_IsTrustee
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsTrustee;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsTrustee = value;
				}
			}


			/// <summary>
			/// Settlor.
			/// </summary>
			public bool RelatedPartyRoles_IsSettlor
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsSettlor;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsSettlor = value;
				}
			}


			/// <summary>
			/// Protector.
			/// </summary>
			public bool RelatedPartyRoles_IsProtector
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsProtector;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsProtector = value;
				}
			}


			/// <summary>
			/// Benificiary.
			/// </summary>
			public bool RelatedPartyRoles_IsBenificiary
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsBenificiary;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsBenificiary = value;
				}
			}


			/// <summary>
			/// Founder.
			/// </summary>
			public bool RelatedPartyRoles_IsFounder
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsFounder;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsFounder = value;
				}
			}


			/// <summary>
			/// President Of Council.
			/// </summary>
			public bool RelatedPartyRoles_IsPresidentOfCouncil
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsPresidentOfCouncil;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsPresidentOfCouncil = value;
				}
			}


			/// <summary>
			/// Vice-President Of Council.
			/// </summary>
			public bool RelatedPartyRoles_IsVicePresidentOfCouncil
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsVicePresidentOfCouncil;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsVicePresidentOfCouncil = value;
				}
			}


			/// <summary>
			/// Secretary Of Council.
			/// </summary>
			public bool RelatedPartyRoles_IsSecretaryOfCouncil
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsSecretaryOfCouncil;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsSecretaryOfCouncil = value;
				}
			}


			/// <summary>
			/// Treasurer Of Council.
			/// </summary>
			public bool RelatedPartyRoles_IsTreasurerOfCouncil
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsTreasurerOfCouncil;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsTreasurerOfCouncil = value;
				}
			}


			/// <summary>
			/// Member Of Council.
			/// </summary>
			public bool RelatedPartyRoles_IsMemberOfCouncil
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsMemberOfCouncil;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsMemberOfCouncil = value;
				}
			}


			/// <summary>
			/// Fund MLCO.
			/// </summary>
			public bool RelatedPartyRoles_IsFundMlco
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsFundMlco;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsFundMlco = value;
				}
			}


			/// <summary>
			/// Fund Administrator.
			/// </summary>
			public bool RelatedPartyRoles_IsFundAdministrator
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsFundAdministrator;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsFundAdministrator = value;
				}
			}


			/// <summary>
			/// Management Company.
			/// </summary>
			public bool RelatedPartyRoles_IsManagementCompany
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsManagementCompany;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsManagementCompany = value;
				}
			}


			/// <summary>
			/// Holder Of Management Shares.
			/// </summary>
			public bool RelatedPartyRoles_IsHolderOfManagementShares
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsHolderOfManagementShares;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsHolderOfManagementShares = value;
				}
			}


			/// <summary>
			/// Alternative Director.
			/// </summary>
			public bool RelatedPartyRoles_IsAlternativeDirector
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsAlternativeDirector;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsAlternativeDirector = value;
				}
			}


			/// <summary>
			/// Secretary.
			/// </summary>
			public bool RelatedPartyRoles_IsSecretary
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsSecretary;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsSecretary = value;
				}
			}


			/// <summary>
			/// Shareholder.
			/// </summary>
			public bool RelatedPartyRoles_IsShareholder
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsShareholder;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsShareholder = value;
				}
			}


			/// <summary>
			/// Ultimate Beneficiary Owner.
			/// </summary>
			public bool RelatedPartyRoles_IsUltimateBeneficiaryOwner
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsUltimateBeneficiaryOwner;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsUltimateBeneficiaryOwner = value;
				}
			}


			/// <summary>
			/// Authorised Signatory.
			/// </summary>
			public bool RelatedPartyRoles_IsAuthorisedSignatory
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsAuthorisedSignatory;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsAuthorisedSignatory = value;
				}
			}


			/// <summary>
			/// Authorised Person.
			/// </summary>
			public bool RelatedPartyRoles_IsAuthorisedPerson
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsAuthorisedPerson;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsAuthorisedPerson = value;
				}
			}


			/// <summary>
			/// Designated E-Banking User.
			/// </summary>
			public bool RelatedPartyRoles_IsDesignatedEBankingUser
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsDesignatedEBankingUser;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsDesignatedEBankingUser = value;
				}
			}


			/// <summary>
			/// Authorised Cardholder.
			/// </summary>
			public bool RelatedPartyRoles_IsAuthorisedCardholder
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsAuthorisedCardholder;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsAuthorisedCardholder = value;
				}
			}


			/// <summary>
			/// Authorised Contact Person.
			/// </summary>
			public bool RelatedPartyRoles_IsAuthorisedContactPerson
			{
				get
				{
					return mInstance.RelatedPartyRoles_IsAuthorisedContactPerson;
				}
				set
				{
					mInstance.RelatedPartyRoles_IsAuthorisedContactPerson = value;
				}
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="RelatedPartyRolesLegal" /> class.
		/// </summary>
		public RelatedPartyRolesLegal() : base(CLASS_NAME)
		{
			mFields = new RelatedPartyRolesLegalFields(this);
		}

		#endregion
	}
}
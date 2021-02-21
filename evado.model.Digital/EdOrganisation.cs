/***************************************************************************************
 * <copyright file="EvOrganisation.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named \license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 * 
 * Description: 
 *  This class contains the EvOrganisation data object.
 *
 ****************************************************************************************/

using System;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// Business entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvOrganisation
  {
    #region Enumerators
    /// <summary>
    /// This enumeration list defines the types of organization
    /// </summary>
    public enum OrganisationTypes
    {
      /// <summary>
      /// This enumeration defines null value or not selection state.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines a account holder organization
      /// </summary>
      Evado,

      /// <summary>
      /// This enumeration defines a trial manager organization
      /// </summary>
      Management,

      /// <summary>
      /// This enumeration defines a trial manager organization
      /// </summary>
      User,
    }

    /// <summary>
    /// This enumeration list defines the states of organization
    /// </summary>
    public enum OrganisationStates
    {
      /// <summary>
      /// This enumeration defines null value or not selection state.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines a registered state of organization
      /// </summary>
      Registered,

      /// <summary>
      /// This enumeration defines an ethics approved state of organization
      /// </summary>
      Ethics_Approved,

      /// <summary>
      /// This enumeration defines a recruitment started state of organization
      /// </summary>
      Recruitment_Started,

      /// <summary>
      /// This enumeration defines a recruitment closed state of organization
      /// </summary>
      Recruitment_Closed,

      /// <summary>
      /// This enumeration defines a database locked state of organization
      /// </summary>
      Last_Treated_Subject,

      /// <summary>
      /// This enumeration defines a database locked state of organization
      /// </summary>
      Database_Locked,

      /// <summary>
      /// This enumeration defines a database unlocked state of organization
      /// </summary>
      Database_Unlocked,

      /// <summary>
      /// This enumeration defines a site closed state of organization
      /// </summary>
      Site_Closed,
    }

    /// <summary>
    /// This enumeration list defines the field names of organization
    /// </summary>
    public enum OrganisationFieldNames
    {
      /// <summary>
      /// This enumeration defines null value or non selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines a trial identifier field name of an organization
      /// </summary>
      TrialId,

      /// <summary>
      /// This enumeration defines an organization identifier field name of an organization
      /// </summary>
      OrgId,

      /// <summary>
      /// This enumeration defines a name field name of an organization
      /// </summary>
      Name,

      /// <summary>
      /// This enumeration defines a organisation type field name of an organization
      /// </summary>
      Org_Type,

      /// <summary>
      /// This enumeration defines an address field delimited address of an organization
      /// </summary>
      Address,

      /// <summary>
      /// This enumeration defines an address field name of an organization
      /// </summary>
      Address_1,

      /// <summary>
      /// This enumeration defines an address field name of an organization
      /// </summary>
      Address_2,

      /// <summary>
      /// This enumeration defines an address city field name of an organization
      /// </summary>
      Address_City,

      /// <summary>
      /// This enumeration defines an address post code field name of an organization
      /// </summary>
      Address_Post_Code,

      /// <summary>
      /// This enumeration defines an address state field name of an organization
      /// </summary>
      Address_State,

      /// <summary>
      /// This enumeration defines a country field name of an organization
      /// </summary>
      Address_Country,

      /// <summary>
      /// This enumeration defines a telephone field name of an organization
      /// </summary>
      Telephone,

      /// <summary>
      /// This enumeration defines a fax phone field name of an organization
      /// </summary>
      Fax_Phone,

      /// <summary>
      /// This enumeration defines an email field name of an organization
      /// </summary>
      Email_Address,

      /// <summary>
      /// This enumeration defines a trial site field name of an organization
      /// </summary>
      TrialSite,

      /// <summary>
      /// This enumeration defines a sponsor site field name of an organization
      /// </summary>
      Sponsor_Site,

      /// <summary>
      /// This enumeration defines a current field name of an organization
      /// </summary>
      Current,

      /// <summary>
      /// This enumeration defines an order field name of an organization
      /// </summary>
      Order,

      /// <summary>
      /// This enumeration defines a state field name of an organization
      /// </summary>
      State,

      /// <summary>
      /// This enumeration defines the ethics approval of milestone class
      /// </summary>
      Ethics_Approval_Date,

      /// <summary>
      /// This enumeration defines a recruitment start date field name of an organization
      /// </summary>
      Recruitment_Start_Date,

      /// <summary>
      /// This enumeration defines a recruitment closed date field name of an organization
      /// </summary>
      Recruitment_Closed_Date,

      /// <summary>
      /// This enumeration defines a last treated subject date field name of an organization
      /// </summary>
      LastTreated_Subject_Date,

      /// <summary>
      /// This enumeration defines a closed date field name of an organization
      /// </summary>
      Closed_Date,

      /// <summary>
      /// This enumeration defines an establishment cost field name of an organization
      /// </summary>
      Establishment_Cost,

      /// <summary>
      /// This enumeration defines a default margin field name of an organization
      /// </summary>
      Default_Margin,

      /// <summary>
      /// This enumeration defines a budget cost total field name of an organization
      /// </summary>
      Budget_Cost_Total,

      /// <summary>
      /// This enumeration defines a budget price total field name of an organization
      /// </summary>
      Budget_Price_Total,

      /// <summary>
      /// This enumeration defines an invoiced total field name of an organization
      /// </summary>
      Invoiced_Total,

      /// <summary>
      /// This enumeration defines a recruitment target field name of an organization
      /// </summary>
      Recruitment_Target,

      /// <summary>
      /// This enumeration defines a recruitment actual field name of an organization
      /// </summary>
      Recruitment_Actual,

      /// <summary>
      /// This enumeration defines the coordinating user id of organisation class
      /// </summary>
      Coordinating_User_Id,

      /// <summary>
      /// This enumeration defines the investigator of organisation class
      /// </summary>
      Site_Investigator,
    }


    /// <summary>
    /// This enumeration list defines the milestone scheduling options
    /// </summary>
    public enum SiteDashboardOptions
    {
      /// <summary>
      /// This enumeration defines no selection
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines a project description component
      /// </summary>
      Description,

      /// <summary>
      /// This enumeration defines an project recruitment component
      /// </summary>
      Recruitment_Target,

      /// <summary>
      /// This enumeration defines an project actual recruitment quantity
      /// </summary>
      Recruitment_Actual,
      /// <summary>
      /// This enumeration defines an project actual recruitment quantity
      /// </summary>
      Recruitment_Chart,

      /// <summary>
      /// This enumeration defines an project principal investigator
      /// </summary>
       Investigator,
    }

    /// <summary>
    /// This enumeration list defines the action codes for Trial orgnization object.
    /// </summary>
    public enum ActionCodes
    {
      /// <summary>
      /// This enumeation defines the not select value.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the save action for the Trial organization object
      /// </summary>
      Save,

      /// <summary>
      /// This enumeration defines the register action for the Trial organization object
      /// </summary>
      Register,

      /// <summary>
      /// This enumeration defines the ethics approval action for the Trial organization object
      /// </summary>
      Ethics_Approved,

      /// <summary>
      /// This enumeration defines the recruitment started action for the Trial organization object
      /// </summary>
      Recruitment_Started,

      /// <summary>
      /// This enumeration defines the recruitment closed action for the Trial organization object
      /// </summary>
      Recruitment_Closed,

      /// <summary>
      /// This enumeration defines the last subject treated action for the Trial organization object
      /// </summary>
      LastSubjectTreated,

      /// <summary>
      /// This enumeration defines the lock site action for the Trial organization object
      /// </summary>
      Lock_Site,

      /// <summary>
      /// This enumeration defines the unlock site action for the Trial organization object
      /// </summary>
      UnLockSite,

      /// <summary>
      /// This enumeration defines the close site action for the Trial organization object
      /// </summary>
      Close_Site,

      /// <summary>
      /// This enumeration defines the ssuperseded action for the Trial organization object
      /// </summary>
      Delete_Object
    }

    #endregion

    #region Internal member variables

    private Guid _Guid = Guid.Empty;
    private Guid _OrgGuid = Guid.Empty;
    private string _OrgId = String.Empty;
    private string _AdGroup = String.Empty;
    private string _Name = String.Empty;
    private string _AddressStreet_1 = String.Empty;
    private string _AddressStreet_2 = String.Empty;
    private string _AddressCity = String.Empty;
    private string _AddressPostCode = String.Empty;
    private string _AddressState = String.Empty;
    private string _Country = String.Empty;
    private string _Telephone = String.Empty;
    private string _FaxPhone = String.Empty;
    private string _EmailAddress = String.Empty;
    private OrganisationTypes _OrgType = OrganisationTypes.Null;
    private int _ScheduleVersion = 0;
    /// <summary>
    /// This property contains the schedule release version.
    /// </summary>
    public int ScheduleVersion
    {
      get { return _ScheduleVersion; }
      set { _ScheduleVersion = value; }
    }
    private bool _TrialSite = false;
    private bool _SponsorSite = false;
    private bool _Current = true;
    private int _Order = 0;
    private string _CoordinatorUserId = String.Empty;
    private string _UpdatedBy = String.Empty;
    private DateTime _UpdatedDate = EvcStatics.CONST_DATE_NULL;
    private string _SupersededBy = String.Empty;
    private string _UserCommonName = String.Empty;
    private string _UpdatedByUserId = String.Empty;
    private bool _IsAuthenticatedSignature = false;
    private ActionCodes _Action = ActionCodes.Null;

    /// 
    /// Trial site components
    /// 
    private string _StudyId = String.Empty;
    private string _PrincipleInvestigatorId = String.Empty;
    private string _PrincipleInvestigator = String.Empty;
    private string _EthicsGroup = String.Empty;
    //private bool _EthicsPrimarySite = false;
    //private bool _EthicsAcceptPrimarySite = false;
    private int _RecruitmentTarget = 0;
    private int _RecruitmentActual = 0;
    private int _FundedScrenningFailures = 0;
    private DateTime _EthicsApprovalDate = EvcStatics.CONST_DATE_NULL;
    private DateTime _RecruitmentStartDate = EvcStatics.CONST_DATE_NULL;
    private DateTime _RecruitmentClosedDate = EvcStatics.CONST_DATE_NULL;
    private DateTime _LastTreatedSubjectDate = EvcStatics.CONST_DATE_NULL;
    private DateTime _ClosedDate = EvcStatics.CONST_DATE_NULL;
    private float _EstablishmentCost = 0;
    private float _SiteDefaultMargin = 0;
    private float _BudgetCostTotal = 0;
    private float _BudgetPriceTotal = 0;
    private float _InvoicedTotal = 0;
    private OrganisationStates _State = OrganisationStates.Null;
    private List<EdUserSignoff> _Signoffs = new List<EdUserSignoff> ( );

    /// <summary>
    /// This field defines if the object is selected.
    /// </summary>
    public bool Selected = false;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region public properties

    /// <summary>
    /// This property contains a global unique identifier of an organization
    /// </summary>
    public Guid Guid
    {
      get
      {
        return this._Guid;
      }
      set
      {
        this._Guid = value;
      }
    }

    /// <summary>
    /// This property contains an customer global unique identifier of an organization
    /// This foreign key links the organisation to the customer object.
    /// </summary>
    public Guid CustomerGuid { get; set; }

    /// <summary>
    /// This property contains an organization global unique identifier of an organization
    /// </summary>
    public Guid OrgGuid
    {
      get
      {
        return this._OrgGuid;
      }
      set
      {
        this._OrgGuid = value;
      }
    }

    /// <summary>
    /// This property contains a trial unique identifier of an organization
    /// </summary>
    public string TrialId
    {
      get
      {
        return this._StudyId;
      }
      set
      {
        this._StudyId = value;
      }
    }

    /// <summary>
    /// This property contains an organization identifier of an organization
    /// </summary>
    public string OrgId
    {
      get
      {
        return this._OrgId;
      }
      set
      {
        this._OrgId = value;
      }
    }

    /// <summary>
    /// This property contains the Active Directory group name for this organisation.
    /// </summary>
    public string AdGroup
    {
      get { return this._AdGroup; }
      set { this._AdGroup = value; }
    }


    /// <summary>
    /// This property contains a name of an organization
    /// </summary>
    public string Name
    {
      get
      {
        return this._Name;
      }
      set
      {
        this._Name = value;
      }
    }

    /// <summary>
    /// This property contains an address of an organization
    /// </summary>
    public string Address
    {
      get
      {
        return this._AddressStreet_1 + ";" 
          + this._AddressStreet_2 + ";" 
          + this._AddressCity + ";" 
          + this._AddressState + ";" 
          + this._AddressPostCode + ";" 
          + this._Country;
      }
      set
      {
        string [ ] arrAddess = value.Split ( ';' );

        if ( arrAddess.Length == 6   )
        {
          this._AddressStreet_1 = arrAddess [ 0 ];
          this._AddressStreet_2 = arrAddess [ 1 ];
          this._AddressCity = arrAddess [ 2 ];
          this._AddressState = arrAddess [ 3 ];
          this._AddressPostCode = arrAddess [ 4 ];
          this._Country = arrAddess [ 5 ];
        }
      }
    }

    /// <summary>
    /// This property contains an address of an organization
    /// </summary>
    public string AddressStreet_1
    {
      get
      {
        return this._AddressStreet_1;
      }
      set
      {
        this._AddressStreet_1 = value;
      }
    }

    /// <summary>
    /// This property contains an address of an organization
    /// </summary>
    public string AddressStreet_2
    {
      get
      {
        return this._AddressStreet_2;
      }
      set
      {
        this._AddressStreet_2 = value;
      }
    }

    /// <summary>
    /// This property contains an address of an organization
    /// </summary>
    public string AddressCity
    {
      get
      {
        return this._AddressCity;
      }
      set
      {
        this._AddressCity = value;
      }
    }

    /// <summary>
    /// This property contains an address of an organization
    /// </summary>
    public string AddressPostCode
    {
      get
      {
        return this._AddressPostCode;
      }
      set
      {
        this._AddressPostCode = value;
      }
    }

    /// <summary>
    /// This property contains an address of an organization
    /// </summary>
    public string AddressState
    {
      get
      {
        return this._AddressState;
      }
      set
      {
        this._AddressState = value;
      }
    }

    /// <summary>
    /// This property contains a country of an organization
    /// </summary>
    public string AddressCountry
    {
      get
      {
        return this._Country;
      }
      set
      {
        this._Country = value;
      }
    }

    /// <summary>
    /// This property contains a telephone of an organization
    /// </summary>
    public string Telephone
    {
      get
      {
        return this._Telephone;
      }
      set
      {
        this._Telephone = value;
      }
    }

    /// <summary>
    /// This property contains a fax phone of an organization
    /// </summary>
    public string FaxPhone
    {
      get
      {
        return this._FaxPhone;
      }
      set
      {
        this._FaxPhone = value;
      }
    }

    /// <summary>
    /// This property contains an email of an organization
    /// </summary>
    public string EmailAddress
    {
      get
      {
        return this._EmailAddress;
      }
      set
      {
        this._EmailAddress = value;
      }
    }

    /// <summary>
    /// This property contains the organisation type value.
    /// </summary>
    public OrganisationTypes OrgType
    {
      get { return _OrgType; }
      set { _OrgType = value; }
    }

    /// <summary>
    /// This property contains the organisation type value.
    /// </summary>
    public String stOrgType
    {
      get
      {
        return EvStatics.enumValueToString ( _OrgType );
      }
    }

    /// <summary>
    /// This property contains an order number of an organization
    /// </summary>
    public int Order
    {
      get
      {
        return this._Order;
      }
      set
      {
        this._Order = value;
      }
    }

    /// <summary>
    /// This property contains an order string number of an organization
    /// </summary>
    public string stOrder
    {
      get
      {
        return this._Order.ToString ( );
      }
      set
      {
        if ( Int32.TryParse ( value, out this._Order ) == false )
        {
          this._Order = 0;
        }
      }
    }

    /// <summary>
    /// This property indicates whether an organization is current.
    /// </summary>
    public bool Current
    {
      get
      {
        return this._Current;
      }
      set
      {
        this._Current = value;
      }
    }

    /// <summary>
    /// This property contains a summary of an organization
    /// </summary>
    public string LinkText
    {
      get
      {
        String stLinkText = this._OrgId
          + evado.model.Properties.Resources.Space_Hypen
          + this._Name;

        if ( this._OrgType != OrganisationTypes.Null )
        {
          stLinkText += evado.model.Properties.Resources.Space_Coma
            + evado.model.Properties.Resources.Organisation_Type_Label
            + EvStatics.enumValueToString ( this._OrgType );
        }

        return stLinkText;
      }
    }

    /// <summary>
    /// This property contains a user sign off object list of an organization
    /// </summary>
    public List<EdUserSignoff> Signoffs
    {
      get
      {
        return this._Signoffs;
      }
      set
      {
        this._Signoffs = value;
      }
    }

    /// <summary>
    /// This property contains the coordinators user identifier.
    /// The empty Coordinator user id indicates that all coordinators have edit access to this organisation.
    /// </summary>
    public string CoordinatorUserId
    {
      get
      {
        return this._CoordinatorUserId;
      }
      set
      {
        this._CoordinatorUserId = value;
      }
    }

    string _SiteInvestigator = String.Empty;
    /// <summary>
    /// This property contains the site invetgigator's name
    /// </summary>
    public string SiteInvestigator
    {
      get
      {
        return this._SiteInvestigator;
      }
      set
      {
        this._SiteInvestigator = value;
      }
    }

    /// <summary>
    /// This property contains an updated string of an organization
    /// </summary>
    public string UpdatedBy
    {
      get
      {
        return this._UpdatedBy;
      }
      set
      {
        this._UpdatedBy = value;
      }
    }
    /// <summary>
    /// This property contains the updated date 
    /// </summary>
    public DateTime UpdatedDate
    {
      get { return _UpdatedDate; }
      set { _UpdatedDate = value; }
    }

    /// <summary>
    /// This property contains a user common name who updates an organization
    /// </summary>
    public string UserCommonName
    {
      get
      {
        return this._UserCommonName;
      }
      set
      {
        this._UserCommonName = value;
      }
    }

    /// <summary>
    /// This property contains a user identifier of those who updates an organization
    /// </summary>
    public string UpdatedByUserId
    {
      get
      {
        return this._UpdatedByUserId;
      }
      set
      {
        this._UpdatedByUserId = value;
      }
    }

    /// <summary>
    /// This property contains indicates whether an organization has authenticated signature
    /// </summary>
    public bool IsAuthenticatedSignature
    {
      get
      {
        return this._IsAuthenticatedSignature;
      }
      set
      {
        this._IsAuthenticatedSignature = value;
      }
    }

    /// <summary>
    /// This property contains a principle investigator identifier of an organization
    /// </summary>
    public string PrincipleInvestigatorId
    {
      get
      {
        return this._PrincipleInvestigatorId;
      }
      set
      {
        this._PrincipleInvestigatorId = value;
      }
    }

    /// <summary>
    /// This property contains a principle investigator of an organization
    /// </summary>
    public string PrincipleInvestigator
    {
      get
      {
        return this._PrincipleInvestigator;
      }
      set
      {
        this._PrincipleInvestigator = value;
      }
    }

    /// <summary>
    /// This property contains a recruitment target of an organization
    /// </summary>
    public int RecruitmentTarget
    {
      get
      {
        return this._RecruitmentTarget;
      }
      set
      {
        this._RecruitmentTarget = value;
      }
    }

    /// <summary>
    /// This property contains a recruitment actual of an organization
    /// </summary>
    public int RecruitmentActual
    {
      get
      {
        return this._RecruitmentActual;
      }
      set
      {
        this._RecruitmentActual = value;
      }
    }

    /// <summary>
    /// This property contains a funded screnning failures of an organization
    /// </summary>
    public int FundedScrenningFailures
    {
      get
      {
        return this._FundedScrenningFailures;
      }
      set
      {
        this._FundedScrenningFailures = value;
      }
    }

    /// <summary>
    /// This property contains an ethics approval date of an organization
    /// </summary>
    public DateTime EthicsApprovalDate
    {
      get
      {
        return this._EthicsApprovalDate;
      }
      set
      {
        this._EthicsApprovalDate = value;
      }
    }

    /// <summary>
    /// This property contains an ethics approval date of an organization
    /// </summary>
    public string stEthicsApprovalDate
    {
      get
      {
        if ( this._EthicsApprovalDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._EthicsApprovalDate.ToString ( "dd MMM yyyy" ); ;
        }
        return String.Empty;
      }
      set
      {
        String v = value;
      }
    }

    /// <summary>
    /// This property contains a recruitment start date of an organization
    /// </summary>
    public DateTime RecruitmentStartDate
    {
      get
      {
        return this._RecruitmentStartDate;
      }
      set
      {
        this._RecruitmentStartDate = value;
      }
    }

    /// <summary>
    /// This property contains a recruitment start date string of an organization
    /// </summary>
    public string stRecruitmentStartDate
    {
      get
      {
        if ( this._RecruitmentStartDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._RecruitmentStartDate.ToString ( "dd MMM yyyy" ); ;
        }
        return String.Empty;
      }
      set
      {
        String v = value;
      }
    }

    /// <summary>
    /// This property contains a recruitment close date of an organization
    /// </summary>
    public DateTime RecruitmentClosedDate
    {
      get
      {
        return this._RecruitmentClosedDate;
      }
      set
      {
        this._RecruitmentClosedDate = value;
      }
    }

    /// <summary>
    /// This property contains a recruitment closed date string of an organization
    /// </summary>
    public string stRecruitmentClosedDate
    {
      get
      {
        if ( this._RecruitmentClosedDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._RecruitmentClosedDate.ToString ( "dd MMM yyyy" ); ;
        }
        return String.Empty;
      }
      set
      {
        String v = value;
      }
    }

    /// <summary>
    /// This property contains a last treated subject date of an organization
    /// </summary>
    public DateTime LastTreatedSubjectDate
    {
      get
      {
        return this._LastTreatedSubjectDate;
      }
      set
      {
        this._LastTreatedSubjectDate = value;
      }
    }

    /// <summary>
    /// This property contains a last treated subject date string of an organization
    /// </summary>
    public string stLastTreatedSubjectDate
    {
      get
      {
        if ( this._LastTreatedSubjectDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._LastTreatedSubjectDate.ToString ( "dd MMM yyyy" ); ;
        }
        return String.Empty;
      }
      set
      {
        String v = value;
      }
    }

    /// <summary>
    /// This property contains a close date of an organization
    /// </summary>
    public DateTime ClosedDate
    {
      get
      {
        return this._ClosedDate;
      }
      set
      {
        this._ClosedDate = value;
      }
    }

    /// <summary>
    /// This property contains a close date string of an organization
    /// </summary>
    public string stClosedDate
    {
      get
      {
        if ( this._ClosedDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._ClosedDate.ToString ( "dd MMM yyyy" ); ;
        }
        return String.Empty;
      }
      set
      {
        String v = value;
      }
    }

    /// <summary>
    /// This property contains a site default margin of an organization
    /// </summary>
    public float SiteDefaultMargin
    {
      get
      {
        return this._SiteDefaultMargin;
      }
      set
      {
        this._SiteDefaultMargin = value;
      }
    }

    /// <summary>
    /// This property contains an establishment cost of an organization
    /// </summary>
    public float EstablishmentCost
    {
      get
      {
        return this._EstablishmentCost;
      }
      set
      {
        this._EstablishmentCost = value;
      }
    }

    /// <summary>
    /// This property contains a budget cost total of an organization
    /// </summary>
    public float BudgetCostTotal
    {
      get
      {
        return this._BudgetCostTotal;
      }
      set
      {
        this._BudgetCostTotal = value;
      }
    }

    /// <summary>
    /// This property contains a budget price total of an organization
    /// </summary>
    public float BudgetPriceTotal
    {
      get
      {
        return this._BudgetPriceTotal;
      }
      set
      {
        this._BudgetPriceTotal = value;
      }
    }

    /// <summary>
    /// This property contains an invoiced total of an organization
    /// </summary>
    public float InvoicedTotal
    {
      get
      {
        return this._InvoicedTotal;
      }
      set
      {
        this._InvoicedTotal = value;
      }
    }

    /// <summary>
    /// This property contains a state object of an organization
    /// </summary>
    public OrganisationStates State
    {
      get
      {
        return this._State;
      }
      set
      {
        this._State = value;
      }
    }

    /// <summary>
    /// This property contains a state description of an organization
    /// </summary>
    public string StateDesc
    {
      get
      {
        return EvStatics.enumValueToString ( this._State );
      }
      set
      {
        string v = value;
      }
    }

    /// <summary>
    /// This property indicates that hte trial is recruiting.
    /// </summary>
    public bool isRecruiting
    {
      get
      {
        // 
        // If the states allow data collection then set it true.
        // 
        if ( this._State == OrganisationStates.Recruitment_Started )
        {
          return true;
        }

        // 
        // default false.
        // 
        return false;
      }
    }

    /// <summary>
    /// This property indicates that the trial is collecting data.
    /// </summary>
    public bool isCollectingData
    {
      get
      {
        // 
        // If the states allow data collection then set it true.
        // 
        if ( this._State == OrganisationStates.Recruitment_Started
          || this._State == OrganisationStates.Recruitment_Closed
          || this._State == OrganisationStates.Database_Unlocked )
        {
          return true;
        }

        // 
        // default false.
        // 
        return false;
      }
    }

    /// <summary>
    /// This property contains save action setting for the object.
    /// </summary>

    public ActionCodes Action
    {
      get { return _Action; }
      set { _Action = value; }
    }

    #endregion

    #region public methods

    // ==================================================================================
    /// <summary>
    /// This method provides a list of trial types.
    /// </summary>
    /// <param name="FieldName">EvOrganisation.OrganisationFieldNames: a Field Name object</param>
    /// <returns>string: a string value</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Switch FieldName and get value defining by the organization field name.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public string getValue ( EvOrganisation.OrganisationFieldNames FieldName )
    {
      //
      // Switch FieldName and get value defining by the organization field name.
      //
      switch ( FieldName )
      {
        case EvOrganisation.OrganisationFieldNames.TrialId:
          return this._StudyId;
        case EvOrganisation.OrganisationFieldNames.OrgId:
          return this._OrgId;
        case EvOrganisation.OrganisationFieldNames.Name:
          return this._Name;

        case EvOrganisation.OrganisationFieldNames.Address:
          return this.Address;

        case EvOrganisation.OrganisationFieldNames.Address_1:
          return this.AddressStreet_1;

        case EvOrganisation.OrganisationFieldNames.Address_2:
          return this.AddressStreet_2;

        case EvOrganisation.OrganisationFieldNames.Address_City:
          return this.AddressCity;

        case EvOrganisation.OrganisationFieldNames.Address_State:
          return this.AddressState;

        case EvOrganisation.OrganisationFieldNames.Address_Post_Code:
          return this.AddressPostCode;

        case EvOrganisation.OrganisationFieldNames.Address_Country:
          return this.AddressCountry;

        case EvOrganisation.OrganisationFieldNames.Telephone:
          return this._Telephone;

        case EvOrganisation.OrganisationFieldNames.Fax_Phone:
          return this._FaxPhone;

        case EvOrganisation.OrganisationFieldNames.Email_Address:
          return this._EmailAddress;

        case EvOrganisation.OrganisationFieldNames.TrialSite:
          return this._TrialSite.ToString ( );

        case EvOrganisation.OrganisationFieldNames.Sponsor_Site:
          return this._SponsorSite.ToString ( );

        case EvOrganisation.OrganisationFieldNames.Current:
          return this._Current.ToString ( );

        case EvOrganisation.OrganisationFieldNames.Coordinating_User_Id:
          return this._CoordinatorUserId;

        case EvOrganisation.OrganisationFieldNames.Order:
          return this._Order.ToString ( );

        case EvOrganisation.OrganisationFieldNames.State:
          return this.StateDesc;

        case EvOrganisation.OrganisationFieldNames.Ethics_Approval_Date:
          return this._EthicsApprovalDate.ToString ( "dd MMM yyyy" );

        case EvOrganisation.OrganisationFieldNames.Recruitment_Start_Date:
          return this._RecruitmentStartDate.ToString ( "dd MMM yyyy" );

        case EvOrganisation.OrganisationFieldNames.Recruitment_Closed_Date:
          return this._RecruitmentClosedDate.ToString ( "dd MMM yyyy" );

        case EvOrganisation.OrganisationFieldNames.LastTreated_Subject_Date:
          return this._LastTreatedSubjectDate.ToString ( "dd MMM yyyy" );

        case EvOrganisation.OrganisationFieldNames.Closed_Date:
          return this._ClosedDate.ToString ( "dd MMM yyyy" );

        case EvOrganisation.OrganisationFieldNames.Establishment_Cost:
          return this._EstablishmentCost.ToString ( "#######0" );

        case EvOrganisation.OrganisationFieldNames.Default_Margin:
          return this._SiteDefaultMargin.ToString ( "##0" );

        case EvOrganisation.OrganisationFieldNames.Budget_Cost_Total:
          return this._BudgetCostTotal.ToString ( "#######0" );

        case EvOrganisation.OrganisationFieldNames.Budget_Price_Total:
          return this._BudgetPriceTotal.ToString ( "#######0" );

        case EvOrganisation.OrganisationFieldNames.Invoiced_Total:
          return this._InvoicedTotal.ToString ( "#######0" );

        case EvOrganisation.OrganisationFieldNames.Recruitment_Target:
          return this._RecruitmentTarget.ToString ( "#######0" );
        default:
          return String.Empty;

      }//END Switch

    }//END getValue method

    // ==================================================================================
    /// <summary>
    /// This method sets the field value.
    /// </summary>
    /// <param name="FieldName">EvOrganisation.OrganisationFieldNames: a field name object</param>
    /// <param name="Value">string: a Value for updating</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize the internal variables
    /// 
    /// 2. Switch the FieldName and update the Value on the organization field names.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void setValue ( EvOrganisation.OrganisationFieldNames FieldName, string Value )
    {
      //
      // Initialize the internal variables
      //
      DateTime date = EvcStatics.CONST_DATE_NULL;
      float fltValue = 0;
      int intValue = 0;

      //
      // Switch the FieldName and update the Value on the organization field names.
      //
      switch ( FieldName )
      {
        case EvOrganisation.OrganisationFieldNames.TrialId:
          this._StudyId = Value;
          return;

        case EvOrganisation.OrganisationFieldNames.OrgId:
          this._OrgId = Value;
          return;

        case EvOrganisation.OrganisationFieldNames.Name:
          this._Name = Value;
          return;

        case EvOrganisation.OrganisationFieldNames.Address:
          {
            this.Address = Value;
          } return;

        case EvOrganisation.OrganisationFieldNames.Address_1:
          this._AddressStreet_1 = Value;
          return;

        case EvOrganisation.OrganisationFieldNames.Address_2:
          this._AddressStreet_2 = Value;
          return;

        case EvOrganisation.OrganisationFieldNames.Address_City:
          this._AddressCity = Value;
          return;

        case EvOrganisation.OrganisationFieldNames.Address_State:
          this._AddressState = Value;
          return;

        case EvOrganisation.OrganisationFieldNames.Address_Post_Code:
          this._AddressPostCode = Value;
          return;

        case EvOrganisation.OrganisationFieldNames.Address_Country:
          this._Country = Value;
          return;

        case EvOrganisation.OrganisationFieldNames.Telephone:
          this._Telephone = Value;
          return;

        case EvOrganisation.OrganisationFieldNames.Fax_Phone:
          this._FaxPhone = Value;
          return;

        case EvOrganisation.OrganisationFieldNames.Email_Address:
          this._EmailAddress = Value;
          return;

        case EvOrganisation.OrganisationFieldNames.Coordinating_User_Id:
          this._CoordinatorUserId = Value;
          return;

        case EvOrganisation.OrganisationFieldNames.Site_Investigator:
          this._SiteInvestigator= Value;
          return;

        case EvOrganisation.OrganisationFieldNames.Org_Type:
          {
            this._OrgType = EvStatics.parseEnumValue<OrganisationTypes> ( Value );
            return;
          }
        case EvOrganisation.OrganisationFieldNames.TrialSite:
          {
            this._TrialSite = false;
            if ( Value.ToLower ( ) == "true"
              || Value.ToLower ( ) == "yes"
              || Value.ToLower ( ) == "1" )
            {
              this._TrialSite = true;
            }
            return;
          }

        case EvOrganisation.OrganisationFieldNames.Sponsor_Site:
          {
            this._SponsorSite = false;
            if ( Value.ToLower ( ) == "true"
              || Value.ToLower ( ) == "yes"
              || Value.ToLower ( ) == "1" )
            {
              this._SponsorSite = true;
            }
            return;
          }

        case EvOrganisation.OrganisationFieldNames.Current:
          {
            this._Current = false;
            if ( Value.ToLower ( ) == "true"
              || Value.ToLower ( ) == "yes"
              || Value.ToLower ( ) == "1" )
            {
              this._Current = true;
            }
            return;
          }

        case EvOrganisation.OrganisationFieldNames.Order:
          {
            this.stOrder = Value;
            return;
          }

        case EvOrganisation.OrganisationFieldNames.Ethics_Approval_Date:
          {
            if ( DateTime.TryParse ( Value, out date ) == true )
            {
              this._EthicsApprovalDate = date;
            }
            return;
          }

        case EvOrganisation.OrganisationFieldNames.Recruitment_Start_Date:
          {
            if ( DateTime.TryParse ( Value, out date ) == true )
            {
              this._RecruitmentStartDate = date;
            }
            return;
          }

        case EvOrganisation.OrganisationFieldNames.Recruitment_Closed_Date:
          {
            if ( DateTime.TryParse ( Value, out date ) == true )
            {
              this._RecruitmentClosedDate = date;
            }
            return;
          }

        case EvOrganisation.OrganisationFieldNames.LastTreated_Subject_Date:
          {
            if ( DateTime.TryParse ( Value, out date ) == true )
            {
              this._LastTreatedSubjectDate = date;
            }
            return;
          }

        case EvOrganisation.OrganisationFieldNames.Closed_Date:
          {
            if ( DateTime.TryParse ( Value, out date ) == true )
            {
              this._ClosedDate = date;
            }
            return;
          }

        case EvOrganisation.OrganisationFieldNames.Establishment_Cost:
          {
            if ( float.TryParse ( Value, out fltValue ) == true )
            {
              this._EstablishmentCost = fltValue;
            }
            return;
          }

        case EvOrganisation.OrganisationFieldNames.Default_Margin:
          {
            if ( int.TryParse ( Value, out intValue ) == true )
            {
              this._SiteDefaultMargin = intValue;
            }
            return;
          }

        case EvOrganisation.OrganisationFieldNames.Budget_Cost_Total:
          {
            if ( float.TryParse ( Value, out fltValue ) == true )
            {
              this._BudgetCostTotal = fltValue;
            }
            return;
          }

        case EvOrganisation.OrganisationFieldNames.Budget_Price_Total:
          {
            if ( float.TryParse ( Value, out fltValue ) == true )
            {
              this._BudgetPriceTotal = fltValue;
            }
            return;
          }

        case EvOrganisation.OrganisationFieldNames.Invoiced_Total:
          {
            if ( float.TryParse ( Value, out fltValue ) == true )
            {
              this._InvoicedTotal = fltValue;
            }
            return;
          }
        case EvOrganisation.OrganisationFieldNames.Recruitment_Target:
          {
            if ( int.TryParse ( Value, out intValue ) == true )
            {
              this._RecruitmentTarget = intValue;
            }
            return;
          }
        default:

          return;

      }//END Switch

    }//END setValue method 

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Static methods
    /// <summary>
    /// This method returnes a list of options containing organisation types.
    /// </summary>
    /// <param name="CurrentType">Current organisation type</param>
    /// <returns></returns>
    public static List<EvOption> getOrganisationTypeList ( OrganisationTypes CurrentType )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( ) );

      switch ( CurrentType )
      {
        case OrganisationTypes.Evado:
          {
            optionList.Add ( EvStatics.getOption (
              OrganisationTypes.Management ) );

            optionList.Add ( EvStatics.getOption (
              OrganisationTypes.Management ) );
            return optionList;
          }
        case OrganisationTypes.Management:
          {
            optionList.Add ( EvStatics.getOption (
              OrganisationTypes.Management ) );
            return optionList;
          }
        default:
          {
            return EvStatics.getOptionsFromEnum ( 
              typeof ( OrganisationTypes ), true );
          }
      }
    }//END static method.

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END EvOrganisation class

}//END namespace Evado.Model.Clinical

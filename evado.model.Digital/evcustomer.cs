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
 *  This class contains the EvCustomer data object.
 *
 ****************************************************************************************/

using System;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This class contains the Evado Clinical Customer details.
  /// </summary>
  [Serializable]
  public class EvCustomer
  {
    #region Class Enumerated lists

    /// <summary>
    /// This enumeration list defines the states of customer
    /// </summary>
    public enum ServiceTypes
    {
      /// <summary>
      /// This enumeration defines null value or not selection state.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines a registered state of customer
      /// </summary>
      Lite,

      /// <summary>
      /// This enumeration defines an ethics approved state of customer
      /// </summary>
      Standard,

      /// <summary>
      /// This enumeration defines an ethics approved state of customer
      /// </summary>
      Enhanced,

      /// <summary>
      /// This enumeration defines a recruitment started state of customer
      /// </summary>
      Enterprise,
    }

    /// <summary>
    /// This enumeration list defines the states of customer
    /// </summary>
    public enum CustomerStates
    {
      /// <summary>
      /// This enumeration defines null value or not selection state.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines a registered state of customer
      /// </summary>
      Registered,

      /// <summary>
      /// This enumeration defines an ethics approved state of customer
      /// </summary>
      Service_Approved,

      /// <summary>
      /// This enumeration defines a recruitment started state of customer
      /// </summary>
      Service_Discontinued,
    }

    /// <summary>
    /// This enumeration list defines the field names of customer
    /// </summary>
    public enum CustomerFieldNames
    {
      /// <summary>
      /// This enumeration defines null value or non selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines an customer identifier field name of an customer
      /// </summary>
      Customer_No,

      /// <summary>
      /// This enumeration defines a name field name of an customer
      /// </summary>
      Name,

      /// <summary>
      /// This enumeration defines an address field delimited address of an customer
      /// </summary>
      Address,

      /// <summary>
      /// This enumeration defines an administrator for an customer
      /// </summary>
      Administrator,

      /// <summary>
      /// This enumeration defines an administrator email address an customer
      /// </summary>
      AdminEmailAddress,

      /// <summary>
      /// This enumeration defines an address field name of an customer
      /// </summary>
      Address_1,

      /// <summary>
      /// This enumeration defines an address field name of an customer
      /// </summary>
      Address_2,

      /// <summary>
      /// This enumeration defines an address city field name of an customer
      /// </summary>
      Address_City,

      /// <summary>
      /// This enumeration defines an address post code field name of an customer
      /// </summary>
      Address_Post_Code,

      /// <summary>
      /// This enumeration defines an address state field name of an customer
      /// </summary>
      Address_State,

      /// <summary>
      /// This enumeration defines a country field name of an customer
      /// </summary>
      Address_Country,

      /// <summary>
      /// This enumeration defines a telephone field name of an customer
      /// </summary>
      Telephone,

      /// <summary>
      /// This enumeration defines a fax phone field name of an customer
      /// </summary>
      Fax_Phone,

      /// <summary>
      /// This enumeration defines an email field name of an customer
      /// </summary>
      Email_Address,

      /// <summary>
      /// This enumeration defines a current field name of an customer
      /// </summary>
      Current,

      /// <summary>
      /// This enumeration defines a state field name of an customer
      /// </summary>
      State,

      /// <summary>
      /// This enumeration defines a single study customer field
      /// </summary>
      Is_Single_Study,

      /// <summary>
      /// This enumeration defines a ADS Group customer field
      /// </summary>
      Ads_Group,

      /// <summary>
      /// This enumeration defines a Home page header text customer field
      /// </summary>
      Home_Page_Header,

      /// <summary>
      /// This enumeration defines a number of studies customer field
      /// </summary>
      No_Of_Studies,

      /// <summary>
      /// This enumeration defines a service type field name of an customer
      /// </summary>
      Service_Type,

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
      /// This enumeration defines the save action for the Trial customer object
      /// </summary>
      Save,

      /// <summary>
      /// This enumeration defines the ssuperseded action for the Trial customer object
      /// </summary>
      Delete_Object
    }

    #endregion

    #region class constants

    /// <summary>
    /// This consant defines the maximum number of subject sht system can handle
    /// </summary>
    public const int CONST_MAX_SUBJECTS = 100000;

    #endregion

    #region public properties

    private Guid _Guid = Guid.Empty;
    /// <summary>
    /// This property contains a global unique identifier of an customer
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
    /// This property contains a global unique identifier of an customer
    /// </summary>
    public Guid CustomerGuid
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


    int _CustomerNo = -1;
    /// <summary>
    /// This property contains an customer No
    /// </summary>
    public int CustomerNo
    {
      get
      {
        return this._CustomerNo;
      }
      set
      {
        this._CustomerNo = value;
      }
    }

    /// <summary>
    /// This property contains an customer identifier
    /// </summary>
    public String CustomerId
    {
      get
      {
        return "CU"+ this._CustomerNo.ToString ( "000" );
      }
    }

    private string _Name = String.Empty;
    /// <summary>
    /// This property contains a name of an customer
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

    private string _AdministratorName = String.Empty;
    /// <summary>
    /// This property contains a name of an customer
    /// </summary>
    public string Administrator
    {
      get
      {
        return this._AdministratorName;
      }
      set
      {
        this._AdministratorName = value;
      }
    }

    private string _AdministratorEmailAddress = String.Empty;
    /// <summary>
    /// This property contains a administors email address.
    /// </summary>
    public string AdminEmailAddress
    {
      get
      {
        return this._AdministratorEmailAddress;
      }
      set
      {
        this._AdministratorEmailAddress = value;
      }
    }

    private string _Telephone = String.Empty;
    private string _EmailAddress = String.Empty;
    /// <summary>
    /// This property contains an address of an customer
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
          + this._AddressCountry;
      }
      set
      {
        string [ ] arrAddess = value.Split ( ';' );

        if ( arrAddess.Length == 6 )
        {
          this._AddressStreet_1 = arrAddess [ 0 ];
          this._AddressStreet_2 = arrAddess [ 1 ];
          this._AddressCity = arrAddess [ 2 ];
          this._AddressState = arrAddess [ 3 ];
          this._AddressPostCode = arrAddess [ 4 ];
          this._AddressCountry = arrAddess [ 5 ];
        }
      }
    }

    private string _AddressStreet_1 = String.Empty;
    /// <summary>
    /// This property contains an address of an customer
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

    private string _AddressStreet_2 = String.Empty;
    /// <summary>
    /// This property contains an address of an customer
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

    private string _AddressCity = String.Empty;
    /// <summary>
    /// This property contains an address of an customer
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

    private string _AddressPostCode = String.Empty;
    /// <summary>
    /// This property contains an address of an customer
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

    private string _AddressState = String.Empty;
    /// <summary>
    /// This property contains an address of an customer
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

    private string _AddressCountry = String.Empty;
    /// <summary>
    /// This property contains a country of an customer
    /// </summary>
    public string AddressCountry
    {
      get
      {
        return this._AddressCountry;
      }
      set
      {
        this._AddressCountry = value;
      }
    }

    /// <summary>
    /// This property contains a telephone of an customer
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
    /// This property contains an email of an customer
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

    private bool _Current = true;
    /// <summary>
    /// This property indicates whether an customer is current.
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
    /// This property contains the customer percentage discount rate for all licenses.
    /// If this value is null or zero, the customner is using the default rate.
    /// </summary>
    /// <remarks>
    /// If this value is null or zero, the customner is using the default rate.
    /// 
    /// Note: the custom service rates over-ride the percentage discount.
    /// </remarks>
    public float CustomDiscountPercentage { get; set; }

    /// <summary>
    /// This property contains the customer lite service rate for this customer.
    /// If this value is null or zero, the customner is using the default rate.
    /// </summary>
    public float CustomLiteServiceRate { get; set; }

    /// <summary>
    /// This property contains the customer standard service rate for this customer.
    /// If this value is null or zero, the customner is using the default rate.
    /// </summary>
    public float CustomStandardServiceRate { get; set; }

    /// <summary>
    /// This property contains the customer enhanced service rate for this customer.
    /// If this value is null or zero, the customner is using the default rate.
    /// </summary>
    public float CustomEnhancedServiceRate { get; set; }
    private List<EvModuleCodes> _LoadedModuleList = new List<EvModuleCodes> ( );

    /// <summary>
    /// This property contains used by this customer.
    /// </summary>
    public string LoadedModules
    {
      get
      {
        string loadedModules = String.Empty;

        //
        // Iterate through the list of modules to ensure that they are all processes.
        //
        foreach ( EvModuleCodes code in this._LoadedModuleList )
        {
          if ( loadedModules.Contains ( code.ToString ( ) ) == false )
          {
            if ( loadedModules != String.Empty )
            {
              loadedModules += ";";
            }
            loadedModules += code.ToString ( );
          }
        }//END moduleList iteration loop
        return loadedModules;
      }
      set
      {
        //
        // Initialise the methods variable and objects.
        //
        string [ ] arModuules = value.Split ( ';' );
        this._LoadedModuleList = new List<EvModuleCodes> ( );

        //
        // Iterate through the list of modules enumerations.
        //
        for ( int i = 0; i < arModuules.Length; i++ )
        {
          EvModuleCodes name = EvModuleCodes.Null;

          if ( arModuules [ i ] == "Patient_Recorded_Outcomes" )
          {
            arModuules [ i ] = "Clinical_Outcome_Assessments";
          }

          if ( arModuules [ i ] == "Patient_Recorded_Outcomes" )
          {
            arModuules [ i ] = "Patient_Recorded_Observation";
          }

          if ( EvcStatics.Enumerations.tryParseEnumValue<EvModuleCodes> ( arModuules [ i ], out name ) == true )
          {
            this.addModule ( name );
          }
        }
      }
    }

    /// <summary>
    /// This property returns the modules as an array of string objects.
    /// </summary>
    public List<EvModuleCodes> LoadedModuleList
    {
      get
      {
        return this._LoadedModuleList;
      }
      set
      {
        this._LoadedModuleList = value;
      }
    }

    private string _HideSubjectFields = String.Empty;
    /// <summary>
    /// This property contains hide subject fields of site profile
    /// </summary>
    public string HideSubjectFields
    {
      get
      {
        return this._HideSubjectFields;
      }
      set
      {
        this._HideSubjectFields = value;
      }
    }

    /// <summary>
    /// This property contains a summary of an customer
    /// </summary>
    public string LinkText
    {
      get
      {
        String stLinkText = this.CustomerNo
          + EvLabels.Space_Hypen
          + this.Name;

        if ( this._ServiceType !=  ServiceTypes.Null )
        {
          stLinkText += EvLabels.Space_Coma
            + EvLabels.Customer_Service_Text
            + this.ServiceType;
        }

        if ( this._State != CustomerStates.Null )
        {
          stLinkText += EvLabels.Space_Coma
            + EvLabels.Label_Status
            + this.StateDesc;
        }

        return stLinkText;
      }
    }


    private bool _IsSingleStudy = true;
    /// <summary>
    /// This property indicates that this customer has a single study.
    /// </summary>
    public bool IsSingleStudy
    {
      get
      {
        return this._IsSingleStudy;
      }
      set
      {
        this._IsSingleStudy = value;
      }
    }

    private int _NoOfStudies = 1;
    /// <summary>
    /// This property contains a number of studies the customer is running.
    /// </summary>
    public int NoOfStudies
    {
      get
      {
        return this._NoOfStudies;
      }
      set
      {
        this._NoOfStudies = value;
      }
    }

    private ServiceTypes _ServiceType = ServiceTypes.Lite;
    /// <summary>
    /// This property contains the customer service type.
    /// </summary>
    public ServiceTypes ServiceType
    {
      get { return _ServiceType; }
      set { _ServiceType = value; }
    }

    private CustomerStates _State = CustomerStates.Null;
    /// <summary>
    /// This property contains a state object of an customer
    /// </summary>
    public CustomerStates State
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
    /// This property contains a state description of an customer
    /// </summary>
    public string StateDesc
    {
      get
      {
        return EvcStatics.Enumerations.enumValueToString ( this._State );
      }
      set
      {
        string v = value;
      }
    }

    private String _AdsGroup = String.Empty;
    /// <summary>
    /// This property contains the ADS Group name for the customer
    /// </summary>
    public String AdsGroupName
    {
      get
      {
        return this._AdsGroup;
      }
      set
      {
        this._AdsGroup = value;
      }
    }

    private String _HomePageHeader = String.Empty;
    /// <summary>
    /// This property contains the home header text for the customer
    /// </summary>
    public String HomePageHeader
    {
      get
      {
        return this._HomePageHeader;
      }
      set
      {
        this._HomePageHeader = value;
      }
    }

    private List<EvUserSignoff> _Signoffs = new List<EvUserSignoff> ( );
    /// <summary>
    /// This property contains a user sign off object list of an customer
    /// </summary>
    public List<EvUserSignoff> Signoffs
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

    private string _Deleted = String.Empty;
    /// <summary>
    /// This property indicates that the customer has been deleted from the system.
    /// </summary>
    public string Deleted
    {
      get { return _Deleted; }
      set { _Deleted = value; }
    }

    private ActionCodes _Action = ActionCodes.Null;
    /// <summary>
    /// This property contains save action setting for the object.
    /// </summary>
    public ActionCodes Action
    {
      get { return _Action; }
      set { _Action = value; }
    }
    private string _UpdatedBy = String.Empty;
    /// <summary>
    /// This property contains an updated string of an customer
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

    private DateTime _UpdatedDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains the updated date 
    /// </summary>
    public DateTime UpdatedDate
    {
      get { return _UpdatedDate; }
      set { _UpdatedDate = value; }
    }

    private string _UserCommonName = String.Empty;
    /// <summary>
    /// This property contains a user common name who updates an customer
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

    private string _UpdatedByUserId = String.Empty;
    /// <summary>
    /// This property contains a user identifier of those who updates an customer
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

    #endregion

    #region public methods

    ///  =================================================================================
    /// <summary>
    /// Description:
    ///   This method tests whether the passed module enumeration is currently loaded.
    /// 
    /// </summary>
    /// <param name="Module"></param>
    /// <returns></returns>
    //  ---------------------------------------------------------------------------------
    public void addModule ( EvModuleCodes Module )
    {
      // 
      // Iterate through the list to look for a matching module.
      // 
      foreach ( EvModuleCodes module in this.LoadedModuleList )
      {
        if ( module == Module
          || Module == EvModuleCodes.Null )
        {
          return;
        }
      }

      this.LoadedModuleList.Add ( Module );

    }//END hadModule method.

    ///  =================================================================================
    /// <summary>
    /// Description:
    ///   This method tests whether the passed module enumeration is currently loaded.
    /// 
    /// </summary>
    /// <param name="Module"></param>
    /// <returns></returns>
    //  ---------------------------------------------------------------------------------
    public bool hasModule ( EvModuleCodes Module )
    {
      // 
      // Iterate through the list to look for a matching module.
      // 
      foreach ( EvModuleCodes module in this.LoadedModuleList )
      {
        if ( module == Module )
        {
          return true;
        }
      }

      return false;

    }//END hadModule method.

    ///  =================================================================================
    /// <summary>
    /// Description:
    ///   This method tests whether the passed module enumeration is currently loaded.
    /// 
    /// </summary>
    /// <param name="ModuleList">Array of string containing module enumerated values.</param>
    /// <returns>Bool: True module exists in both lists.</returns>
    //  ---------------------------------------------------------------------------------
    public bool hasModule ( String [ ] ModuleList )
    {
      // 
      // Iterate through the list to look for a matching module.
      // 
      foreach ( string module1 in ModuleList )
      {
        string module = module1.Trim ( );

        if ( module == EvModuleCodes.All_Modules.ToString ( ) )
        {
          return true;
        }

        foreach ( EvModuleCodes module2 in this.LoadedModuleList )
        {
          if ( module == module2.ToString ( ) )
          {
            return true;
          }
        }
      }

      return false;

    }//END hadModule method.

    // ==================================================================================
    /// <summary>
    /// This method provides a list of trial types.
    /// </summary>
    /// <param name="FieldName">EvCustomer.CustomerFieldNames: a Field Name object</param>
    /// <returns>string: a string value</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Switch FieldName and get value defining by the customer field name.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public string getValue ( EvCustomer.CustomerFieldNames FieldName )
    {
      //
      // Switch FieldName and get value defining by the customer field name.
      //
      switch ( FieldName )
      {

        case EvCustomer.CustomerFieldNames.Customer_No:
          {
            return this.CustomerNo.ToString ( );
          }
        case EvCustomer.CustomerFieldNames.Name:
          {
            return this.Name;
          }

        case EvCustomer.CustomerFieldNames.Address:
          {
            return this.Address;
          }

        case EvCustomer.CustomerFieldNames.Administrator:
          {
            return this.Administrator;
          }

        case EvCustomer.CustomerFieldNames.AdminEmailAddress:
          {
            return this.AdminEmailAddress;
          }

        case EvCustomer.CustomerFieldNames.Address_1:
          {
            return this.AddressStreet_1;
          }

        case EvCustomer.CustomerFieldNames.Address_2:
          {
            return this.AddressStreet_2;
          }

        case EvCustomer.CustomerFieldNames.Address_City:
          {
            return this.AddressCity;
          }

        case EvCustomer.CustomerFieldNames.Address_State:
          {
            return this.AddressState;
          }

        case EvCustomer.CustomerFieldNames.Address_Post_Code:
          {
            return this.AddressPostCode;
          }

        case EvCustomer.CustomerFieldNames.Address_Country:
          {
            return this.AddressCountry;
          }

        case EvCustomer.CustomerFieldNames.Telephone:
          {
            return this._Telephone;
          }

        case EvCustomer.CustomerFieldNames.Email_Address:
          {
            return this._EmailAddress;
          }

        case EvCustomer.CustomerFieldNames.Current:
          {
            return this._Current.ToString ( );
          }
        case EvCustomer.CustomerFieldNames.State:
          {
            return this.StateDesc;
          }
        case EvCustomer.CustomerFieldNames.Ads_Group:
          {
            return this.AdsGroupName;
          }
        case EvCustomer.CustomerFieldNames.Home_Page_Header:
          {
            return this.HomePageHeader;
          }
        case EvCustomer.CustomerFieldNames.No_Of_Studies:
          {
            return this.NoOfStudies.ToString ( );
          }
        case EvCustomer.CustomerFieldNames.Is_Single_Study:
          {
            return this.IsSingleStudy.ToString ( );
          }
        case EvCustomer.CustomerFieldNames.Service_Type:
          {
            return this.ServiceType.ToString ( );
          }
        default:
          {
            return String.Empty;
          }

      }//END Switch

    }//END getValue method

    // ==================================================================================
    /// <summary>
    /// This method sets the field value.
    /// </summary>
    /// <param name="FieldName">EvCustomer.CustomerFieldNames: a field name object</param>
    /// <param name="Value">string: a Value for updating</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize the internal variables
    /// 
    /// 2. Switch the FieldName and update the Value on the customer field names.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void setValue ( EvCustomer.CustomerFieldNames FieldName, string Value )
    {
      //
      // Switch the FieldName and update the Value on the customer field names.
      //
      switch ( FieldName )
      {

        case EvCustomer.CustomerFieldNames.Customer_No:
          {
            this.CustomerNo = int.Parse ( Value );
            return;
          }
        case EvCustomer.CustomerFieldNames.Name:
          {
            this._Name = Value;
            return;
          }
        case EvCustomer.CustomerFieldNames.Address:
          {
            this.Address = Value;
            return;
          }
        case EvCustomer.CustomerFieldNames.Administrator:
          {
            this.Administrator = Value;
            return;
          }
        case EvCustomer.CustomerFieldNames.AdminEmailAddress:
          {
            this.AdminEmailAddress = Value;
            return;
          }
        case EvCustomer.CustomerFieldNames.Address_1:
          {
            this._AddressStreet_1 = Value;
            return;
          }
        case EvCustomer.CustomerFieldNames.Address_2:
          {
            this._AddressStreet_2 = Value;
            return;
          }
        case EvCustomer.CustomerFieldNames.Address_City:
          {
            this._AddressCity = Value;
            return;
          }
        case EvCustomer.CustomerFieldNames.Address_State:
          {
            this._AddressState = Value;
            return;
          }
        case EvCustomer.CustomerFieldNames.Address_Post_Code:
          {
            this._AddressPostCode = Value;
            return;
          }
        case EvCustomer.CustomerFieldNames.Address_Country:
          {
            this.AddressCountry = Value;
            return;
          }
        case EvCustomer.CustomerFieldNames.Telephone:
          {
            this._Telephone = Value;
            return;
          }
        case EvCustomer.CustomerFieldNames.Email_Address:
          {
            this._EmailAddress = Value;
            return;
          }

        case EvCustomer.CustomerFieldNames.Current:
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

        case EvCustomer.CustomerFieldNames.No_Of_Studies:
          {
            this.NoOfStudies = EvStatics.getInteger ( Value );
            return;
          }
        case EvCustomer.CustomerFieldNames.Is_Single_Study:
          {
            this.IsSingleStudy = EvStatics.getBool ( Value );
            return;
          }
        case EvCustomer.CustomerFieldNames.Service_Type:
          {

            this.ServiceType = EvStatics.Enumerations.parseEnumValue<EvCustomer.ServiceTypes> ( Value );
            return;
          }
        case EvCustomer.CustomerFieldNames.State:
          {

            this.State = EvStatics.Enumerations.parseEnumValue<EvCustomer.CustomerStates> ( Value );
            return;
          }
        case EvCustomer.CustomerFieldNames.Ads_Group:
          {
            this.AdsGroupName = Value;
            return;
          }
        case EvCustomer.CustomerFieldNames.Home_Page_Header:
          {
            this.HomePageHeader = Value;
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
    /// <returns>List of EvOption objects</returns>
    public static List<EvOption> GetServiceList ( bool IncludeEnterprise )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( ) );

      //
      // Add the service options
      //
      optionList.Add ( EvcStatics.Enumerations.getOption (
        ServiceTypes.Lite ) );

      optionList.Add ( EvcStatics.Enumerations.getOption (
        ServiceTypes.Standard ) );

      optionList.Add ( EvcStatics.Enumerations.getOption (
        ServiceTypes.Enhanced ) );

      if ( IncludeEnterprise == true )
      {
        optionList.Add ( EvcStatics.Enumerations.getOption (
          ServiceTypes.Enterprise ) );
      }
      return optionList;

    }//END static GetServiceList method.

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }//END EvCustomer class

}//END namespace Evado.Model.Digital

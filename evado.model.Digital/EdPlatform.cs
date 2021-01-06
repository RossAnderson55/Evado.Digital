/***************************************************************************************
 * <copyright file="EvSiteProfile.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvSiteProfile data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{

  /// <summary>
  /// Business entity used to model SiteProperties
  /// </summary>
  [Serializable]
  public class EdPlatform : Evado.Model.EvParameters
  {
    #region enumerated lists

    /// <summary>
    /// This enumeration list defines the field names of organization
    /// </summary>
    public enum SettingFieldNames
    {
      /// <summary>
      /// This enumeration defines null value or non selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines a Version field name of an application profile
      /// </summary>
      Version,

      /// <summary>
      /// This enumeration defines an SiteLicenseNo field name of an application profile
      /// </summary>
      SiteLicenseNo,

      /// <summary>
      /// This enumeration defines an EarlyWithdrawalOptions field name of an application profile
      /// </summary>
      EarlyWithdrawalOptions,

      /// <summary>
      /// This enumeration defines an DiseaseTypeListOptions field name of an application profile
      /// </summary>
      DiseaseTypeListOptions,

      /// <summary>
      /// This enumeration defines an DiseaseListOptions field name of an application profile
      /// </summary>
      DiseaseListOptions,

      /// <summary>
      /// This enumeration defines a CategoryListOptions field name of an application profile
      /// </summary>
      CategoryListOptions,

      /// <summary>
      /// This enumeration defines a SignoffStatement field name of an application profile
      /// </summary>
      SignoffStatement,

      /// <summary>
      /// This enumeration defines a DisplayVisitResourceTime field name of an application profile
      /// </summary>
      DisplayVisitResourceTime,

      /// <summary>
      /// This enumeration defines an DefaultTrialId field name of an application profile
      /// </summary>
      DefaultTrialId,

      /// <summary>
      /// This enumeration defines a HelpUrl field name of an application profile
      /// </summary>
      HelpUrl,

      /// <summary>
      /// This enumeration defines a RegulatoryReports field name of an application profile
      /// </summary>
      RegulatoryReports,


      /// <summary>
      /// This enumeration defines a DisplayHistory field name of an application profile
      /// </summary>
      DisplayHistory,

      /// <summary>
      /// This enumeration defines a DepersonalisedAcceess field name of an application profile
      /// </summary>
      DepersonalisedAccess,

      /// <summary>
      /// This enumeration defines a LoadedModules field name of an application profile
      /// </summary>
      LoadedModules,

      /// <summary>
      /// This enumeration defines an HideSubjectFields field name of an application profile
      /// </summary>
      HideSubjectFields,

      /// <summary>
      /// This enumeration defines a FdaCompliance field name of an application profile
      /// </summary>
      FdaCompliance,

      /// <summary>
      /// This enumeration defines a MaximumSelectionListLength field name of an application profile
      /// </summary>
      MaximumSelectionListLength,

      /// <summary>
      /// This enumeration defines a OverRideConfig field name of an application profile
      /// </summary>
      OverRideConfig,

      /// <summary>
      /// This enumeration defines a SmtpServerfield name of an application profile
      /// </summary>
      SmtpServer,

      /// <summary>
      /// This enumeration defines a SmtpServerPort field name of an application profile
      /// </summary>
      SmtpServerPort,

      /// <summary>
      /// This enumeration defines a SmtpUserId field name of an application profile
      /// </summary>
      SmtpUserId,

      /// <summary>
      /// This enumeration defines a SmtpPassword field name of an application profile
      /// </summary>
      SmtpPassword,

      /// <summary>
      /// This enumeration defines a EmailAlertTestAddress field name of an application profile
      /// </summary>
      EmailAlertTestAddress,

      /// <summary>
      /// This enumeration defines a ProHiddenFieldse of an application profile
      /// </summary>
      Pro_Hidden_Fields,

      /// <summary>
      /// This enumeration defines a display site dashboard of an application profile
      /// </summary>
      Display_Site_Dashboard,

      /// <summary>
      /// This enumeration defines the Lite trial maximum subject number.
      /// </summary>
      Lite_Max_Subject_No,

      /// <summary>
      /// This enumeration defines the Standard trial maximum subject number.
      /// </summary>
      Standard_Max_Subject_No,

      /// <summary>
      /// This enumeration defines the demonstration account Expiry in days.
      /// </summary>
      DemoAccountExpiryDays,

      /// <summary>
      /// This enumeration defines the demonstration registration page video URL.
      /// </summary>
      DemoRegistrationVideoUrl,
    }


    #endregion

    #region Constants
    /// <summary>
    /// This constant defines null value of global unique identifier
    /// </summary>
    public static readonly string NullGuid = Guid.Empty.ToString ( );

    #endregion

    #region Class Properties

    #region Object identifiers

    private Guid _Guid = Guid.Empty;
    /// <summary>
    /// This property contains a global unique identifier of site profile
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

    private string _ApplicationId = String.Empty;
    /// <summary>
    /// This property contains an application setting UID of site profile
    /// </summary>
    public string ApplicationId
    {
      get
      {
        return this._ApplicationId;
      }
      set
      {
        this._ApplicationId = value;
      }
    }

    private String _Version = String.Empty;
    /// <summary>
    /// This property contains version of site profile
    /// </summary>
    public String Version
    {
      get { return _Version; }
      set
      {
        _Version = value;
      }
    }

    private String _MinorVersion = String.Empty;
    /// <summary>
    /// This property contains version of site profile
    /// </summary>
    public String MinorVersion
    {
      get { return _MinorVersion; }
      set
      {
        _MinorVersion = value;
      }
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Object Update

    private string _EarlyWithdrawalOptions = String.Empty;
    /// <summary>
    /// This property contains early withdrawal options string of site profile
    /// </summary>
    public string EarlyWithdrawalOptions
    {
      get
      {
        return this._EarlyWithdrawalOptions;
      }
      set
      {
        this._EarlyWithdrawalOptions = value;
      }
    }

    private string _DiseaseTypeListOptions = String.Empty;
    /// <summary>
    /// This property contains disease type list options string of site profile
    /// </summary>
    public string DiseaseTypeListOptions
    {
      get
      {
        return this._DiseaseTypeListOptions;
      }
      set
      {
        this._DiseaseTypeListOptions = value;
      }
    }

    private string _DiseaseListOptions = String.Empty;
    /// <summary>
    /// This property contains disease list options string of site profile
    /// </summary>
    public string DiseaseListOptions
    {
      get
      {
        return this._DiseaseListOptions;
      }
      set
      {
        this._DiseaseListOptions = value; ;
      }
    }

    private string _CategoryListOptions = String.Empty;
    /// <summary>
    /// This property contains category list options string of site profile
    /// </summary>
    public string CategoryListOptions
    {
      get
      {
        return this._CategoryListOptions;
      }
      set
      {
        this._CategoryListOptions = value;
      }
    }

    private string _SignoffStatement = String.Empty;
    /// <summary>
    /// This property contains the signoff statement of site profile
    /// </summary>
    public string SignoffStatement
    {
      get
      {
        return this._SignoffStatement;
      }
      set
      {
        this._SignoffStatement = value;
      }
    }

    private string _HelpUrl = String.Empty;
    /// <summary>
    /// This property contains a help url of site profile
    /// </summary>
    public string HelpUrl
    {
      get
      {
        return this._HelpUrl;
      }
      set
      {
        this._HelpUrl = value;

        this.setParameter ( SettingFieldNames.HelpUrl, EvDataTypes.Text, value );
      }
    }

    private string _RegulatoryReports = String.Empty;
    /// <summary>
    /// This property contains regulatory reports of site profile
    /// </summary>
    public string RegulatoryReports
    {
      get
      {
        return this._RegulatoryReports;
      }
      set
      {
        this._RegulatoryReports = value;
      }
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region EVADO environment configuration

    private List<EvModuleCodes> _LoadedModuleList = new List<EvModuleCodes> ( );
    /// <summary>
    /// This property contains loaded modules of site profile
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
    /// This property contains Lite trial maximum subject no
    /// </summary>
    public int LiteMaxSubjectNo
    {
      get
      {
        var value = this.getParameter ( SettingFieldNames.Lite_Max_Subject_No.ToString ( ) );

        return EvStatics.getInteger ( value );
      }
      set
      {
        this.setParameter ( SettingFieldNames.Lite_Max_Subject_No, EvDataTypes.Integer, value.ToString ( ) );
      }
    }

    /// <summary>
    /// This property contains standard trial maximum subject no
    /// </summary>
    public int StandardMaxSubjectNo
    {
      get
      {
        var value = this.getParameter ( SettingFieldNames.Standard_Max_Subject_No.ToString ( ) );

        return EvStatics.getInteger ( value );
      }
      set
      {

        this.setParameter ( SettingFieldNames.Standard_Max_Subject_No, EvDataTypes.Integer, value.ToString ( ) );
      }
    }

    /// <summary>
    /// This property contains demonstration account expiry in days
    /// </summary>
    public int DemoAccountExpiryDays
    {
      get
      {
        var value = this.getParameter ( SettingFieldNames.DemoAccountExpiryDays.ToString ( ) );

        return EvStatics.getInteger ( value );
      }
      set
      {

        this.setParameter ( SettingFieldNames.DemoAccountExpiryDays, EvDataTypes.Integer, value.ToString ( "00" ) );
      }
    }

    /// <summary>
    /// This property contains demonstration account expiry in days
    /// </summary>
    public String DemoRegistrationVideoUrl
    {
      get
      {
        return this.getParameter ( SettingFieldNames.DemoRegistrationVideoUrl.ToString ( ) );

      }
      set
      {

        this.setParameter ( SettingFieldNames.DemoRegistrationVideoUrl, EvDataTypes.Text, value );
      }
    }

    private int _MaximumSelectionListLength = 100;
    /// <summary>
    /// This property contains maximum selection list lenght of site profile
    /// </summary>
    public int MaximumSelectionListLength
    {
      get
      {
        return this._MaximumSelectionListLength;
      }
      set
      {
        this._MaximumSelectionListLength = value;
      }
    }

    private bool _OverRideConfig = false;
    /// <summary>
    /// This property indicates whether the site profile has over rigde configuration
    /// </summary>
    public bool OverRideConfig
    {
      get
      {
        return this._OverRideConfig;
      }
      set
      {
        this._OverRideConfig = value;
      }
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region SMTP configuration parameters

    private string _SmtpServer = String.Empty;
    /// <summary>
    /// This property contains smtp server address of site profile
    /// </summary>
    public string SmtpServer
    {
      get
      {
        return this._SmtpServer;
      }
      set
      {
        this._SmtpServer = value;

        this.setParameter ( SettingFieldNames.SmtpServer, EvDataTypes.Text, value );
      }
    }

    private int _SmtpServerPort = 25;
    /// <summary>
    /// This property contains smtp server port of site profile
    /// </summary>
    public int SmtpServerPort
    {
      get
      {
        return this._SmtpServerPort;
      }
      set
      {
        this._SmtpServerPort = value;

        this.setParameter ( SettingFieldNames.SmtpServerPort, EvDataTypes.Integer, value.ToString ( ) );
      }
    }

    private string _SmtpUserId = String.Empty;
    /// <summary>
    /// This property contains smtp user identifier of site profile
    /// </summary>
    public string SmtpUserId
    {
      get
      {
        return this._SmtpUserId;
      }
      set
      {
        this._SmtpUserId = value;

        this.setParameter ( SettingFieldNames.SmtpUserId, EvDataTypes.Text, value );
      }
    }

    private string _SmtpPassword = String.Empty;
    /// <summary>
    /// This property contains smtp password of site profile
    /// </summary>
    public string SmtpPassword
    {
      get
      {
        return this._SmtpPassword;
      }
      set
      {
        this._SmtpPassword = value;

        this.setParameter ( SettingFieldNames.SmtpPassword, EvDataTypes.Text, value );
      }
    }

    private String _EmailAlertTestAddress = String.Empty;
    /// <summary>
    /// This property contains email alert test address of site profile
    /// </summary>
    public String EmailAlertTestAddress
    {
      get { return _EmailAlertTestAddress; }
      set
      {
        _EmailAlertTestAddress = value;

        this.setParameter ( SettingFieldNames.EmailAlertTestAddress, EvDataTypes.Text, value );
      }
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region PRO configuration

    private const string CONST_PRO_HIDDEN_FIELDS = "PRO_HIDDEN_FIELDS";
    /// <summary>
    /// Ths property contains PRO hidden field parameter.
    /// </summary>
    public String ProHiddenFields
    {
      get
      {
        return this.getParameter ( SettingFieldNames.Pro_Hidden_Fields );

      }
      set
      {
        this.setParameter ( SettingFieldNames.Pro_Hidden_Fields, EvDataTypes.Text, value.ToString ( ) );
      }
    }

    /// <summary>
    /// Ths property contains PRO hidden field parameter.
    /// </summary>
    public bool DisplaySiteDashboard
    {
      get
      {
        string value = this.getParameter ( SettingFieldNames.Display_Site_Dashboard );

        return EvStatics.getBool ( value );

      }
      set
      {
        this.setParameter ( SettingFieldNames.Pro_Hidden_Fields, EvDataTypes.Boolean, value.ToString ( ) );
      }
    }

    #endregion

    #region Object Update

    private string _UpdateLog = String.Empty;
    /// <summary>
    /// This property contains update log of site profile
    /// </summary>
    public string UpdateLog
    {
      get
      {
        return this._UpdateLog;
      }
      set
      {
        this._UpdateLog = value;
      }
    }

    private string _UserCommonName = String.Empty;
    /// <summary>
    /// This property contains user common name of those who updates site profile
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

    private string _UserId = String.Empty;
    /// <summary>
    /// This property contains user identifier of those who updates site profile
    /// </summary>
    public string UserId
    {
      get
      {
        return this._UserId;
      }
      set
      {
        this._UserId = value;
      }
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    /// <summary>
    /// This property contains a list of application parameters.
    /// </summary>
    //public List<EvObjectParameter> Parameters { get; set; }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Methods

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
    /// This method sets the field value.
    /// </summary>
    /// <param name="FieldName">EvApplicationProfile.ApplicationProfileFieldNames: a field name object</param>
    /// <param name="Value">string: a Value for updating</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize the internal variables
    /// 
    /// 2. Switch the FieldName and update the Value on the organization field names.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void setValue (
      EdPlatform.SettingFieldNames FieldName,
      String Value )
    {
      //
      // Initialize the internal variables
      //
      DateTime date = EvcStatics.CONST_DATE_NULL;
      //float fltValue = 0;
      int intValue = 0;
      Guid guidValue = Guid.Empty;

      //
      // Switch the FieldName and update the Value on the organization field names.
      //
      switch ( FieldName )
      {
        case EdPlatform.SettingFieldNames.Version:
          {
            this.Version = Value;
            return;
          }

        case EdPlatform.SettingFieldNames.EarlyWithdrawalOptions:
          {
            Value = Value.Replace ( "\r\n", String.Empty );
            Value = Value.Replace ( ",", String.Empty );
            Value = Value.Replace ( "; ", ";" );
            this.EarlyWithdrawalOptions = Value;
            return;
          }

        case EdPlatform.SettingFieldNames.DiseaseTypeListOptions:
          {
            Value = Value.Replace ( "\r\n", String.Empty );
            Value = Value.Replace ( ",", String.Empty );
            Value = Value.Replace ( "; ", ";" );
            this.DiseaseTypeListOptions = Value;
            return;
          }

        case EdPlatform.SettingFieldNames.DiseaseListOptions:
          {
            Value = Value.Replace ( "\r\n", String.Empty );
            Value = Value.Replace ( ",", String.Empty );
            Value = Value.Replace ( "; ", ";" );
            this.DiseaseListOptions = Value;
            return;
          }

        case EdPlatform.SettingFieldNames.CategoryListOptions:
          {
            Value = Value.Replace ( "\r\n", String.Empty );
            Value = Value.Replace ( ",", String.Empty );
            Value = Value.Replace ( "; ", ";" );
            this.CategoryListOptions = Value;
            return;
          }

        case EdPlatform.SettingFieldNames.SignoffStatement:
          {
            this.SignoffStatement = Value;
            return;
          }

        case EdPlatform.SettingFieldNames.LoadedModules:
          {
            this.LoadedModules = Value;
            this.LoadedModules = this.LoadedModules.Replace ( ",", ";" );
            return;
          }

        case EdPlatform.SettingFieldNames.HideSubjectFields:
          {
            this.HideSubjectFields = Value;
            return;
          }

        case EdPlatform.SettingFieldNames.Lite_Max_Subject_No:
          {
            this.setParameter ( SettingFieldNames.Lite_Max_Subject_No, EvDataTypes.Integer, Value );
            return;
          }

        case EdPlatform.SettingFieldNames.Standard_Max_Subject_No:
          {
            this.setParameter ( SettingFieldNames.Standard_Max_Subject_No, EvDataTypes.Integer, Value );
            return;
          }

        case EdPlatform.SettingFieldNames.MaximumSelectionListLength:
          {
            this.MaximumSelectionListLength = EvcStatics.getInteger ( Value );
            return;
          }

        case EdPlatform.SettingFieldNames.DemoAccountExpiryDays:
          {
            this.DemoAccountExpiryDays = EvcStatics.getInteger ( Value );
            return;
          }
        case EdPlatform.SettingFieldNames.DemoRegistrationVideoUrl:
          {
            this.DemoRegistrationVideoUrl = Value;
            return;
          }

        case EdPlatform.SettingFieldNames.OverRideConfig:
          {
            this.OverRideConfig = EvcStatics.getBool ( Value );
            return;
          }

        case EdPlatform.SettingFieldNames.Pro_Hidden_Fields:
          {
            this.ProHiddenFields = Value;
            return;
          }

        case EdPlatform.SettingFieldNames.Display_Site_Dashboard:
          {
            this.DisplaySiteDashboard = EvStatics.getBool ( Value );
            return;
          }

        case EdPlatform.SettingFieldNames.SmtpServer:
          {
            this.SmtpServer = Value;
            return;
          }

        case EdPlatform.SettingFieldNames.SmtpServerPort:
          {
            if ( int.TryParse ( Value, out intValue ) == true )
            {
              this.SmtpServerPort = intValue;
            }
            return;
          }

        case EdPlatform.SettingFieldNames.SmtpUserId:
          {
            this.SmtpUserId = Value;
            return;
          }

        case EdPlatform.SettingFieldNames.SmtpPassword:
          {
            this.SmtpPassword = Value;
            return;
          }

        case EdPlatform.SettingFieldNames.EmailAlertTestAddress:
          {
            this.EmailAlertTestAddress = Value;
            return;
          }
        default:

          return;

      }//END Switch

    }//END setValue method 

    // ==================================================================================
    /// <summary>
    /// This class returns the module lists as a selection option arraylist.    
    /// </summary>
    /// <param name="IsSelection">Boolean: true, if option is selected</param>
    /// <returns>ArrayList: a list of disease activity options</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list, a disease type array and an option object
    /// 
    /// 2. Validate whether the option is select.
    /// 
    /// 3. Loop through the disease type array
    /// 
    /// 4. Append the type array value to option object
    /// 
    /// 5. Add option items to the return list.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public ArrayList getDiseaseTypeListOptions ( bool IsSelection )
    {
      // 
      // Initialise local variables and objects.
      // 
      ArrayList list = new ArrayList ( );
      string [ ] arrDiseaseTypes = this.DiseaseTypeListOptions.Split ( ';' );
      EvOption option = new EvOption ( );

      // 
      // If a selection list then add a null option for the first element.
      // 
      if ( IsSelection == true )
      {
        list.Add ( option );
      }

      // 
      // Iterate through the module list generating the arraylist.
      // 
      foreach ( string disease in arrDiseaseTypes )
      {
        if ( disease != String.Empty )
        {
          option = new EvOption ( disease.Trim ( ), disease.Trim ( ) );
          list.Add ( option );
        }
      }//END iteration loop

      // 
      //Return the completed Array List.
      //
      return list;

    }//END getDiseaseCategoryList method

    // ==================================================================================
    /// <summary>
    /// This class returns the module lists as a selection option arraylist.
    /// </summary>
    /// <param name="IsSelection">Boolean: true, if option is selected</param>
    /// <returns>ArrayList: a list of disease activity options</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list, a disease array and an option object
    /// 
    /// 2. Validate whether the option is select.
    /// 
    /// 3. Loop through the disease array
    /// 
    /// 4. Append the disease array value to option object
    /// 
    /// 5. Add option items to the return list.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public ArrayList getDiseaseOptionList ( bool IsSelection )
    {
      // 
      // Initialise local variables and objects.
      // 
      ArrayList list = new ArrayList ( );
      string [ ] arrDiseases = this.DiseaseListOptions.Split ( ';' );
      EvOption option = new EvOption ( );
      // 
      // If a selection list then add a null option for the first element.
      // 
      if ( IsSelection == true )
      {
        list.Add ( option );
      }
      // 
      // Iterate through the module list generating the arraylist.
      // 
      foreach ( string disease in arrDiseases )
      {
        if ( disease != String.Empty )
        {
          option = new EvOption ( disease.Trim ( ), disease.Trim ( ) );
          list.Add ( option );
        }
      }
      // 
      //Return the completed Array List.
      //
      return list;

    }//END getDiseaseOptionList method

    // ==================================================================================
    /// <summary>
    /// This class returns the module lists as a selection option arraylist.
    /// </summary>
    /// <param name="IsSelection">Boolean: true, if option is selected</param>
    /// <returns>ArrayList: a list of disease activity options</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list, a category array and an option object
    /// 
    /// 2. Validate whether the option is select.
    /// 
    /// 3. Loop through the category array
    /// 
    /// 4. Append the category array value to option object
    /// 
    /// 5. Add option items to the return list.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public ArrayList getCategoryOptionList ( bool IsSelection )
    {
      // 
      // Initialise local variables and objects.
      // 
      ArrayList list = new ArrayList ( );
      string [ ] arrCategories = this.CategoryListOptions.Split ( ';' );
      EvOption option = new EvOption ( );
      // 
      // If a selection list then add a null option for the first element.
      // 
      if ( IsSelection == true )
      {
        list.Add ( option );
      }

      // 
      // Iterate through the module list generating the arraylist.
      // 
      foreach ( string category in arrCategories )
      {
        if ( category != String.Empty )
        {
          option = new EvOption ( category, category );
          list.Add ( option );
        }
      }//END iteration loop

      // 
      //Return the completed Array List.
      //
      return list;

    }//END getCategoryOptionList method

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class static Methods

    //  =================================================================================
    /// <summary>
    /// This method returns a list of field data types.
    /// </summary>
    /// <param name="DisplayAllModules">Bool: True display all selection.</param>
    /// <returns>List of EvOption: a list of data types.</returns>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getModuleList ( )
    {
      //
      // Initialize a return list and an option object.
      //
      List<EvOption> list = new List<EvOption> ( );

      list.Add ( EvStatics.Enumerations.getOption ( Evado.Model.Digital.EvModuleCodes.All_Modules ) );

      list.Add ( EvStatics.Enumerations.getOption ( Evado.Model.Digital.EvModuleCodes.Administration_Module ) );

      list.Add ( EvStatics.Enumerations.getOption ( Evado.Model.Digital.EvModuleCodes.Management_Module ) );

      list.Add ( EvStatics.Enumerations.getOption ( Evado.Model.Digital.EvModuleCodes.Design_Module ) );

      list.Add ( EvStatics.Enumerations.getOption ( Evado.Model.Digital.EvModuleCodes.Entity_Module ) );

      list.Add ( EvStatics.Enumerations.getOption ( Evado.Model.Digital.EvModuleCodes.Record_Module ) );

      list.Add ( EvStatics.Enumerations.getOption ( Evado.Model.Digital.EvModuleCodes.Imaging_Module ) );

      list.Add ( EvStatics.Enumerations.getOption ( Evado.Model.Digital.EvModuleCodes.Integration_Module ) );

      return list;

    }//END getDataTypes methd

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }//END SiteProperties class

}//END namespace Evado.Model.Digital

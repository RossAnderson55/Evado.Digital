/***************************************************************************************
 * <copyright file="EvSiteProfile.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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
  public class EdAdapterSettings : Evado.Model.EvParameters
  {
    #region enumerated lists

    /// <summary>
    /// This enumeration list defines the field names of organization
    /// </summary>
    public enum AdapterFieldNames
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
      /// This enumeration defines a HelpUrl field name of an application profile
      /// </summary>
      HelpUrl,

      /// <summary>
      /// This enumeration defines a MaximumSelectionListLength field name of an application profile
      /// </summary>
      MaximumSelectionListLength,

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
      /// This enumeration defines the demonstration account Expiry in days.
      /// </summary>
      DemoAccountExpiryDays,

      /// <summary>
      /// This enumeration defines the demonstration registration page video URL.
      /// </summary>
      DemoRegistrationVideoUrl,

      /// <summary>
      /// This enumeration defines a state field name of an customer
      /// </summary>
      State,

      /// <summary>
      /// This enumeration defines a ADS Group customer field
      /// </summary>
      Ads_Group,

      /// <summary>
      /// This enumeration defines a Home page header text customer field
      /// </summary>
      Home_Page_Header,

      /// <summary>
      /// This enumeration define a title field name of a trial
      /// </summary>
      Title,

      /// <summary>
      /// This enumeration define a reference field name of a trial
      /// </summary>
      Http_Reference,

      /// <summary>
      /// This enumeration defines a description field name of a trial
      /// </summary>
      Description,

      /// <summary>
      /// This enumeration definse a collecting binary data field name of a trial
      /// </summary>
      Enable_Binary_Data,

      /// <summary>
      /// This enumeration defines if the user profile update can update the user's organisation details.
      /// </summary>
      Enable_User_Organisation_Update,

      /// <summary>
      /// This enumeration defines if the enables edit button to update entities details.
      /// </summary>
      Enable_Entity_Edit_Button_Update,

      /// <summary>
      /// This enumeration defines if the enables save button to update entities details.
      /// Submit button is there by default.
      /// </summary>
      Enable_Entity_Save_Button_Update,

      /// <summary>
      /// This enumeration defines if the user address is to be collected.
      /// </summary>
      Enable_User_Address_Update,
      
      /// <summary>
      /// This enumeration indicates that the admin group is to be displayed
      /// to administrators on all entity pages.
      /// </summary>
      EnableAdminGroupOnEntitPages,

      /// <summary>
      /// This enumeration definse a collecting binary data field name of a trial
      /// </summary>
      Use_Home_Page_Header_On_All_Pages,

      /// <summary>
      /// this enumeration defines the roles in the application.
      /// </summary>
      User_Roles,

      /// <summary>
      /// this enumeration defines the default user role field in the application
      /// </summary>
      Default_User_Roles,

      /// <summary>
      /// this enumeration defines the user category list for the application
      /// </summary>
      User_Category_List,

      /// <summary>
      /// This enumeration defines the OrgTypes in the application
      /// </summary>
      Org_Types,

      /// <summary>
      /// This enumeration defines the Entity Query filter static options.
      /// </summary>
      Static_Query_Filter_Options,

      /// <summary>
      /// This enumeration defines the default organisation type
      /// </summary>
      Default_User_Org_Type,

      /// <summary>
      /// this enumeration defines the hidden user fields the application.
      /// </summary>
      Hidden_User_Fields,

      /// <summary>
      /// this enumeration defines the hidden user fields the application.
      /// </summary>
      Hidden_Organisation_Fields,

      /// <summary>
      /// this enumeration defines the primary entity that will be created when the user registers in the envronment.
      /// </summary>
      User_Primary_Entity,

      /// <summary>
      /// this enumeration indicates if a new organisation is created when a user registes in the platform.
      /// </summary>
      Create_Organisation_On_User_Registration,

      /// <summary>
      /// this enumeration defines the primary entity that will be create when the user creates a new organisation..
      /// </summary>
      Organisation_Primary_Entity,

    }

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

    #region class constants and objects

    public const string EVADO_ORGANISATION = "Evado";
    #endregion

    #region Object Update

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

        this.setParameter ( AdapterFieldNames.HelpUrl, EvDataTypes.Text, value );
      }
    }


    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Application settings

    private string _HomePageHeaderText = String.Empty;
    /// <summary>
    /// This property contains the Home page header text.   
    /// </summary>
    public string HomePageHeaderText
    {
      get
      {
        return this._HomePageHeaderText;
      }
      set
      {
        this._HomePageHeaderText = value;
      }
    }

    private string _Title = String.Empty;
    /// <summary>
    /// This property contains the trial title.
    /// </summary>
    public string Title
    {
      get
      {
        return this._Title;
      }
      set
      {
        this._Title = value;
      }
    }

    private string _HttpReference = String.Empty;
    /// <summary>
    /// This property contains the HTTP reference link for the application.
    /// </summary>
    public string HttpReference
    {
      get
      {
        return
          this.getParameter ( EdAdapterSettings.AdapterFieldNames.Http_Reference );
      }
      set
      {
        this.setParameter ( EdAdapterSettings.AdapterFieldNames.Http_Reference,
          EvDataTypes.Text, value.ToString ( ) );
      }
    }

    private string _Description = String.Empty;
    /// <summary>
    /// This property contains a description of the application.
    /// </summary>
    public string Description
    {
      get
      {
        return this._Description;
      }
      set
      {
        this._Description = value;
      }
    }

    private string _State = String.Empty;
    /// <summary>
    /// This property contains a state of the application.
    /// </summary>
    public string State
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

    // =====================================================================================
    /// <summary>
    /// This property contains the Adapter ADS Group name
    /// </summary>
    // -------------------------------------------------------------------------------------
    public String AdsGroupName
    {
      get
      {
        return this.getParameter ( EdAdapterSettings.AdapterFieldNames.Ads_Group );
      }
      set
      {
        this.setParameter ( EdAdapterSettings.AdapterFieldNames.Ads_Group,
          EvDataTypes.Text, value );
      }
    }

    // =====================================================================================
    /// <summary>
    /// This property indicates to user the home header for all entity and record pages.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public bool UseHomePageHeaderOnAllPages
    {
      get
      {
        return EvStatics.getBool (
          this.getParameter ( EdAdapterSettings.AdapterFieldNames.Use_Home_Page_Header_On_All_Pages ) );
      }
      set
      {
        this.setParameter ( EdAdapterSettings.AdapterFieldNames.Use_Home_Page_Header_On_All_Pages,
          EvDataTypes.Boolean, value.ToString ( ) );
      }
    }

    // =====================================================================================
    /// <summary>
    /// This property indicates to user the home header for all entity and record pages.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public bool EnableAdminGroupOnEntityPages
    {
      get
      {
        return EvStatics.getBool (
          this.getParameter ( EdAdapterSettings.AdapterFieldNames.EnableAdminGroupOnEntitPages) );
      }
      set
      {
        this.setParameter ( EdAdapterSettings.AdapterFieldNames.EnableAdminGroupOnEntitPages,
          EvDataTypes.Boolean, value.ToString ( ) );
      }
    }

    // =====================================================================================
    /// <summary>
    /// This property indicates binary data is being collected.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public bool EnableBinaryData
    {
      get
      {
        return EvStatics.getBool (
          this.getParameter ( EdAdapterSettings.AdapterFieldNames.Enable_Binary_Data ) );
      }
      set
      {
        this.setParameter ( EdAdapterSettings.AdapterFieldNames.Enable_Binary_Data,
          EvDataTypes.Boolean, value.ToString ( ) );
      }
    }

    // =====================================================================================
    /// <summary>
    /// This property if the user can update their organisation deails.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public bool EnableEntityEditButtonUpdate
    {
      get
      {
        return EvStatics.getBool (
          this.getParameter ( EdAdapterSettings.AdapterFieldNames.Enable_Entity_Edit_Button_Update ) );
      }
      set
      {
        this.setParameter ( EdAdapterSettings.AdapterFieldNames.Enable_Entity_Edit_Button_Update,
          EvDataTypes.Boolean, value.ToString ( ) );
      }
    }

    // =====================================================================================
    /// <summary>
    /// This property if the user can update their organisation deails.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public bool EnableEntitySaveButtonUpdate
    {
      get
      {
        return EvStatics.getBool (
          this.getParameter ( EdAdapterSettings.AdapterFieldNames.Enable_Entity_Save_Button_Update ) );
      }
      set
      {
        this.setParameter ( EdAdapterSettings.AdapterFieldNames.Enable_Entity_Save_Button_Update,
          EvDataTypes.Boolean, value.ToString ( ) );
      }
    }

    // =====================================================================================
    /// <summary>
    /// This property if the user can update their organisation deails.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public bool EnableUserOrganisationUpdate
    {
      get
      {
        return EvStatics.getBool (
          this.getParameter ( EdAdapterSettings.AdapterFieldNames.Enable_User_Organisation_Update ) );
      }
      set
      {
        this.setParameter ( EdAdapterSettings.AdapterFieldNames.Enable_User_Organisation_Update,
          EvDataTypes.Boolean, value.ToString ( ) );
      }
    }

    // =====================================================================================
    /// <summary>
    /// This property if the user can update their organisation deails.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public bool EnableUserAddressUpdate
    {
      get
      {
        return EvStatics.getBool (
          this.getParameter ( EdAdapterSettings.AdapterFieldNames.Enable_User_Address_Update ) );
      }
      set
      {
        this.setParameter ( EdAdapterSettings.AdapterFieldNames.Enable_User_Address_Update,
          EvDataTypes.Boolean, value.ToString ( ) );
      }
    }

    /// <summary>
    /// This property contains demonstration account expiry in days
    /// </summary>
    public String StaticQueryFilterOptions
    {
      get
      {
        return this.getParameter ( AdapterFieldNames.Static_Query_Filter_Options );
      }
      set
      {
        this.setParameter ( AdapterFieldNames.Static_Query_Filter_Options, EvDataTypes.Text, value );
      }
    }

    /// <summary>
    /// This property contains demonstration account expiry in days
    /// </summary>
    public String HiddenUserFields
    {
      get
      {
        return this.getParameter ( AdapterFieldNames.Hidden_User_Fields );
      }
      set
      {
        this.setParameter ( AdapterFieldNames.Hidden_User_Fields, EvDataTypes.Text, value );
      }
    }

    /// <summary>
    /// The method return true if the hidden field is selected.
    /// </summary>
    /// <param name="Field">EdUserProfile.UserProfileFieldNames</param>
    /// <returns>bool</returns>
    public bool hasHiddenUserProfileField ( EdUserProfile.FieldNames Field )
    {
      if ( this.HiddenUserFields.Contains ( Field.ToString ( ) ) == true )
      {
        return true;
      }
      return false;
    }

    /// <summary>
    /// This property contains demonstration account expiry in days
    /// </summary>
    public String HiddenOrganisationFields
    {
      get
      {
        return this.getParameter ( AdapterFieldNames.Hidden_Organisation_Fields );
      }
      set
      {
        this.setParameter ( AdapterFieldNames.Hidden_Organisation_Fields, EvDataTypes.Text, value );
      }
    }
    /// <summary>
    /// The method return true if the hidden field is selected.
    /// </summary>
    /// <param name="Field">EdOrganisation.OrganisationFieldNames value</param>
    /// <returns>Bool</returns>
    public bool hasHiddenOrganisationField ( EdOrganisation.FieldNames Field )
    {
      if ( this.HiddenOrganisationFields.Contains ( Field.ToString ( ) ) == true )
      {
        return true;
      }
      return false;
    }

    /// <summary>
    /// This property defines the a user's primary entity to be created when registering.
    /// </summary>
    public String UserPrimaryEntity
    {
      get
      {
        return this.getParameter ( AdapterFieldNames.User_Primary_Entity );
      }
      set
      {
        this.setParameter ( AdapterFieldNames.User_Primary_Entity, EvDataTypes.Text, value );
      }
    }

    /// <summary>
    /// This property indicates if an organisation is to be created when a user registers.
    /// </summary>
    public bool CreateOrganisationOnUserRegistration
    {
      get
      {
        return EvStatics.getBool ( this.getParameter ( AdapterFieldNames.Create_Organisation_On_User_Registration ) );
      }
      set
      {
        this.setParameter ( AdapterFieldNames.Create_Organisation_On_User_Registration, EvDataTypes.Boolean, value.ToString ( ) );
      }
    }

    /// <summary>
    /// This property defines the a user category list to be use to define user categories and types.
    /// </summary>
    public String UserCategoryList
    {
      get
      {
        return this.getParameter ( AdapterFieldNames.User_Category_List );
      }
      set
      {
        this.setParameter ( AdapterFieldNames.User_Category_List, EvDataTypes.Text, value );
      }
    }

    /// <summary>
    /// This property defines the a organisation's primary entity to be created when registering.
    /// </summary>
    public String OrganisationPrimaryEntity
    {
      get
      {
        return this.getParameter ( AdapterFieldNames.Organisation_Primary_Entity );
      }
      set
      {
        this.setParameter ( AdapterFieldNames.Organisation_Primary_Entity, EvDataTypes.Text, value );
      }
    }

    #endregion

    #region Organisation Types Group


    public const String StaticOrgType = "Evado;Customer";

    /// <summary>
    /// This property contains the name of the person who last updated the trial object.
    /// </summary>
    public string OrgTypes
    {
      get
      {
        return this.getParameter ( AdapterFieldNames.Org_Types );
      }
      set
      {
        this.setParameter ( AdapterFieldNames.Org_Types, EvDataTypes.Text, value );
      }
    }

    /// <summary>
    /// This property contains the default organisation type
    /// </summary>
    public String DefaultOrgType
    {
      get
      {
        return
          this.getParameter ( EdAdapterSettings.AdapterFieldNames.Default_User_Org_Type );
      }
      set
      {
        this.setParameter ( EdAdapterSettings.AdapterFieldNames.Default_User_Org_Type,
          EvDataTypes.Text, value.ToString ( ) );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method returns a selected list of application roles based on the delimited
    /// list of selected roles that are passed to the method.
    /// </summary>
    /// <param name="SelectedOrgTypeIds">Delimited string of role identifiers</param>
    /// <returns>List of EdOrgType objects</returns>
    // ----------------------------------------------------------------------------------
    public List<EvOption> GetOrgTypeList ( bool ForSelectionList )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> optionList = new List<EvOption> ( );

      if ( ForSelectionList == true )
      {
        optionList.Add ( new EvOption ( ) );
      }

      optionList.Add ( new EvOption ( "Evado" ) );
      optionList.Add ( new EvOption ( "Customer" ) );

      //
      // get an array of org types
      //
      String [ ] arOrgTypes = this.OrgTypes.Split ( ';' );

      //
      // iterate through the list creating selection options.
      //
      for ( int i = 0; i < arOrgTypes.Length; i++ )
      {
        string str = arOrgTypes [ i ].Trim ( );

        if ( str == String.Empty
          || str.ToLower ( ) == "null" )
        {
          continue;
        }

        optionList.Add ( new EvOption ( str, str.Replace ( "_", " " ) ) );
      }

      //
      // return the list of selected roles
      //
      return optionList;
    }//END method

    #endregion

    #region Role Group

    public const String StaticRoles = "Administrator;Manager;Designer;Staff;Customer";


    /// <summary>
    /// This property contains the name of the person who last updated the trial object.
    /// </summary>
    public string UserRoles
    {
      get
      {
        return
          this.getParameter ( EdAdapterSettings.AdapterFieldNames.User_Roles );
      }
      set
      {
        this.setParameter ( EdAdapterSettings.AdapterFieldNames.User_Roles,
          EvDataTypes.Text, value.ToString ( ) );
      }
    }


    /// <summary>
    /// This property contains a delimited list of the default user roles.
    /// New uses will be given these roles as their default access.
    /// </summary>
    public String DefaultUserRoles
    {
      get
      {
        return
          this.getParameter ( EdAdapterSettings.AdapterFieldNames.Default_User_Roles );
      }
      set
      {
        this.setParameter ( EdAdapterSettings.AdapterFieldNames.Default_User_Roles,
          EvDataTypes.Text, value.ToString ( ) );
      }
    }



    // ==================================================================================
    /// <summary>
    /// This method returns a selected list of application roles based on the delimited
    /// list of selected roles that are passed to the method.
    /// </summary>
    /// <param name="SelectedRoleIds">Delimited string of role identifiers</param>
    /// <returns>List of EdRole objects</returns>
    // ----------------------------------------------------------------------------------
    public List<EvOption> GetRoleOptionList ( bool ForSelectionList )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> optionList = new List<EvOption> ( );
      String [ ] arRoles = this.UserRoles.Split ( ';' );

      if ( ForSelectionList == true )
      {
        optionList.Add ( new EvOption ( ) );
      }

      //
      // add the static roles.
      //
      optionList.Add ( new EvOption ( "Administrator" ) );
      optionList.Add ( new EvOption ( "Manager" ) );
      optionList.Add ( new EvOption ( "Designer" ) );
      optionList.Add ( new EvOption ( "Staff" ) );
      optionList.Add ( new EvOption ( "Customer" ) );

      //
      // Iterate through the array of roles to create the option list.
      //
      for ( int i = 0; i < arRoles.Length; i++ )
      {
        string str = arRoles [ i ].Trim ( );

        //
        // skip empty and null roles
        //
        if ( str == String.Empty
          || str.ToLower ( ) == "null" )
        {
          continue;
        }

        //
        // add the role as a selection option object.
        //
        optionList.Add ( new EvOption ( str, str.Replace ( "_", " " ) ) );
      }

      //
      // return the list of selected roles
      //
      return optionList;
    }//END method

    #endregion

    #region EVADO environment configuration

    /// <summary>
    /// This property contains demonstration account expiry in days
    /// </summary>
    public int DemoAccountExpiryDays
    {
      get
      {
        var value = this.getParameter ( AdapterFieldNames.DemoAccountExpiryDays );

        return EvStatics.getInteger ( value );
      }
      set
      {

        this.setParameter ( AdapterFieldNames.DemoAccountExpiryDays, EvDataTypes.Integer, value.ToString ( "00" ) );
      }
    }

    /// <summary>
    /// This property contains demonstration account expiry in days
    /// </summary>
    public String DemoRegistrationVideoUrl
    {
      get
      {
        return this.getParameter ( AdapterFieldNames.DemoRegistrationVideoUrl );

      }
      set
      {

        this.setParameter ( AdapterFieldNames.DemoRegistrationVideoUrl, EvDataTypes.Text, value );
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

        this.setParameter ( AdapterFieldNames.SmtpServer, EvDataTypes.Text, value );
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

        this.setParameter ( AdapterFieldNames.SmtpServerPort, EvDataTypes.Integer, value.ToString ( ) );
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

        this.setParameter ( AdapterFieldNames.SmtpUserId, EvDataTypes.Text, value );
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

        this.setParameter ( AdapterFieldNames.SmtpPassword, EvDataTypes.Text, value );
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

        this.setParameter ( AdapterFieldNames.EmailAlertTestAddress, EvDataTypes.Text, value );
      }
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Object Update

    private string _UpdatedBy = String.Empty;
    /// <summary>
    /// This property contains the name of the person who last updated the trial object.
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

    private string _UpdatedByUserId = String.Empty;
    /// <summary>
    ///  This property contains the network identifier of the user that saved the trial object.
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

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    /// <summary>
    /// This property contains a list of application parameters.
    /// </summary>
    //public List<EvObjectParameter> Parameters { get; set; }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Methods

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
      EdAdapterSettings.AdapterFieldNames FieldName,
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
        case EdAdapterSettings.AdapterFieldNames.Version:
          {
            this.Version = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.MaximumSelectionListLength:
          {
            this.MaximumSelectionListLength = EvcStatics.getInteger ( Value );
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.DemoAccountExpiryDays:
          {
            this.DemoAccountExpiryDays = EvcStatics.getInteger ( Value );
            return;
          }
        case EdAdapterSettings.AdapterFieldNames.DemoRegistrationVideoUrl:
          {
            this.DemoRegistrationVideoUrl = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.SmtpServer:
          {
            this.SmtpServer = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.SmtpServerPort:
          {
            if ( int.TryParse ( Value, out intValue ) == true )
            {
              this.SmtpServerPort = intValue;
            }
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.SmtpUserId:
          {
            this.SmtpUserId = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.SmtpPassword:
          {
            this.SmtpPassword = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.EmailAlertTestAddress:
          {
            this.EmailAlertTestAddress = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.State:
          {
            this.setParameter ( AdapterFieldNames.State, EvDataTypes.Text, Value );
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Ads_Group:
          {
            this.setParameter ( AdapterFieldNames.Ads_Group, EvDataTypes.Text, Value );
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.HelpUrl:
          {
            this.HelpUrl = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Home_Page_Header:
          {
            this._HomePageHeaderText = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Title:
          {
            this._Title = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Http_Reference:
          {
            this._HttpReference = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Description:
          {
            this._HttpReference = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.EnableAdminGroupOnEntitPages:
          {
            this.setParameter ( EdAdapterSettings.AdapterFieldNames.EnableAdminGroupOnEntitPages,
              EvDataTypes.Boolean, Value.ToString ( ) );
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Enable_Binary_Data:
          {
            this.setParameter ( AdapterFieldNames.Enable_Binary_Data, EvDataTypes.Boolean, Value );
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Enable_Entity_Edit_Button_Update:
          {
            this.setParameter ( EdAdapterSettings.AdapterFieldNames.Enable_Entity_Edit_Button_Update,
              EvDataTypes.Boolean, Value.ToString ( ) );
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Enable_Entity_Save_Button_Update:
          {
            this.setParameter ( EdAdapterSettings.AdapterFieldNames.Enable_Entity_Save_Button_Update,
              EvDataTypes.Boolean, Value.ToString ( ) );
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Enable_User_Organisation_Update:
          {
            this.setParameter ( AdapterFieldNames.Enable_User_Organisation_Update, EvDataTypes.Boolean, Value );
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Enable_User_Address_Update:
          {
            this.setParameter ( AdapterFieldNames.Enable_User_Address_Update, EvDataTypes.Boolean, Value );
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Use_Home_Page_Header_On_All_Pages:
          {
            this.setParameter ( AdapterFieldNames.Use_Home_Page_Header_On_All_Pages, EvDataTypes.Boolean, Value );
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.User_Roles:
          {
            string roles = Value;
            roles = roles.Replace ( "\r\n", ";" );
            roles = roles.Replace ( "; ", ";" );
            roles = roles.Replace ( " ;", ";" );
            this.UserRoles = roles.Replace ( ";;", ";" );
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Default_User_Roles:
          {
            this.DefaultUserRoles = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Org_Types:
          {
            string orgType = Value;
            orgType = orgType.Replace ( "\r\n", ";" );
            orgType = orgType.Replace ( "; ", ";" );
            orgType = orgType.Replace ( " ;", ";" );
            this.OrgTypes = orgType.Replace ( ";;", ";" );
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Default_User_Org_Type:
          {
            this.DefaultOrgType = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Static_Query_Filter_Options:
          {
            this.setParameter ( AdapterFieldNames.Static_Query_Filter_Options, EvDataTypes.Text, Value );
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Hidden_User_Fields:
          {
            this.HiddenUserFields = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Hidden_Organisation_Fields:
          {
            this.HiddenOrganisationFields = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.User_Primary_Entity:
          {
            this.UserPrimaryEntity = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Create_Organisation_On_User_Registration:
          {
            this.CreateOrganisationOnUserRegistration = EvStatics.getBool ( Value );
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.Organisation_Primary_Entity:
          {
            this.OrganisationPrimaryEntity = Value;
            return;
          }

        case EdAdapterSettings.AdapterFieldNames.User_Category_List:
          {
            this.UserCategoryList = Value;
            return;
          }


        default:

          return;

      }//END Switch

    }//END setValue method 

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class static Methods
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }//END SiteProperties class

}//END namespace Evado.Model.Digital

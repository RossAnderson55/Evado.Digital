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
  public class EdAdapterParameters : Evado.Model.EvParameters
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
      /// This enumeration define a description field name of a trial
      /// </summary>
      Description,

      /// <summary>
      /// This enumeration define a collecting binary data field name of a trial
      /// </summary>
      Enable_Binary_Data,

      /// <summary>
      /// this enumeration defiens the roles in the application.
      /// </summary>
      Roles,

      /// <summary>
      /// this enumeration the default user role field in the application
      /// </summary>
      Default_User_Roles,

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

    #region Application setings

    private string _HomePageHeaderText = String.Empty;
    /// <summary>
    /// This property contains the Home page header text.
    /// 
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
          this.getParameter ( EdAdapterParameters.AdapterFieldNames.Http_Reference );
      }
      set
      {
        this.setParameter ( EdAdapterParameters.AdapterFieldNames.Http_Reference,
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
    /// This property indicates binary data is being collected.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public bool EnableBinaryData
    {
      get
      {
        return EvStatics.getBool (
          this.getParameter ( EdAdapterParameters.AdapterFieldNames.Enable_Binary_Data ) );
      }
      set
      {
        this.setParameter ( EdAdapterParameters.AdapterFieldNames.Enable_Binary_Data,
          EvDataTypes.Boolean, value.ToString ( ) );
      }
    }

    #endregion

    #region Role Group

    private List<EdRole> _RoleList = new List<EdRole> ( );

    /// <summary>
    /// This property contains the list of roles for this application.
    /// Static roles will be automatically added to the last on creation.
    /// </summary>
    public List<EdRole> RoleList
    {
      get { return _RoleList; }
      set { _RoleList = value; }
    }



    private String _DefaultRoles = String.Empty;

    /// <summary>
    /// This property contains a delimited list of the default user roles.
    /// New uses will be given these roles as their default access.
    /// </summary>
    public String DefaultUserRoles
    {
      get
      {
        return
          this.getParameter ( EdAdapterParameters.AdapterFieldNames.Default_User_Roles );
      }
      set
      {
        this.setParameter ( EdAdapterParameters.AdapterFieldNames.Default_User_Roles,
          EvDataTypes.Text, value.ToString ( ) );
      }
    }


    /// <summary>
    /// This property contains the name of the person who last updated the trial object.
    /// </summary>
    public string Roles
    {
      get
      {
        String roles = String.Empty;
        foreach ( EdRole role in this._RoleList )
        {
          roles += role.RoleId + ":" + role.Description + ";\r\n";
        }

        return roles;
      }
      set
      {
        this._RoleList = new List<EdRole> ( );
        EdRole role = new EdRole ( );
        String roles = value;
        String [ ] arRoles = roles.Split ( ';' );

        foreach ( string str in arRoles )
        {
          if ( str.Contains ( ":" ) == true )
          {
            string [ ] arStr = str.Split ( ':' );

            role = new EdRole ( arStr [ 0 ], arStr [ 1 ] );
          }
          else
          {
            role = new EdRole ( str, str );
          }

          this._RoleList.Add ( role );
        }
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
    public List<EdRole> filteredRoleList ( String SelectedRoleIds )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EdRole> roleList = new List<EdRole> ( );

      //
      // Iterate through the application roles and select those roles
      // that are in the selected role list.
      //
      foreach ( EdRole role in this._RoleList )
      {
        if ( SelectedRoleIds.Contains ( role.RoleId ) == true )
        {
          roleList.Add ( role );
        }
      }

      //
      // return the list of selected roles
      //
      return roleList;
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
        var value = this.getParameter ( AdapterFieldNames.DemoAccountExpiryDays.ToString ( ) );

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
        return this.getParameter ( AdapterFieldNames.DemoRegistrationVideoUrl.ToString ( ) );

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
      EdAdapterParameters.AdapterFieldNames FieldName,
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
        case EdAdapterParameters.AdapterFieldNames.Version:
          {
            this.Version = Value;
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.MaximumSelectionListLength:
          {
            this.MaximumSelectionListLength = EvcStatics.getInteger ( Value );
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.DemoAccountExpiryDays:
          {
            this.DemoAccountExpiryDays = EvcStatics.getInteger ( Value );
            return;
          }
        case EdAdapterParameters.AdapterFieldNames.DemoRegistrationVideoUrl:
          {
            this.DemoRegistrationVideoUrl = Value;
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.SmtpServer:
          {
            this.SmtpServer = Value;
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.SmtpServerPort:
          {
            if ( int.TryParse ( Value, out intValue ) == true )
            {
              this.SmtpServerPort = intValue;
            }
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.SmtpUserId:
          {
            this.SmtpUserId = Value;
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.SmtpPassword:
          {
            this.SmtpPassword = Value;
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.EmailAlertTestAddress:
          {
            this.EmailAlertTestAddress = Value;
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.State:
          {
            this.setParameter ( AdapterFieldNames.State, EvDataTypes.Text, Value );
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.Ads_Group:
          {
            this.setParameter ( AdapterFieldNames.Ads_Group, EvDataTypes.Text, Value );
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.HelpUrl:
          {
            this.HelpUrl = Value ;
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.Home_Page_Header:
          {
            this._HomePageHeaderText = Value;
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.Title:
          {
            this._Title = Value ;
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.Http_Reference:
          {
            this._HttpReference = Value;
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.Description:
          {
            this._HttpReference = Value;
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.Enable_Binary_Data:
          {
            this.setParameter ( AdapterFieldNames.Ads_Group, EvDataTypes.Text, Value );
            return;
          }

        case EdAdapterParameters.AdapterFieldNames.Roles:
          {
            this.Roles = Value ;
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

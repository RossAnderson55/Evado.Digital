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


    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region EVADO environment configuration

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
        this.setParameter ( SettingFieldNames.Display_Site_Dashboard, EvDataTypes.Boolean, value.ToString ( ) );
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

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class static Methods
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }//END SiteProperties class

}//END namespace Evado.Model.Digital

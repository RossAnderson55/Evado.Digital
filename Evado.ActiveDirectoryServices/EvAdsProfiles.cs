/***************************************************************************************
 * <copyright file="EvAdsProfiles.cs company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2001 - 2013 EVADO HOLDING PTY. LTD.  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 * 
 ****************************************************************************************/
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

using Evado.Model;

namespace Evado.ActiveDirectoryServices
{
  //  ==================================================================================
  /// <summary>
  /// A business Component used to manage User and Group Information in Active Directory Service
  /// Configuration data for 'web.config'
  /// </summary>
  // -------------------------------------------------------------------------------------
  public class EvAdsProfiles
  {
    #region Initialise Class variables and objects

    public const string CONFIG_ADS_SERVER_KEY = "ADS_SERVER";
    public const string CONFIG_ADS_ADMIN_NAME_KEY = "ADS_ADMIN_NAME";
    public const string CONFIG_ADS_ADMIN_PASSWORD_KEY = "ADS_ADMIN_PASSWORD";
    public const string CONFIG_ADS_USER_ROLE_CONTAINER_KEY = "ADS_USER_ROLES";
    public const string CONFIG_ADS_LDAP_STRING_KEY = "ADS_LDAP_STRING";

    private static string _eventLogSource = ConfigurationManager.AppSettings [ Evado.Model.EvStatics.CONFIG_EVENT_LOG_KEY ];
    private static string _server = ConfigurationManager.AppSettings [ CONFIG_ADS_SERVER_KEY ].ToString ( );
    private static string _adminName = ConfigurationManager.AppSettings [ CONFIG_ADS_ADMIN_NAME_KEY ].ToString ( );
    private static string _adminPassword = ConfigurationManager.AppSettings [ CONFIG_ADS_ADMIN_PASSWORD_KEY ].ToString ( );
    private static string _rootContainer = ConfigurationManager.AppSettings [ CONFIG_ADS_USER_ROLE_CONTAINER_KEY ].ToString ( );
    private static string _ldapString = ConfigurationManager.AppSettings [ CONFIG_ADS_LDAP_STRING_KEY ].ToString ( );
    private EvAdsFacade _evAdFacade;
    /// <summary>
    /// The class initialisation method.
    /// </summary>
    public EvAdsProfiles ( )
    {
      this.LogInitMethod ( "EvAdsProfiles" );
      EvAdsConfig adConfig = new EvAdsConfig
      {
        Server = _server,
        AdminName = _adminName,
        AdminPassword = _adminPassword,
        RootContainer = _rootContainer,
        UsersContainer = _rootContainer,
        GroupsContainer = _rootContainer,
        LdapString = _ldapString
      };

      this.LogInit ( "Server: " + _server );
      this.LogInit ( "adminName: " + _adminName );
      this.LogInit ( "adminPassword: " + _adminPassword );
      this.LogInit ( "rootContainer: " + _rootContainer );
      this.LogInit ( "ldapString: " + _ldapString );

      _evAdFacade = EvAdsFacadeFactory.CreateFacade ( adConfig );

      this.LogAssembly ( this._evAdFacade.Log );

      this.LogInitMethodEnd ( "EvAdsProfiles" );
    }

    public string DomainName
    {
      get
      {
        String domainName = String.Empty;
        string [ ] addRootContainer = _rootContainer.Split ( ',' );

        foreach ( string str in addRootContainer )
        {
          if ( str.Contains ( "DC=" ) == true )
          {
            if ( domainName != String.Empty )
            {
              domainName += ".";
            }
            domainName += str.Replace ( "DC=", String.Empty );
          }
        }

        return domainName;
      }
    }
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region UserProfile Query methods
    //  ==================================================================================
    /// <summary>
    /// Get All evado users with group property or not according to parameter
    /// </summary>
    /// <param name="WithGroup">bool</param>
    /// <returns>List&lt;EvAdsUserProfile&gt;</returns>
    // -------------------------------------------------------------------------------------
    public List<EvAdsUserProfile> AllAdsUsers ( bool WithGroup )
    {
      this.LogMethod ( "AllAdsUsers method." );
      this.LogValue ( "WithGroups: " + WithGroup );

      this._evAdFacade.LogLevel = this._LogLevel;

      return _evAdFacade.AllEvUsers ( WithGroup );
      // return users;
    }

    //  ==================================================================================
    /// <summary>
    /// Get All evado users without group
    /// And Filtered by Filter value
    /// </summary>
    /// <param name="Filter">string</param>
    /// <returns>List&lt;EvAdsUserProfile&gt;</returns>
    // -------------------------------------------------------------------------------------
    public List<EvAdsUserProfile> AllAdsUsers ( string Filter )
    {
      this.LogMethod ( "AllAdsUsers method." );
      this.LogValue ( "Filter: " + Filter );
      this._evAdFacade.LogLevel = this._LogLevel;

      return _evAdFacade.AllEvUsersAfterBeingFiltered ( Filter );
      // return users;
    }

    //  ==================================================================================
    /// <summary>
    /// Get All evado groups
    /// </summary>
    /// <returns>List&lt;EvAdsGroupProfile&gt;</returns>
    // -------------------------------------------------------------------------------------
    public List<EvAdsGroupProfile> AllAdsGroups ( )
    {
      this.LogMethod ( "AllAdsGroups method." );
      return _evAdFacade.getAllGroups ( );
    }

    //  ==================================================================================
    /// <summary>
    /// Get All evado customer groups
    /// </summary>
    /// <returns>List&lt;EvAdsGroupProfile&gt;</returns>
    // -------------------------------------------------------------------------------------
    public List<EvAdsGroupProfile> AllAdsCustomers ( )
    {
      this.LogMethod ( "AllAdsCustomers method." );
      this._evAdFacade.LogLevel = this._LogLevel;

      List<EvAdsGroupProfile> groups = _evAdFacade.getAllGroups ( );

      return groups.FindAll ( delegate ( EvAdsGroupProfile s )
      {
        return s.Name.StartsWith ( "CU_" );
      } );
    }

    #endregion

    #region Retrieve Query methods

    // =====================================================================================
    /// <summary>
    /// GetUser method
    /// 
    /// Description:
    /// Get  the information for a user from ADS
    /// 
    /// </summary>
    /// <param name="UserId">string</param>
    /// <returns>EvAdsUserProfile</returns>
    // -------------------------------------------------------------------------------------
    public EvAdsUserProfile GetUser ( string UserId )
    {
      this.LogMethod ( "GetUser method." );
      this.LogValue ( "UserId: " + UserId );

      EvAdsUserProfile evUser;
      this._evAdFacade.LogLevel = this._LogLevel;

      EvAdsCallResult callResult = this._evAdFacade.FindAdUserById ( UserId, out evUser );
      if ( callResult == EvAdsCallResult.Success )
      {
        return evUser;
      }

      return null;

    }// Close GetUser method

    // =====================================================================================
    /// <summary>
    /// GetUser method
    /// 
    /// Description:
    /// Get  the information for a user from ADS
    /// 
    /// </summary>
    /// <param name="UserId">string</param>
    /// <returns>EvAdsUserProfile</returns>
    // -------------------------------------------------------------------------------------
    public EvAdsUserProfile GetUserByEmail ( string Email )
    {
      this.LogMethod ( "GetUserByEmail method." );
      this.LogValue ( "Email: " + Email );

      EvAdsUserProfile evUser;
      this._evAdFacade.LogLevel = this._LogLevel;

      EvAdsCallResult callResult = this._evAdFacade.FindAdUserByEmail ( Email, out evUser );
      if ( callResult == EvAdsCallResult.Success )
      {
        return evUser;
      }

      return null;

    }// Close GetUserByEmail method

    // =====================================================================================
    /// <summary>
    /// GetUser method
    /// 
    /// Description:
    /// Get  the information for a user from ADS
    /// 
    /// </summary>
    /// <param name="UserId">string</param>
    /// <returns>EvAdsUserProfile</returns>
    // -------------------------------------------------------------------------------------
    public EvAdsUserProfile GetUserByPasswordResetToken ( string token )
    {
      this.LogMethod ( "GetUserByPasswordResetToken method." );
      this.LogValue ( "token: " + token );
      this._evAdFacade.LogLevel = this._LogLevel;

      EvAdsUserProfile evUser;

      EvAdsCallResult callResult = this._evAdFacade.FindAdUserByPasswordResetToken ( token, out evUser );
      if ( callResult == EvAdsCallResult.Success )
      {
        return evUser;
      }
      return null;

    }// Close GetUserByEmail method

    // =====================================================================================
    /// <summary>
    /// GetGroup method
    /// 
    /// Description:
    /// Get  the information for a group from ADS
    /// 
    /// </summary>
    /// <param name="GroupName">string</param>
    /// <returns>EvAdsGroupProfile</returns>
    // -------------------------------------------------------------------------------------
    public EvAdsGroupProfile GetGroup ( string GroupNameOrGuid )
    {
      this.LogMethod ( "GetGroup method." );
      this.LogValue ( "GroupNameOrGuid: " + GroupNameOrGuid );

      EvAdsGroupProfile evGroup;
      Guid groupGuid;
      EvAdsCallResult callResult;
      this._evAdFacade.LogLevel = this._LogLevel;

      // check if this is a guid
      try
      {
        groupGuid = new Guid ( GroupNameOrGuid );
      }
      catch ( FormatException )
      {
        groupGuid = default ( Guid );
      }

      if ( groupGuid != default ( Guid ) )
      {
        callResult = this._evAdFacade.FindAdGroupByGuid ( groupGuid, out evGroup );
      }
      else
      {
        callResult = this._evAdFacade.FindAdGroupByName ( GroupNameOrGuid, out evGroup );
      }

      if ( callResult == EvAdsCallResult.Success )
      {
        return evGroup;
      }

      return null;

    }// Close getTrial method

    public void AddGroupsToEvAdsUser ( ref EvAdsUserProfile evUser )
    {
      this._evAdFacade.AddGroupsToEvAdsUser ( ref evUser );
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Update Data methods

    // =====================================================================================
    /// <summary>
    /// DeleteItem method
    /// 
    /// Description:
    /// Deletes the User from the table. 
    /// 
    /// </summary>
    /// <param name="Profile">EvAdsUserProfile</param>
    // -------------------------------------------------------------------------------------
    public Evado.Model.EvEventCodes DeleteItem ( EvAdsUserProfile Profile )
    {
      this.LogMethod ( "DeleteItem method." );
      this.LogValue ( "Profile.SamAccountName: " + Profile.SamAccountName );
      this._evAdFacade.LogLevel = this._LogLevel;

      this._evAdFacade.DeleteUserFromAd ( Profile );

      this.LogAssembly ( this._evAdFacade.Log );

      return Evado.Model.EvEventCodes.Ok;

    } // END DeleteItem method


    // =====================================================================================
    /// <summary>
    /// Description:
    /// Creates or Updates the user profile in the ADS. 
    /// </summary>
    /// <param name="UserToSave">EvAdsUserProfile</param>
    /// <param name="UserToSave">EvAdsUserProfile</param>
    /// <param name="OutEvUser">EvAdsUserProfile</param>
    // -------------------------------------------------------------------------------------
    public EvAdsCallResult SaveItem (
      EvAdsUserProfile UserToSave,
      bool isCreatingNewUser,
      out EvAdsUserProfile OutEvUser )
    {
      this.LogMethod ( "SaveItem method." );


      this.LogValue ( "UserToSave.SamAccountName: " + UserToSave.SamAccountName );
      this.LogValue ( "isCreatingNewUser: " + isCreatingNewUser );
      this.LogValue ( "UserToSave.Password: " + UserToSave.Password );

      // 
      // Check that the user id is valid
      // 
      EvAdsUserProfile userToCheck;
      EvAdsCallResult callResult;
      OutEvUser = null;

      try
      {
        this._evAdFacade.LogLevel = this._LogLevel;
        callResult = this._evAdFacade.FindAdUserById ( UserToSave.SamAccountName, out userToCheck );

        if ( isCreatingNewUser == true
          && callResult != EvAdsCallResult.Object_Not_Found )
        {
          this.LogMethod_End ( "ACTION: SaveItem" );
          return EvAdsCallResult.Invalid_Argument;
        }
        else
        {
          if ( isCreatingNewUser == true
           && callResult == EvAdsCallResult.Object_Not_Found )
          {
            OutEvUser = _evAdFacade.CreateNewUser ( UserToSave );
            this.LogMethod_End ( "ACTION: CreateNewUser" );
          }
          else
          {
            if ( isCreatingNewUser == false
              && callResult == EvAdsCallResult.Success )
            {
              this.LogValue ( "ACTION: UpdatE user" );
              OutEvUser = _evAdFacade.UpdateAdUser ( UserToSave );
            }
          }
        }
      }
      catch ( Exception Ex )
      {
        this.LogValue ( EvStatics.getException ( Ex ) );
      }
      this.LogAssembly ( this._evAdFacade.Log );

      this.LogMethod_End ( "SaveItem" );
      return EvAdsCallResult.Success;
    }

    // =====================================================================================
    /// <summary>
    /// saveItem method
    /// 
    /// Description:
    /// Creates or Updates the user profile in the ADS. 
    /// 
    /// </summary>
    /// <param name="UserToSave">EvAdsUserProfile</param>
    /// <param name="UserToSave">EvAdsUserProfile</param>
    /// <param name="OutEvUser">EvAdsUserProfile</param>
    // -------------------------------------------------------------------------------------
    public EvAdsCallResult setPassword (
      String SamAccountName,
      String Password )
    {
      this.LogMethod ( "setPassword method." );
      this.LogValue ( "SamAccountName: " + SamAccountName );
      this.LogValue ( "Password: " + Password );

      // 
      // Check that the user id is valid
      // 
      this._evAdFacade.LogLevel = this._LogLevel;

      EvAdsCallResult callResult = this._evAdFacade.SetPassword ( SamAccountName, Password );

      this._ClassLog.AppendLine ( _evAdFacade.Log );

      this.LogMethod_End ( "setPassword" );

      return callResult;
    }



    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Loggin properties and methods.


    private const String CONST_NAME_SPACE = "Evado.ActiveDirectoryServices.EvAdsProfiles.";
    /// <summary>
    /// DebugLog stores the business object status conditions.
    /// </summary>
    protected System.Text.StringBuilder _ClassLog = new System.Text.StringBuilder ( );

    public string ApplicationLog
    {
      get
      {
        return _ClassLog.ToString ( );
      }
    }

    private int _LogLevel = 2;

    /// <summary>
    /// This property sets the application logging level between 0 and 4
    /// </summary>
    public int LogLevel
    {
      set
      {
        this._LogLevel = value;

        if ( this._LogLevel < 0 )
        {
          this._LogLevel = 0;
        }

        if ( this._LogLevel > 4 )
        {
          this._LogLevel = 4;
        }

        if ( this._LogLevel == 0 )
        {
          this._ClassLog = new System.Text.StringBuilder ( );
        }
      }
    }

    private bool _DebugOn = false;
    /// <summary>
    /// This property sets the debug state for the class.
    /// </summary>
    public bool DebugOn
    {
      set
      {
        _DebugOn = value;
        if ( _DebugOn == true )
        {
          this._LogLevel = 4;
        }
        else
        {
          this._ClassLog = new System.Text.StringBuilder ( );
        }
      }
    }

    public string Log
    {
      get
      {
        return ApplicationLog.ToString ( );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method resets the debug log
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void resetApplicationLog ( )
    {
      this._ClassLog = new System.Text.StringBuilder ( );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogInitMethod ( String MethodName )
    {
      this._ClassLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
      + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
      + EvAdsProfiles.CONST_NAME_SPACE + MethodName
      + " initialisation method." );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogInitMethodEnd ( String MethodName )
    {
      String value = Evado.Model.EvStatics.CONST_METHOD_END;

      value = value.Replace ( " END OF METHOD ", " END OF " + CONST_NAME_SPACE + MethodName + " METHOD " );

      this._ClassLog.AppendLine ( value );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="DebugLogString">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogInit ( String DebugLogString )
    {
      this._ClassLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + DebugLogString );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogMethod ( String Value )
    {
      if ( this._LogLevel >= 2 )
      {
        this._ClassLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
        + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
        + EvAdsProfiles.CONST_NAME_SPACE + Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the applocation string 
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogMethod_End ( String MethodName )
    {
      if ( this._LogLevel >= 2 )
      {
        String value = Evado.Model.EvStatics.CONST_METHOD_END;

        value = value.Replace ( " END OF METHOD ", " END OF " + CONST_NAME_SPACE + MethodName + " METHOD " );

        this._ClassLog.AppendLine ( value );
      }
    }


    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="DebugLogString">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogValue ( String DebugLogString )
    {
      if ( this._LogLevel >= 3 )
      {
        this._ClassLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + DebugLogString );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="DebugLogString">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogDebug ( String DebugLogString )
    {
      if ( this._LogLevel >= 4 )
      {
        this._ClassLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + DebugLogString );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogAssembly ( String Value )
    {
      if ( this._LogLevel >= 3 )
      {
        this._ClassLog.AppendLine ( Value );
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }//END CLass

}//END namespace

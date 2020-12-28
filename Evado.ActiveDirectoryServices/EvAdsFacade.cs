/***************************************************************************************
 * <copyright file="EvAdFacade.cs company="EVADO HOLDING PTY. LTD.">
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
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.DirectoryServices;

namespace Evado.ActiveDirectoryServices
{
  //  ==================================================================================
  /// <summary>
  /// Class EvAdFacade
  /// Managing User accounts with MS Active Directory Service.
  /// By using System.DirectoryServices.AccountManagement(Wrapper for LDAP), LDAP doesn't need to be used.
  /// </summary>
  //  ---------------------------------------------------------------------------------- 
  public class EvAdsFacade : IEvAccountManageable
  {

    #region Initialisation methods
    //  ==================================================================================
    /// <summary>
    /// Initializes a new instance of the EvAdsFacade class.
    /// </summary>
    /// <param name="Config">EvAdsConfig.</param>
    //  ---------------------------------------------------------------------------------- 
    public EvAdsFacade ( EvAdsConfig Config )
    {
      this.LogInitMethod ( "EvAdsFacade" );
      _evAdConfig = Config;
      _evAdsLdapHelper = new EvAdsLdapHelper ( _evAdConfig.LdapString, _evAdConfig.AdminName, _evAdConfig.AdminPassword );

      this.LogInit ( "_evAdConfig.UsersContainer: " + _evAdConfig.UsersContainer );
      this.LogInit ( "_evAdConfig.LdapString: " + _evAdConfig.LdapString );
      this.LogInit ( "_evAdConfig.AdminName: " + _evAdConfig.AdminName );
      this.LogInit ( "_evAdConfig.AdminPassword: " + _evAdConfig.AdminPassword );
      this.LogInit ( "_evAdConfig.RootContainer: " + _evAdConfig.RootContainer );
      this.LogInit ( "_evAdConfig.GroupsContainer: " + _evAdConfig.GroupsContainer );

      this.LogInitMethodEnd ( "EvAdsProfiles" );
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region readonly private fields

    private readonly EvAdsConfig _evAdConfig;
    private readonly EvAdsLdapHelper _evAdsLdapHelper;
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region private properties

   // private List<EvAdsGroupProfile> _AdGroupList;
    /// <summary>
    /// DebugLog stores the business object status conditions.
    /// </summary>
    protected System.Text.StringBuilder _ClassLog = new System.Text.StringBuilder ( );

    public string Log
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

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Public Method fields

    //  ==================================================================================
    /// <summary>
    /// Creates the new use in AD.
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <returns>EvAdsUserProfile.</returns>
    /// <example>
    /// <code numberLines="true">
    /// string userId = "User" + Guid.NewGuid().ToString();
    /// EvAdsUserProfile EvUser = new EvAdsUserProfile()
    /// {
    ///    EmailAddress = @"test@evado.com.au",
    ///    SamAccountName = userId
    /// };
    /// 
    /// string EvGroupName = "Group" + Guid.NewGuid().ToString();
    /// EvAdsGroupProfile EvGroup = new EvAdsGroupProfile()
    /// {
    ///    Name = EvGroupName,
    ///    Description = "New Description"
    /// };
    /// List&lt;EvAdsGroupProfile&gt EvGroups = new List&lt;EvAdsGroupProfile&gt;();
    /// EvGroups.Add(EvGroup);
    /// EvUser.EvGroups = EvGroups;
    /// 
    /// EvAdConfig Config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    ///   
    /// EvAdsFacade adFacade = EvAdFacadeFactory.CreateFacade(Config); 
    /// EvAdsUserProfile createdUser = adFacade.CreateNewUser(EvUser);
    /// </code>
    /// </example>
    //  ---------------------------------------------------------------------------------- 
    public EvAdsUserProfile CreateNewUser (
      EvAdsUserProfile EvUser )
    {
      this.LogMethod ( "CreateNewUser method" );
      EvAdsCallResult callResult;
      string password = EvUser.Password;

      //
      //Check validation of 'EvUser' Parameter
      //
      bool validation = EvAdsUserParamValidator ( EvUser );
      if ( validation == false )
      {
        this.LogValue ( "ERROR: Validation failed" );
        return null;
      }

      //
      //Create UserPrincipalFromAD in ADS
      //
      callResult = this.CreateUserPrincipalWithAdUser ( EvUser );

      if ( callResult != EvAdsCallResult.Success )
      {
        this.LogValue ( "ERROR: CreateNewUser failed." );
        return null;
      }

      //
      //After finishing creation, Return the User has been created.
      //
      EvAdsUserProfile evUser;
      callResult = FindAdUserById ( EvUser.SamAccountName, out evUser );

      if ( callResult != EvAdsCallResult.Success )
      {
        this.LogValue ( " ERROR: Retrieving user object." );
        return null;
      }

      return evUser;

    }//END method

    //  ==================================================================================
    /// <summary>
    /// Updates the information for user in AD.
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <returns>EvAdsUserProfile.</returns>
    /// <example>
    /// <code numberLines="true">
    /// string userId = "User" + Guid.NewGuid().ToString();
    /// EvAdsUserProfile EvUser = new EvAdsUserProfile()
    /// {
    ///    EmailAddress = @"test@evado.com.au",
    ///    SamAccountName = userId
    /// };
    /// 
    /// EvAdConfig Config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    ///   
    /// EvAdsFacade adFacade = EvAdFacadeFactory.CreateFacade(Config); 
    /// EvAdsUserProfile updatedUser = adFacade.UpdateAdUser(EvUser);
    /// </code>
    /// </example>
    //  ---------------------------------------------------------------------------------- 
    public EvAdsUserProfile UpdateAdUser (
      EvAdsUserProfile EvUser )
    {
      this.LogMethod ( "UpdateAdUser method" );
      //
      //Initialize Action Delegate to Update UserProfile.
      //
      Action<UserPrincipal> actionToUpdate = up =>
          {
            //
            //Check EvUser's String Value to if the value is string.empty
            //If it is convert it to 'NULL' since ADS could not accept string.empty as Parameter
            //
            up.Description = this.ChekcEmptyOrNullString ( up.Description, EvUser.Description );
            up.DisplayName = this.ChekcEmptyOrNullString ( up.DisplayName, EvUser.DisplayName );
            up.EmailAddress = this.ChekcEmptyOrNullString ( up.EmailAddress, EvUser.EmailAddress );
            up.GivenName = this.ChekcEmptyOrNullString ( up.GivenName, EvUser.GivenName );
            up.Surname = this.ChekcEmptyOrNullString ( up.Surname, EvUser.Surname );
            up.UserPrincipalName = this.ChekcEmptyOrNullString ( up.UserPrincipalName, EvUser.UserPrincipalName );
            up.SamAccountName = this.ChekcEmptyOrNullString ( up.SamAccountName, EvUser.SamAccountName );
            up.VoiceTelephoneNumber = this.ChekcEmptyOrNullString ( up.VoiceTelephoneNumber, EvUser.VoiceTelephoneNumber );
            up.Save ( );

            //
            //Enable User
            //
            if ( EvUser.Enabled != null )
            {
              up.Enabled = EvUser.Enabled;
            }
            up.UserCannotChangePassword = EvUser.UserCannotChangePassword;
            up.PasswordNeverExpires = EvUser.PasswordNeverExpires;
            up.Save ( );

            //
            //To update additional properties 
            //
            _evAdsLdapHelper.SetAdditionalUserProperty ( EvUser );

            //
            //If Password is entered, Set new password
            //
            if ( String.IsNullOrEmpty ( EvUser.Password ) == false )
            {
              up.SetPassword ( EvUser.Password );
              up.Save ( );
            }

            //
            //Create or Update GroupPrincipalFromAD
            //
            this.CreateOrUpdateGroupPrincipal ( EvUser, up );
          };

      //
      // Execute template method with add_delegate
      //
      var result = TemplateForAdUserOperation ( EvUser, actionToUpdate );

      //
      //If there is any group checkbox unselected, unregister user from the group
      //
      var rolesToBeDeleted = EvUser.EvGroups.FindAll ( role => role.ToBeDeletedFromUser == true );
      if ( rolesToBeDeleted.Count > 0 )
      {
        UnregisterUserFromMultipleGroups ( EvUser, rolesToBeDeleted );
      }
      EvAdsUserProfile evUser = null;
      EvAdsCallResult callResult = FindAdUserById ( EvUser.SamAccountName, out evUser );

      if ( callResult == EvAdsCallResult.Success )
      {
        return evUser;
      }
      else
      {
        return null;
      }
    } // End UpdateAdUser Method

    //  ==================================================================================
    /// <summary>
    /// Deletes the user from ADS.
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <returns>EvAdsCallResult.</returns>
    /// <example>
    /// <code numberLines="true">
    /// string userId = "User" + Guid.NewGuid().ToString();
    /// EvAdsUserProfile EvUser = new EvAdsUserProfile()
    /// {
    ///    EmailAddress = @"test@evado.com.au",
    ///    SamAccountName = userId
    /// };
    /// 
    /// EvAdConfig Config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    ///   
    /// EvAdsFacade adFacade = EvAdFacadeFactory.CreateFacade(Config); 
    /// EvAdsCallResult CallResult = adFacade.DeleteUserFromAd(EvUser);
    /// </code>
    /// </example>
    //  ---------------------------------------------------------------------------------- 
    public EvAdsCallResult DeleteUserFromAd ( EvAdsUserProfile EvUser )
    {
      //
      //Initialize Action Delegate to Delete UserProfile.
      //
      Action<UserPrincipal> actionToUpdate = up => up.Delete ( );

      return TemplateForAdUserOperation ( EvUser, actionToUpdate );
    }

    //  ==================================================================================
    /// <summary>
    /// Finds the User by id. and Id should be samAccountName
    /// </summary>
    /// <param name="EvUserId">string.</param>
    /// <param name="CallResult">EvAmCallResultt.</param>
    /// <returns>EvAdsUserProfile.</returns>
    /// <example>
    /// <code numberLines="true">
    /// string userId = "User" + Guid.NewGuid().ToString();
    /// EvAdsUserProfile EvUser = new EvAdsUserProfile()
    /// {
    ///    EmailAddress = @"test@evado.com.au",
    ///    SamAccountName = userId
    /// };
    /// EvAdsCallResult CallResult;
    /// EvAdConfig Config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    /// EvAdsFacade adFacade = EvAdFacadeFactory.CreateFacade(Config); 
    /// EvAdsUserProfile evUser = null
    /// EvAdsCallResult callResult = adFacade.FindAdUserById(EvUserId, out evUser);
    /// </code>
    /// </example>
    //  ---------------------------------------------------------------------------------- 
    public EvAdsCallResult FindAdUserById ( string EvUserId, out EvAdsUserProfile EvUser )
    {
      //EvAdsUserProfile
      // Validate EvUserId:string parameter
      //
      bool validation = StringParamValidator ( EvUserId );
      if ( validation == false )
      {
        EvUser = null;
        return EvAdsCallResult.Invalid_Argument;
      }

      //
      // Get UserPrincipalFromAD with SamAccountName
      //
      UserPrincipal userPrincipal = GetUserPrincipalWithId ( EvUserId );
      if ( userPrincipal == null )
      {
        EvUser = null;
        return EvAdsCallResult.Object_Not_Found;
      }

      //
      // Creating new User in ADS
      //
      using ( userPrincipal )
      {
        EvUser = CreateEvUserWithUserPrincipal ( userPrincipal, true );
      }

      //
      // if evVUser is null, there must be error.
      //
      if ( EvUser != null )
      {
        return EvAdsCallResult.Success;
      }
      else
      {
        return EvAdsCallResult.Operation_Failure_In_ADS;
      }
    }

    public EvAdsCallResult FindAdUserByEmail ( string Email, out EvAdsUserProfile EvUser )
    {
      //EvAdsUserProfile
      // Validate EvUserId:string parameter
      //
      bool validation = StringParamValidator ( Email );
      if ( validation == false )
      {
        EvUser = null;
        return EvAdsCallResult.Invalid_Argument;
      }

      //
      // Get UserPrincipalFromAD with SamAccountName
      //
      UserPrincipal userPrincipal = GetUserPrincipalByEmail ( Email );
      if ( userPrincipal == null )
      {
        EvUser = null;
        return EvAdsCallResult.Object_Not_Found;
      }

      //
      // Creating new User in ADS
      //
      using ( userPrincipal )
      {
        EvUser = CreateEvUserWithUserPrincipal ( userPrincipal, true );
      }

      //
      // if evVUser is null, there must be error.
      //
      if ( EvUser != null )
      {
        return EvAdsCallResult.Success;
      }
      else
      {
        return EvAdsCallResult.Operation_Failure_In_ADS;
      }
    }

    public EvAdsCallResult FindAdUserByPasswordResetToken ( string Token, out EvAdsUserProfile EvUser )
    {
      //EvAdsUserProfile
      // Validate EvUserId:string parameter
      //
      bool validation = StringParamValidator ( Token );
      if ( validation == false )
      {
        EvUser = null;
        return EvAdsCallResult.Invalid_Argument;
      }

      //
      // Get UserPrincipalFromAD with SamAccountName
      //
      UserPrincipal userPrincipal = GetUserPrincipalByPasswordResetToken ( Token );
      if ( userPrincipal == null )
      {
        EvUser = null;
        return EvAdsCallResult.Object_Not_Found;
      }

      //
      // Creating new User in ADS
      //
      using ( userPrincipal )
      {
        EvUser = CreateEvUserWithUserPrincipal ( userPrincipal, true );
      }

      //
      // if evVUser is null, there must be error.
      //
      if ( EvUser != null )
      {
        return EvAdsCallResult.Success;
      }
      else
      {
        return EvAdsCallResult.Operation_Failure_In_ADS;
      }
    }


    //  ==================================================================================
    /// <summary>
    /// Creates the new EvGroup.
    /// </summary>
    /// <param name="EvGroup">EvAdsGroupProfile.</param>
    /// <returns>EvAdsCallResult.</returns>
    /// <example>
    /// <code numberLines="true">
    /// string EvGroupName = "Group" + Guid.NewGuid().ToString();
    /// EvAdsGroupProfile EvGroup = new EvAdsGroupProfile()
    /// {
    ///     Name = EvGroupName,
    ///     Description = "New Description"
    /// };
    /// EvAdConfig config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    /// EvAdsFacade adFacade = EvAdFacadeFactory.CreateFacade(config); 
    /// EvAdsCallResult callResult = adFacade.CreateNewGroup(EvGroup);
    /// </code>
    /// </example>
    //  ---------------------------------------------------------------------------------- 
    public EvAdsCallResult CreateNewGroup ( EvAdsGroupProfile EvGroup )
    {
      //
      // Validate EvGroup:EvAdsGroupProfile
      //
      bool validation = EvAdsGroupParamValidator ( EvGroup );
      if ( validation == false )
      {
        return EvAdsCallResult.Invalid_Argument;
      }

      //
      //Get PrincipalContext to access ADS
      //
      PrincipalContext principalContext = GroupsPrincipalContext ( );

      //
      //Create new Group in ADS
      //
      using ( principalContext )
      {
        var gp = CreateGroupPrincipalWithAdGroup ( EvGroup );
        gp.Dispose ( );
      }

      return EvAdsCallResult.Success;
    }

    //  ==================================================================================
    /// <summary>
    /// Finds Group by name in Ads.
    /// </summary>
    /// <param name="EvGroupName">string.</param>
    /// <param name="CallResult">EvAdsCallResult.</param>
    /// <returns>EvAdsGroupProfile.</returns>
    /// <example>
    /// <code numberLines="true">
    /// string EvGroupName = "Group" + Guid.NewGuid().ToString();
    /// EvAdsGroupProfile EvGroup = new EvAdsGroupProfile()
    /// {
    ///     Name = EvGroupName,
    ///     Description = "New Description"
    /// };
    /// 
    /// EvAdConfig Config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    /// EvAdsFacade adFacade = EvAdFacadeFactory.CreateFacade(Config); 
    /// EvAdsGroupProfile evGroup; 
    /// EvAdsCallResult callResult = adFacade.FindAdGroupByName(EvGroupName, out evGroup);
    /// </code>
    /// </example>
    //  ---------------------------------------------------------------------------------- 
    public EvAdsCallResult FindAdGroupByName ( string EvGroupName, out EvAdsGroupProfile EvGroup )
    {
      //
      // Validate EvGroup:EvAdsGroupProfile
      //
      var validation = StringParamValidator ( EvGroupName );
      if ( validation == false )
      {
        EvGroup = null;
        return EvAdsCallResult.Invalid_Argument;
      }

      //
      // Get GroupPrincipalFromAD by Group's Name
      //
      var groupPrincipal = GetGroupPrincipalWithName ( EvGroupName );
      if ( groupPrincipal == null )
      {
        EvGroup = null;
        return EvAdsCallResult.Object_Not_Found;
      }

      //
      // Find Group
      //
      using ( groupPrincipal )
      {
        EvGroup = CreateEvGroupWithGroupPrincipal ( groupPrincipal, true );
      }

      if ( EvGroup == null )
      {
        return EvAdsCallResult.Object_Not_Found;
      }
      else
      {
        return EvAdsCallResult.Success;
      }
    }

    //  ==================================================================================
    /// <summary>
    /// Finds Group by name in Ads.
    /// </summary>
    /// <param name="GroupGuid">Guid.</param>
    /// <param name="EvGroup">EvAdsGroupProfile.</param>
    /// <returns>EvAdsGroupProfile.</returns>
    //  ---------------------------------------------------------------------------------- 
    public EvAdsCallResult FindAdGroupByGuid ( Guid GroupGuid, out EvAdsGroupProfile EvGroup )
    {
      //
      // Get GroupPrincipalFromAD by Group's Guid
      //
      var principalContext = RootPrincipalContext ( );
      var groupPrincipal = GroupPrincipal.FindByIdentity ( principalContext, IdentityType.Guid, GroupGuid.ToString ( ) );

      if ( groupPrincipal == null )
      {
        EvGroup = null;
        return EvAdsCallResult.Object_Not_Found;
      }

      //
      // Find Group
      //
      using ( groupPrincipal )
      {
        EvGroup = CreateEvGroupWithGroupPrincipal ( groupPrincipal, true );
      }

      if ( EvGroup == null )
      {
        return EvAdsCallResult.Object_Not_Found;
      }
      else
      {
        return EvAdsCallResult.Success;
      }
    }


    //  ==================================================================================
    /// <summary>
    /// Registers the user to a group in ADS
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <param name="EvGroup">EvAdsGroupProfile.</param>
    /// <returns>EvAdsCallResult.</returns>
    /// <example>
    /// <code numberLines="true">
    /// string userId = "User" + Guid.NewGuid().ToString();
    /// EvAdsUserProfile EvUser = new EvAdsUserProfile()
    /// {
    ///    EmailAddress = @"test@evado.com.au",
    ///    SamAccountName = userId
    /// };
    /// 
    /// string EvGroupName = "Group" + Guid.NewGuid().ToString();
    /// EvAdsGroupProfile EvGroup = new EvAdsGroupProfile()
    /// {
    ///     Name = EvGroupName,
    ///     Description = "New Description"
    /// };
    /// 
    /// EvAdConfig Config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    /// EvAdsFacade adFacade = EvAdFacadeFactory.CreateFacade(Config); 
    /// EvAdsCallResult CallResult = adFacade.RegisterUserToAGroup(EvUser, EvGroup);
    /// </code>
    /// </example>
    //  ---------------------------------------------------------------------------------- 
    public EvAdsCallResult RegisterUserToAGroup ( EvAdsUserProfile EvUser, EvAdsGroupProfile EvGroup )
    {
      //
      // Action Delegate to register a user to a group
      //
      Action<UserPrincipal, GroupPrincipal> actionToRegister = ( u, g ) =>
      {
        g.Members.Add ( u );
        g.Save ( );
      };

      return RegisteringAndUnregisteringTemplateForSingleOperation ( EvUser, EvGroup, actionToRegister );
    }

    //  ==================================================================================
    /// <summary>
    /// Registers the EvUser to multiple groups in ADS.
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <param name="EvGroups">List&lt;EvAdsGroupProfile&gt;.</param>
    /// <returns>EvAdsCallResult.</returns>
    /// <example>
    /// <code numberLines="true">
    /// string userId = "User" + Guid.NewGuid().ToString();
    /// EvAdsUserProfile EvUser = new EvAdsUserProfile()
    /// {
    ///    EmailAddress = @"test@evado.com.au",
    ///    SamAccountName = userId
    /// };
    /// 
    /// string evGroupName1 = "Group" + Guid.NewGuid().ToString();
    /// EvAdsGroupProfile evGroup1 = new EvAdsGroupProfile()
    /// {
    ///     Name = EvGroupName,
    ///     Description = "New Description"
    /// };
    /// string evGroupName2 = "Group" + Guid.NewGuid().ToString();
    /// EvAdsGroupProfile evGroup2 = new EvAdsGroupProfile()
    /// {
    ///     Name = EvGroupName,
    ///     Description = "New Description"
    /// };
    /// 
    /// List&lt;EvAdsGroupProfile&gt; EvGroups = new List&lt;EvAdsGroupProfile&gt;();
    /// EvGroups.Add(evGroup1);
    /// EvGroups.Add(evGroup2);
    ///
    /// EvAdConfig Config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    /// EvAdsFacade adFacade = EvAdFacadeFactory.CreateFacade(Config); 
    /// EvAdsCallResult CallResult = adFacade.RegisterUserToMultipleGroups(EvUser, EvGroups);
    /// </code>
    /// </example>
    //  ---------------------------------------------------------------------------------- 
    public EvAdsCallResult RegisterUserToMultipleGroups ( EvAdsUserProfile EvUser, List<EvAdsGroupProfile> EvGroups )
    {
      //
      // Action Delegate to register a user to multiple groups
      //
      Action<UserPrincipal, GroupPrincipal> actionToRegister = ( u, g ) =>
      {
        g.Members.Add ( u );
        g.Save ( );
      };

      return RegisteringAndUnregisteringTemplateForMultipleOperation ( EvUser, EvGroups, actionToRegister );
    }

    //  ==================================================================================
    /// <summary>
    /// Unregister the EvUser from single EvGroup.
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <param name="EvGroup">EvAdsGroupProfile.</param>
    /// <returns>EvAdsCallResult.</returns>
    /// <example>
    /// <code numberLines="true">
    /// string userId = "User" + Guid.NewGuid().ToString();
    /// EvAdsUserProfile EvUser = new EvAdsUserProfile()
    /// {
    ///    EmailAddress = @"test@evado.com.au",
    ///    SamAccountName = userId
    /// };
    /// 
    /// string EvGroupName = "Group" + Guid.NewGuid().ToString();
    /// EvAdsGroupProfile EvGroup = new EvAdsGroupProfile()
    /// {
    ///     Name = EvGroupName,
    ///     Description = "New Description"
    /// };
    /// 
    /// EvAdConfig Config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    /// EvAdsFacade adFacade = EvAdFacadeFactory.CreateFacade(Config); 
    /// EvAdsCallResult CallResult = adFacade.UnregisterUserFromSingleGroup(EvUser, EvGroup);
    /// </code>
    /// </example>
    //  ---------------------------------------------------------------------------------- 
    public EvAdsCallResult UnregisterUserFromSingleGroup ( EvAdsUserProfile EvUser, EvAdsGroupProfile EvGroup )
    {
      //
      // Action Delegate to unregister a user to a group
      //
      Action<UserPrincipal, GroupPrincipal> actionToUnregister = ( u, g ) =>
      {
        g.Members.Remove ( u );
        g.Save ( );
      };

      return RegisteringAndUnregisteringTemplateForSingleOperation ( EvUser, EvGroup, actionToUnregister );
    }

    //  ==================================================================================
    /// <summary>
    /// Unregister the EvUser from multiple groups.
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <param name="EvGroups">List&lt;EvAdsGroupProfile&gt;.</param>
    /// <returns>EvAdsCallResult.</returns>
    /// <example>
    /// <code numberLines="true">
    /// string userId = "User" + Guid.NewGuid().ToString();
    /// EvAdsUserProfile EvUser = new EvAdsUserProfile()
    /// {
    ///    EmailAddress = @"test@evado.com.au",
    ///    SamAccountName = userId
    /// };
    /// 
    /// string evGroupName1 = "Group" + Guid.NewGuid().ToString();
    /// EvAdsGroupProfile evGroup1 = new EvAdsGroupProfile()
    /// {
    ///     Name = EvGroupName,
    ///     Description = "New Description"
    /// };
    /// string evGroupName2 = "Group" + Guid.NewGuid().ToString();
    /// EvAdsGroupProfile evGroup2 = new EvAdsGroupProfile()
    /// {
    ///     Name = EvGroupName,
    ///     Description = "New Description"
    /// };
    /// 
    /// List&lt;EvAdsGroupProfile&gt; EvGroups = new List&lt;EvAdsGroupProfile&gt;();
    /// EvGroups.Add(evGroup1);
    /// EvGroups.Add(evGroup2);
    ///
    /// EvAdConfig Config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    /// EvAdsFacade adFacade = EvAdFacadeFactory.CreateFacade(Config); 
    /// EvAdsCallResult CallResult = adFacade.UnregisterUserFromMultipleGroups(EvUser, EvGroups);
    /// </code>
    /// </example>
    //  ---------------------------------------------------------------------------------- 
    public EvAdsCallResult UnregisterUserFromMultipleGroups ( EvAdsUserProfile EvUser, List<EvAdsGroupProfile> EvGroups )
    {
      //
      // Action Delegate to unregister a user to multiple groups
      //
      Action<UserPrincipal, GroupPrincipal> actionToUnregister = ( u, g ) =>
      {
        g.Members.Remove ( u );
        g.Save ( );
      };

      return RegisteringAndUnregisteringTemplateForMultipleOperation ( EvUser, EvGroups, actionToUnregister );
    }

    //  ==================================================================================
    /// <summary>
    /// Get all users in ADS with group or without group
    /// According to parameter value
    /// </summary>
    /// <param name="WithGroup">bool.</param>
    /// <returns>List&lt;EvAdsUserProfile&gt;.</returns>
    /// <example>
    /// <code numberLines="true">
    /// EvAdConfig Config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    /// EvAdsFacade adFacade = EvAdFacadeFactory.CreateFacade(Config); 
    /// List&lt;EvAdsUserProfile&gt; allEvUser = adFacade.AllEvUsers(false);
    /// </code>
    /// </example>
    //  ---------------------------------------------------------------------------------- 
    public List<EvAdsUserProfile> AllEvUsers ( bool WithGroup )
    {
      this.LogMethod ( "AllEvUsers method" );
      //
      // Preparing PrincipalSearcher
      //
      PrincipalContext userPrincipalContext = UsersPrincipalContext ( );
      UserPrincipal userPrinciapl = new UserPrincipal ( userPrincipalContext );

      PrincipalSearcher principalSearcher = new PrincipalSearcher ( userPrinciapl );

      //
      // Get Every User
      //
      return ( from UserPrincipal principal in principalSearcher.FindAll ( )
               select CreateEvUserWithUserPrincipal ( principal, WithGroup )
              ).ToList ( );
    }

    //  ==================================================================================
    /// <summary>
    /// Get all users after being filtered in ADS
    /// Filter is parameter value
    /// </summary>
    /// <param name="WithGroup">bool.</param>
    /// <returns>List&lt;EvAdsUserProfile&gt;.</returns>
    /// <example>
    /// <code numberLines="true">
    /// EvAdConfig Config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    /// EvAdsFacade adFacade = EvAdFacadeFactory.CreateFacade(Config); 
    /// string Filter = "Evado";
    /// List&lt;EvAdsUserProfile&gt; allEvUser = adFacade.AllEvUsersAfterBeingFiltered(Filter);
    /// </code>
    /// </example>
    //  ---------------------------------------------------------------------------------- 
    public List<EvAdsUserProfile> AllEvUsersAfterBeingFiltered ( string Filter )
    {
      this.LogMethod ( "AllEvUsersAfterBeingFiltered method" );
      List<EvAdsUserProfile> userProfiles = new List<EvAdsUserProfile> ( );

      //
      // Preparing PrincipalSearcher
      //
      PrincipalContext userPrincipalContext = UsersPrincipalContext ( );
      UserPrincipal userPrincipal = new UserPrincipal ( userPrincipalContext );
      PrincipalSearcher principalSearcher = new PrincipalSearcher ( userPrincipal );

      //
      // Get All Users.
      //
      foreach ( UserPrincipal user in principalSearcher.FindAll ( ) )
      {

        //var groupPrincipals = user.GetGroups ( );

        //foreach ( GroupPrincipal group in groupPrincipals )

        System.DirectoryServices.DirectoryEntry de = ( System.DirectoryServices.DirectoryEntry ) user.GetUnderlyingObject ( );

        PropertyValueCollection groups = de.Properties [ "memberOf" ];

        foreach ( string groupDN in groups )
        {
          //
          // If filter is empty or group name is matched with filter, add user to list.
          //
          if ( Filter == string.Empty
            || /*group.Name*/groupDN.Contains ( "ORG_" + Filter.ToUpper ( ) + "_" )
            || groupDN.Contains ( "ROL_" + Filter.ToUpper ( ) + "_" ) )
          {
            EvAdsUserProfile evAdsUser = new EvAdsUserProfile ( );
            evAdsUser.SamAccountName = user.SamAccountName;
            evAdsUser.DisplayName = user.DisplayName;
            evAdsUser.EmailAddress = user.EmailAddress;
            evAdsUser.VoiceTelephoneNumber = user.VoiceTelephoneNumber;
            userProfiles.Add ( evAdsUser );
            break;
          }
        }

      }

      userProfiles = userProfiles.OrderBy ( o => o.SamAccountName ).ToList ( );

      return userProfiles;
    } //End Method AllEvUsersAfterBeingFiltered

    //  ==================================================================================
    /// <summary>
    /// Get all groups 
    /// </summary>
    /// <param name="WithGroup">bool.</param>
    /// <returns>List&lt;EvAdsGroupProfile&gt;.</returns>
    /// <example>
    /// <code numberLines="true">
    /// EvAdConfig Config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    /// EvAdsFacade adFacade = EvAdFacadeFactory.CreateFacade(Config); 
    /// List&lt;EvAdsGroupProfile&gt; allEvUser = adFacade.AllEvGroups();
    /// </code>
    /// </example>
    //  ---------------------------------------------------------------------------------- 
    public List<EvAdsGroupProfile> getAllGroups ( )
    {
      this.LogMethod ( "getAllGroups method" );
      //
      // Preparing PrincipalSearcher
      //
      PrincipalContext groupPrincipalContext = GroupsPrincipalContext ( );
      GroupPrincipal groupPrincipal = new GroupPrincipal ( groupPrincipalContext );

      PrincipalSearcher principalSearcher = new PrincipalSearcher ( groupPrincipal );

      //
      // Get All Group
      //
      return ( from GroupPrincipal principal in principalSearcher.FindAll ( )
               select CreateEvGroupWithGroupPrincipal ( principal, false )
              ).ToList ( );

    }

    #endregion //End Public Methods

    #region Private Methods
    //  ==================================================================================
    /// <summary>
    /// Check 'EvUserString's' string value to see if it is null or empty
    /// if it is null or empty set 'UserPrincipalString' as null, since ADS does allow to have string.empty as their value.
    /// </summary>
    /// <param name="UserPrincipalString">string.</param>
    /// <param name="EvUserString">string.</param>
    //  ---------------------------------------------------------------------------------- 
    private string ChekcEmptyOrNullString ( string UserPrincipalString, string EvUserString )
    {
      if ( UserPrincipalString != EvUserString )
      {
        if ( string.IsNullOrEmpty ( EvUserString ) )
        {
          UserPrincipalString = null;
        }
        else
        {
          UserPrincipalString = EvUserString;
        }
      }
      return UserPrincipalString;
    }

    //  ==================================================================================
    /// <summary>
    /// Create PrincipalContext for Container.
    /// Container is the top level of LDAP to search
    /// </summary>
    /// <param name="Container">String.</param>
    /// <returns>PrincipalContext.</returns>
    //  ---------------------------------------------------------------------------------- 
    private PrincipalContext PrincipalContextForContainer ( String Container )
    {
      var principalContext = new PrincipalContext ( _evAdConfig.ContextType,
                                                  _evAdConfig.Server,
                                                  Container,
                                                  _evAdConfig.AdminName,
                                                  _evAdConfig.AdminPassword );
      return principalContext;
    }

    //  ==================================================================================
    /// <summary>
    /// Create PrincipalContext for Root Container.
    /// </summary>
    /// <returns>PrincipalContext.</returns>
    //  ---------------------------------------------------------------------------------- 
    private PrincipalContext RootPrincipalContext ( )
    {
      return PrincipalContextForContainer ( _evAdConfig.RootContainer );
    }

    //  ==================================================================================
    /// <summary>
    /// Create PrincipalContext for Users Container.
    /// </summary>
    /// <returns>PrincipalContext.</returns>
    //  ---------------------------------------------------------------------------------- 
    private PrincipalContext UsersPrincipalContext ( )
    {
      return PrincipalContextForContainer ( _evAdConfig.UsersContainer );
    }

    //  ==================================================================================
    /// <summary>
    /// Create PrincipalContext for Groups Container.
    /// </summary>
    /// <returns>PrincipalContext.</returns>
    //  ---------------------------------------------------------------------------------- 
    private PrincipalContext GroupsPrincipalContext ( )
    {
      return PrincipalContextForContainer ( _evAdConfig.GroupsContainer );
    }

    //  ==================================================================================
    /// <summary>
    /// Creates the GroupPrincipal of MS ADS with EvAdsGroupProfile value.
    /// </summary>
    /// <param name="EvGroup">EvAdsGroupProfile.</param>
    /// <returns>GroupPrincipal.</returns>
    //  ---------------------------------------------------------------------------------- 
    private GroupPrincipal CreateGroupPrincipalWithAdGroup ( EvAdsGroupProfile EvGroup )
    {
      //
      // Prepare GroupPrincipalFromAD to manipulate.
      //
      PrincipalContext groupsContext = GroupsPrincipalContext ( );
      GroupPrincipal groupPrincipal = new GroupPrincipal ( groupsContext );

      //
      // Set properties and Save
      //
      groupPrincipal.Description = EvGroup.Description;
      groupPrincipal.DisplayName = EvGroup.DisplayName;
      if ( EvGroup.GroupScope != null )
      {
        groupPrincipal.GroupScope = EvGroup.GroupScope;
      }
      groupPrincipal.IsSecurityGroup = EvGroup.IsSecurityGroup ?? true;
      groupPrincipal.Name = EvGroup.Name;

      groupPrincipal.Save ( );

      return groupPrincipal;
    }

    //  ==================================================================================
    /// <summary>
    /// Creates the UserPrincipal of MS ADS with EvAdsUserProfile value.
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <returns>EvAdsCallResult.</returns>
    //  ---------------------------------------------------------------------------------- 
    private EvAdsCallResult CreateUserPrincipalWithAdUser (
      EvAdsUserProfile EvUser )
    {
      this.LogMethod ( "CreateUserPrincipalWithAdUser method" );
      this.LogValue ( "UserId: " + EvUser.UserId );
      this.LogValue ( "SamAccountName: " + EvUser.SamAccountName );
      this.LogValue ( "UserPrincipalName: " + EvUser.UserPrincipalName );
      this.LogValue ( "UserId.Length: " + EvUser.UserId.Length );

      try
      {
        //
        // Prepare UserPrincipalFromAD to manipulate.
        //
        PrincipalContext usersContext = this.UsersPrincipalContext ( );
        this.LogValue ( " usersContext.Name: " + usersContext.Name );

        using ( usersContext )
        {
          //
          // create a new user principal instance
          //
          UserPrincipal userPrincipal = new UserPrincipal ( usersContext );

          this.LogValue ( " usersContext.Name: " + userPrincipal.Name );

          using ( userPrincipal )
          {
            //
            // Set Attributes
            //
            userPrincipal.Name = EvUser.UserId;
            if ( String.IsNullOrEmpty ( EvUser.SamAccountName ) == false )
            {
              userPrincipal.SamAccountName = EvUser.SamAccountName;
            }
            if ( String.IsNullOrEmpty ( EvUser.UserPrincipalName ) == false )
            {
              userPrincipal.UserPrincipalName = EvUser.UserPrincipalName;
            }
            if ( String.IsNullOrEmpty ( EvUser.GivenName ) == false )
            {
              userPrincipal.GivenName = EvUser.GivenName;
            }
            if ( String.IsNullOrEmpty ( EvUser.Surname ) == false )
            {
              userPrincipal.Surname = EvUser.Surname;
            }
            if ( String.IsNullOrEmpty ( EvUser.DisplayName ) == false )
            {
              userPrincipal.Name = EvUser.DisplayName;
            }
            if ( String.IsNullOrEmpty ( EvUser.Description ) == false )
            {
              userPrincipal.Description = EvUser.Description;
            }
            if ( String.IsNullOrEmpty ( EvUser.EmailAddress ) == false )
            {
              userPrincipal.EmailAddress = EvUser.EmailAddress;
            }
            if ( String.IsNullOrEmpty ( EvUser.VoiceTelephoneNumber ) == false )
            {
              userPrincipal.VoiceTelephoneNumber = EvUser.VoiceTelephoneNumber;
            }
            userPrincipal.Enabled = true;
            userPrincipal.UserCannotChangePassword = false;
            userPrincipal.PasswordNeverExpires = true;
            userPrincipal.Save ( );

            if ( String.IsNullOrEmpty ( EvUser.Password ) == false )
            {
              userPrincipal.SetPassword ( EvUser.Password );
              userPrincipal.Save ( );
              this.LogValue ( "Password added." );
            }

            //
            // Do task relating to GroupPricipal
            //
            this.CreateOrUpdateGroupPrincipal ( EvUser, userPrincipal );

          }//END userPrincipal

        }//END usersContext 
      }
      catch ( Exception Ex )
      {
        this.LogValue ( Evado.Model.EvStatics.getException ( Ex ) );

        return EvAdsCallResult.Operation_Failure_In_ADS;
      }

      return EvAdsCallResult.Success;

    }//ENd CreateUserPrincipalWithAdUser method

    //  ==================================================================================
    /// <summary>
    /// Creates the UserPrincipal of MS ADS with EvAdsUserProfile value.
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <returns>EvAdsCallResult.</returns>
    //  ---------------------------------------------------------------------------------- 
    public EvAdsCallResult SetPassword (
      String SamAccountName,
      String Password )
    {
      this.LogMethod ( "SetPassword method" );
      this.LogValue ( "SamAccountName: " + SamAccountName );
      this.LogValue ( "Password: " + Password );

      try
      {
        //
        // Get UserPrincipalFromAD with SamAccountName
        //
        UserPrincipal userPrincipal = this.GetUserPrincipalWithId ( SamAccountName );
        if ( userPrincipal == null )
        {
          this.LogValue ( "ERROR: Object not found" );
          return EvAdsCallResult.Object_Not_Found;
        }

        userPrincipal.UserCannotChangePassword = false;
        userPrincipal.Enabled = true;

        using ( userPrincipal )
        {
          if ( String.IsNullOrEmpty ( Password ) == false )
          {
            userPrincipal.SetPassword ( Password );
            userPrincipal.Save ( );
            this.LogValue ( "Password updated." );
          }
        }
      }
      catch ( Exception Ex )
      {
        this.LogValue ( Evado.Model.EvStatics.getException ( Ex ) );
        return EvAdsCallResult.Operation_Failure_In_ADS;
      }

      return EvAdsCallResult.Success;

    }//ENd CreateUserPrincipalWithAdUser method

    //  ==================================================================================
    /// <summary>
    /// Creates the or update  GroupPrincipal where UserPrincipal is registered.
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <param name="UserPrincipalFromAD">UserPrincipal.</param>
    //  ---------------------------------------------------------------------------------- 
    private void CreateOrUpdateGroupPrincipal (
      EvAdsUserProfile EvUser,
      UserPrincipal UserPrincipalFromAD )
    {
      this.LogMethod ( "CreateOrUpdateGroupPrincipal method" );
      //
      // Get EvGroups that 'EvUser' has
      //
      List<EvAdsGroupProfile> userGroupList = EvUser.EvGroups;
      List<GroupPrincipal> groupPrincipalsToAddUser = new List<GroupPrincipal> ( );

      if ( userGroupList == null )
      {
        this.LogValue ( "User Groups are null." );
        return;
      }

      for ( int count = 0; count < userGroupList.Count; count++ )
      {
        if ( userGroupList [ count ] == null )
        {
          this.LogValue ( "NULL user group object found." );
          userGroupList.RemoveAt ( count );
          count--;
        }
      }

      //
      // Create comparer for group name comparison
      //
      IEqualityComparer<Principal> principalComparer = new EvPrincipalComparer ( );

      //
      // create the list of groups to add to the user.
      //
      groupPrincipalsToAddUser = userGroupList.Select ( eg => GetGroupPrincipal ( eg )
                                                                ?? CreateGroupPrincipalWithAdGroup ( eg ) ).ToList ( );

      try
      {
        //
        // If there is any group that EvUser has but not registered in ADS register it.
        //
        foreach ( GroupPrincipal groupPrincipal in groupPrincipalsToAddUser )
        {
          if ( groupPrincipal == null )
          {
            this.LogValue ( "Null group Principal found." );
            continue;
          }

          if ( groupPrincipal.Members.Contains ( UserPrincipalFromAD, principalComparer ) == false )
          {
            groupPrincipal.Members.Add ( UserPrincipalFromAD );

            groupPrincipal.Save ( );
          }
        }
      }
      catch( Exception Ex )
      {
        this.LogValue ( "EXCEPTION GENERATED SAVING GROUPS." );
        this.LogValue ( Evado.Model.EvStatics.getException ( Ex ) );
      }
    }//END method

    //  ==================================================================================
    /// <summary>
    /// Creates the EvAdsUserProfile with UserPrincipal of ADS.
    /// </summary>
    /// <param name="UserPrincipalFromAD">UserPrincipal.</param>
    /// <param name="WithGroup">bool.</param>
    /// <returns>EvAdsUserProfile.</returns>
    //  ---------------------------------------------------------------------------------- 
    private EvAdsUserProfile CreateEvUserWithUserPrincipal (
      UserPrincipal UserPrincipalFromAD,
      bool WithGroup )
    {
      // this.LogMethod ( "CreateEvUserWithUserPrincipal method" );
      //
      // Convert UserPrincipalFromAD object to EvAdsUserProfile instance
      //
      EvAdsUserProfile evUser = EvUserInstance ( UserPrincipalFromAD );

      //
      // Get additional Properties
      //
      this._evAdsLdapHelper.GetAdditionalUserProperty ( evUser );


      if ( WithGroup == true )
      {
        this.AddGroupsToEvAdsUser ( ref evUser, UserPrincipalFromAD );
      }

      return evUser;
    }

    //  ==================================================================================
    /// <summary>
    /// This method add groups the the Ads user.
    /// </summary>
    /// <param name="evUser">EvAdsUserProfile</param>
    /// <param name="userPrincipal">UserPrincipal</param>
    //  ---------------------------------------------------------------------------------- 
    public void AddGroupsToEvAdsUser (
      ref EvAdsUserProfile evUser,
      UserPrincipal userPrincipal = null )
    {

      System.DirectoryServices.DirectoryEntry de = default ( System.DirectoryServices.DirectoryEntry );

      //
      // Get UserPrincipalFromAD with SamAccountName
      //
      if ( userPrincipal == null )
      {
        userPrincipal = GetUserPrincipalWithId ( evUser.UserId );
      }

      if ( userPrincipal != null )
      {
        de = ( System.DirectoryServices.DirectoryEntry ) userPrincipal.GetUnderlyingObject ( );
      }

      //
      // if WithGroup is true, add EvGroup values to EvUser
      //

      // NOTE: not using GetGroups() as it seems to only work when the machine is attached to the domain.
      // Instead using the underlying DirectoryEntry methods
      //
      if ( de != null && /*UserPrincipalFromAD.GetGroups()*/ de.Properties [ "memberOf" ].Count > 0 )
      {
        /*
        PrincipalSearchResult<Principal> groupPrincipals = UserPrincipalFromAD.GetGroups ( );

        foreach ( Principal principal in groupPrincipals )
        */
        foreach ( string groupDN in de.Properties [ "memberOf" ] )
        {
          //var adGroup = CreateEvGroupWithGroupPrincipal((GroupPrincipal)principal, false);

          foreach ( var part in groupDN.Split ( ',' ) )
          {
            if ( part.StartsWith ( "CN=" ) )
            {
              var group = part.Replace ( "CN=", "" );
              var groupPrincipal = GetGroupPrincipalWithName ( group );
              if ( groupPrincipal != null )
              {
                var adGroup = CreateEvGroupWithGroupPrincipal ( groupPrincipal, false );
                evUser.AddEvGroup ( adGroup );
              }
            }
          }
        }
      }
    }

    //  ==================================================================================
    /// <summary>
    /// Creates the EvAdsUserProfile with UserPrincipal of ADS.
    /// </summary>
    /// <param name="UserPrincipalFromAD">UserPrincipal.</param>
    /// <returns>EvAdsUserProfile.</returns>
    //  ----------------------------------------------------------------------------------
    private EvAdsUserProfile EvUserInstance (
      UserPrincipal UserPrincipalFromAD )
    {
      // this.LogMethod ( "EvUserInstance method" );
      //
      // Create EvAdsUserProfile instance and set Property with UserPrincipalFromAD
      //
      EvAdsUserProfile evUser = new EvAdsUserProfile ( UserPrincipalFromAD.SamAccountName )
          {
            UserGuid = UserPrincipalFromAD.Guid,
            Description = UserPrincipalFromAD.Description,
            DisplayName = UserPrincipalFromAD.DisplayName,
            EmailAddress = UserPrincipalFromAD.EmailAddress,
            Enabled = UserPrincipalFromAD.Enabled,
            GivenName = UserPrincipalFromAD.GivenName,
            PasswordNeverExpires = UserPrincipalFromAD.PasswordNeverExpires,
            SamAccountName = UserPrincipalFromAD.SamAccountName,
            Surname = UserPrincipalFromAD.Surname,
            UserCannotChangePassword = UserPrincipalFromAD.UserCannotChangePassword,
            UserPrincipalName = UserPrincipalFromAD.UserPrincipalName,
            VoiceTelephoneNumber = UserPrincipalFromAD.VoiceTelephoneNumber
          };

      return evUser;
    }

    //  ==================================================================================
    /// <summary>
    /// Creates the EvAdsGroupProfile with GroupPrincipal of ADS.
    /// </summary>
    /// <param name="GroupPrincipalFromAD">GroupPrincipal.</param>
    /// <param name="WithMembers">bool.</param>
    /// <returns>EvAdsGroupProfile.</returns>
    //  ---------------------------------------------------------------------------------- 
    private EvAdsGroupProfile CreateEvGroupWithGroupPrincipal (
      GroupPrincipal GroupPrincipalFromAD,
      bool WithMembers )
    {
      // this.LogMethod ( "CreateEvGroupWithGroupPrincipal method" );
      //
      // Get EvAdsGroupProfile instance.
      //
      EvAdsGroupProfile evGroup = EvGroupInstance ( GroupPrincipalFromAD );

      //
      // if WithMembers is true, Add EvAdsUser instance to 'evGroup'
      //
      if ( WithMembers == true )
      {
        PrincipalSearchResult<Principal> userPrincipal = GroupPrincipalFromAD.GetMembers ( );

        foreach ( Principal principal in userPrincipal )
        {
          UserPrincipal up = principal as UserPrincipal;
          if ( up != null )
          {
            EvAdsUserProfile evUser = EvUserInstance ( up );
            evGroup.AddEvUser ( evUser );
          }
        }
      }

      return evGroup;
    }

    //  ==================================================================================
    /// <summary>
    /// Creates the EvAdsGroupProfile with GroupPrincipal of ADS.
    /// </summary>
    /// <param name="GroupPrincipalFromAD">GroupPrincipal.</param>
    /// <returns>EvAdsGroupProfile.</returns>
    //  ----------------------------------------------------------------------------------
    private static EvAdsGroupProfile EvGroupInstance (
      GroupPrincipal GroupPrincipalFromAD )
    {
      //
      // Create EvAdsGroupProfile instance and set properties with 'GroupPrincipalFromAD' properties
      //
      EvAdsGroupProfile evGroup = new EvAdsGroupProfile ( )
          {
            GroupGuid = GroupPrincipalFromAD.Guid,
            Description = GroupPrincipalFromAD.Description,
            DisplayName = GroupPrincipalFromAD.DisplayName,
            GroupScope = GroupPrincipalFromAD.GroupScope,
            IsSecurityGroup = GroupPrincipalFromAD.IsSecurityGroup,
            Name = GroupPrincipalFromAD.Name,

          };
      return evGroup;
    }

    //  ==================================================================================
    /// <summary>
    /// Gets the  UserPrincipalFromAD of ADS.
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <returns>UserPrincipal.</returns>
    //  ---------------------------------------------------------------------------------- 
    private UserPrincipal GetUserPrincipal ( EvAdsUserProfile EvUser )
    {
      // this.LogMethod ( "CreateEvGroupWithGroupPrincipal method" );

      UserPrincipal userPrincipal = GetUserPrincipalWithId ( EvUser.UserId );

      return userPrincipal;
    }

    //  ==================================================================================
    /// <summary>
    /// Gets the  UserPrincipalFromAD of ADS with EvUserId.
    /// EvUserId shall be SamAccountName
    /// </summary>
    /// <param name="EvUserId">string.</param>
    /// <returns>UserPrincipal.</returns>
    //  ---------------------------------------------------------------------------------- 
    private UserPrincipal GetUserPrincipalWithId ( string EvUserId )
    {
      // this.LogMethod ( "GetUserPrincipalWithId method" );

      PrincipalContext principalContext = RootPrincipalContext ( );

      UserPrincipal userPrincipal =
          UserPrincipal.FindByIdentity ( principalContext, IdentityType.SamAccountName, EvUserId );

      return userPrincipal;
    }

    //  ==================================================================================
    /// <summary>
    /// Gets the  UserPrincipalFromAD of ADS with EvUserId.
    /// Guid shall be SamAccountName Guid
    /// </summary>
    /// <param name="Guid">Guid.</param>
    /// <returns>UserPrincipal.</returns>
    //  ---------------------------------------------------------------------------------- 
    private UserPrincipal GetUserPrincipalWithGuid ( string Guid )
    {
      // this.LogMethod ( "GetUserPrincipalWithGuid method" );
      PrincipalContext principalContext = RootPrincipalContext ( );

      UserPrincipal userPrincipal =
          UserPrincipal.FindByIdentity ( principalContext, IdentityType.Guid, Guid );

      return userPrincipal;
    }

    //  ==================================================================================
    /// <summary>
    /// Gets the  UserPrincipalFromAD of ADS with EvUserId.
    /// Email shall be user's registered email account
    /// </summary>
    /// <param name="Email">String.</param>
    /// <returns>UserPrincipal.</returns>
    //  ---------------------------------------------------------------------------------- 
    private UserPrincipal GetUserPrincipalByEmail ( string Email )
    {
      // this.LogMethod ( "GetUserPrincipalByEmail method" );

      DirectorySearcher adSearcher = new DirectorySearcher ( _evAdConfig.LdapString );
      adSearcher.Filter = ( "mail=" + Email.ToLower ( ) );
      SearchResultCollection coll = adSearcher.FindAll ( );
      foreach ( SearchResult item in coll )
      {
        DirectoryEntry directoryEntry = item.GetDirectoryEntry ( );

        return GetUserPrincipalWithGuid ( directoryEntry.Guid.ToString ( ) );
      }

      return null;
    }


    //  ==================================================================================
    /// <summary>
    /// Gets the  UserPrincipalFromAD of ADS with EvUserId.
    /// Token shall be user's password reset token
    /// </summary>
    /// <param name="Token">String.</param>
    /// <returns>UserPrincipal.</returns>
    //  ---------------------------------------------------------------------------------- 
    private UserPrincipal GetUserPrincipalByPasswordResetToken ( string Token )
    {
      // this.LogMethod ( "GetUserPrincipalByPasswordResetToken method" );

      DirectorySearcher adSearcher = new DirectorySearcher ( _evAdConfig.LdapString );
      adSearcher.Filter = ( "info=" + Token.ToLower ( ) + ";*" );
      SearchResultCollection coll = adSearcher.FindAll ( );
      foreach ( SearchResult item in coll )
      {
        DirectoryEntry directoryEntry = item.GetDirectoryEntry ( );
        return GetUserPrincipalWithGuid ( directoryEntry.Guid.ToString ( ) );
      }

      return null;
    }

    //  ==================================================================================
    /// <summary>
    /// Gets the GroupPrincipalFromAD.
    /// </summary>
    /// <param name="EvGroup">EvAdsGroupProfile.</param>
    /// <returns>GroupPrincipal.</returns>
    //  ---------------------------------------------------------------------------------- 
    private GroupPrincipal GetGroupPrincipal ( EvAdsGroupProfile EvGroup )
    {
      // this.LogMethod ( "GetGroupPrincipal method" );

      GroupPrincipal groupPrincipal = GetGroupPrincipalWithName ( EvGroup.Name );

      return groupPrincipal;
    }

    /// <summary>
    /// Gets the GroupPrincipalFromAD with Unique EvGroupName.
    /// </summary>
    /// <param name="EvGroupName">string.</param>
    /// <returns>GroupPrincipal.</returns>
    //  ---------------------------------------------------------------------------------- 
    private GroupPrincipal GetGroupPrincipalWithName ( string EvGroupName )
    {
      // this.LogMethod ( "GetGroupPrincipalWithName method" );
      var principalContext = RootPrincipalContext ( );

      var groupPrincipal =
          GroupPrincipal.FindByIdentity ( principalContext, IdentityType.Name, EvGroupName );

      return groupPrincipal;
    }

    //  ==================================================================================
    /// <summary>
    /// Validate the value of EvAdsUserProfile parameter.
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <returns><c>true</c> if EvUser is not null, <c>false</c> otherwise</returns>
    //  ---------------------------------------------------------------------------------- 
    private bool EvAdsUserParamValidator ( EvAdsUserProfile EvUser )
    {
      return EvUser != null;
    }

    //  ==================================================================================
    /// <summary>
    /// Validate the value of string parameter.
    /// </summary>
    /// <param name="EvUserId">The ad EvUser id.</param>
    /// <returns><c>true</c> if EvUserId is not empty and null , <c>false</c> otherwise</returns>
    //  ---------------------------------------------------------------------------------- 
    private bool StringParamValidator ( string EvUserId )
    {
      return String.IsNullOrEmpty ( EvUserId ) == false;
    }

    //  ==================================================================================
    /// <summary>
    /// Validate the value of EvAdsGroupProfile parameter.
    /// </summary>
    /// <param name="EvGroup">EvAdsGroupProfile.</param>
    /// <returns><c>true</c> if EvGroup is not empty and null, <c>false</c> otherwise</returns>
    private bool EvAdsGroupParamValidator ( EvAdsGroupProfile EvGroup )
    {
      return EvGroup != null;
    }

    //  ==================================================================================
    /// <summary>
    /// Templates Method for APIs that have a single EvAdsUserProfile parameter
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <param name="ActionDelegate">Action&lt;UserPrincipal&gt;.</param>
    /// <returns>EvAdsCallResult.</returns>
    //  ---------------------------------------------------------------------------------- 
    private EvAdsCallResult TemplateForAdUserOperation ( EvAdsUserProfile EvUser, Action<UserPrincipal> ActionDelegate )
    {
      // this.LogMethod ( "TemplateForAdUserOperation method" );
      //
      // Validate 'EvUser' parameter
      //
      bool validation = EvAdsUserParamValidator ( EvUser );
      if ( validation == false )
      {
        return EvAdsCallResult.Invalid_Argument;
      }

      //
      // Get UserPrincipal
      //
      UserPrincipal userPrincipal = GetUserPrincipal ( EvUser );
      if ( userPrincipal == null )
      {
        return EvAdsCallResult.Object_Not_Found;
      }

      //
      // Execute Delegation 
      //
      using ( userPrincipal )
      {
        ActionDelegate ( userPrincipal );
      }

      return EvAdsCallResult.Success;
    }

    //  ==================================================================================
    /// <summary>
    /// Templates Method for APIs that have a single EvAdsGroupProfile parameter
    /// </summary>
    /// <param name="EvGroup">EvAdsGroupProfile.</param>
    /// <param name="ActionDelegate">Action&lt;GroupPrincipal&gt;.</param>
    /// <returns>EvAdsCallResult.</returns>
    //  ---------------------------------------------------------------------------------- 
    private EvAdsCallResult TemplateForAdGroupOperation ( EvAdsGroupProfile EvGroup, Action<GroupPrincipal> ActionDelegate )
    {
      // this.LogMethod ( "TemplateForAdGroupOperation method" );
      //
      // Validate 'EvGroup' parameter
      //
      bool validation = EvAdsGroupParamValidator ( EvGroup );
      if ( validation == false )
      {
        return EvAdsCallResult.Invalid_Argument;
      }

      //
      // Get GroupPrincipal
      //
      GroupPrincipal groupPrincipal = GetGroupPrincipal ( EvGroup );
      if ( groupPrincipal == null )
      {
        return EvAdsCallResult.Object_Not_Found;
      }

      //
      // Execute Delegation 
      //
      using ( groupPrincipal )
      {
        ActionDelegate ( groupPrincipal );
        return EvAdsCallResult.Success;
      }
    }

    //  ==================================================================================
    /// <summary>
    /// Registering the and unregistering template for single EvAdsUserProfile and EvAdsGroupProfile Parameter.
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <param name="EvGroup">EvAdsGroupProfile.</param>
    /// <param name="ActionDelegate">Action&lt;UserPrincipal, GroupPrincipal&gt;.</param>
    /// <returns>EvAdsCallResult.</returns>
    //  ---------------------------------------------------------------------------------- 
    private EvAdsCallResult RegisteringAndUnregisteringTemplateForSingleOperation (
      EvAdsUserProfile EvUser,
      EvAdsGroupProfile EvGroup,
      Action<UserPrincipal,
      GroupPrincipal> ActionDelegate )
    {
      // this.LogMethod ( "RegisteringAndUnregisteringTemplateForSingleOperation method" );
      //
      // Validate 'EvUser'
      //
      bool validationForAdUser = EvAdsUserParamValidator ( EvUser );

      //
      // Validate 'EvGroup'
      //
      bool validationForAdGroup = EvAdsGroupParamValidator ( EvGroup );
      if ( validationForAdUser == false || validationForAdGroup == false )
      {
        return EvAdsCallResult.Invalid_Argument;
      }

      //
      // Get UserPrincipal and GroupPrincipal instances
      //
      UserPrincipal userPrincipal = GetUserPrincipal ( EvUser );
      GroupPrincipal groupPrincipal = GetGroupPrincipal ( EvGroup );

      if ( userPrincipal == null || groupPrincipal == null )
      {
        return EvAdsCallResult.Object_Not_Found;
      }

      //
      // Execute Action Delegate
      //
      using ( groupPrincipal )
      {
        using ( userPrincipal )
        {
          ActionDelegate ( userPrincipal, groupPrincipal );
        }
      }

      return EvAdsCallResult.Success;
    } //End Method RegisteringAndUnregisteringTemplateForSingleOperation

    //  ==================================================================================
    /// <summary>
    /// Registering the and unregistering template for single EvAdsUserProfile and multiple EvAdsGroupProfile Parameter.
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <param name="EvGroups">List&lt;EvAdsGroupProfile>&gt;</param>
    /// <param name="ActionDelegate"> Action&lt;UserPrincipal, GroupPrincipal&gt;.</param>
    /// <returns>EvAdsCallResult.</returns>
    //  ---------------------------------------------------------------------------------- 
    private EvAdsCallResult RegisteringAndUnregisteringTemplateForMultipleOperation (
      EvAdsUserProfile EvUser,
      List<EvAdsGroupProfile> EvGroups,
      Action<UserPrincipal, GroupPrincipal> ActionDelegate )
    {
      // this.LogMethod ( "RegisteringAndUnregisteringTemplateForMultipleOperation method" );
      //
      // Validate 'EvUser'
      //
      bool validationForAdUser = EvAdsUserParamValidator ( EvUser );

      if ( validationForAdUser == false || EvGroups == null )
      {
        return EvAdsCallResult.Invalid_Argument;
      }

      //
      // Get UserPrincipal instance
      //
      UserPrincipal userPrincipal = GetUserPrincipal ( EvUser );
      if ( userPrincipal == null )
      {
        return EvAdsCallResult.Object_Not_Found;
      }

      using ( userPrincipal )
      {
        foreach ( EvAdsGroupProfile adGroup in EvGroups )
        {
          //
          //Get GroupPrincipal instance and validate it
          //
          GroupPrincipal groupPrincipal = GetGroupPrincipal ( adGroup );
          bool validationForAdGroup = EvAdsGroupParamValidator ( adGroup );
          if ( validationForAdGroup == false )
          {
            return EvAdsCallResult.Invalid_Argument;
          }

          if ( groupPrincipal == null )
          {
            return EvAdsCallResult.Object_Not_Found;
          }

          //
          // Execute Action Delegate
          //
          using ( groupPrincipal )
          {
            ActionDelegate ( userPrincipal, groupPrincipal );
          }
        }
      }

      return EvAdsCallResult.Success;
    } //End Method RegisteringAndUnregisteringTemplateForMultipleOperation

    #endregion //End Private Methods

    #region Debug methods.


    private const String CONST_NAME_SPACE = "Evado.ActiveDirectoryServices.EvAdsFacade.";

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
    private void LogInitMethod ( String Value )
    {
      if ( this._LogLevel >= 2 )
      {
        this._ClassLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
        + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
        + EvAdsFacade.CONST_NAME_SPACE + Value );
      }
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
      if ( this._LogLevel >= 2 )
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
    protected void LogMethod ( String Value )
    {
      if ( this._LogLevel >= 2 )
      {
        this._ClassLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
        + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
        + EvAdsFacade.CONST_NAME_SPACE + Value );
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
    private void Log_DebugValue ( String DebugLogString )
    {
      if ( this._LogLevel >= 4 )
      {
        this._ClassLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + DebugLogString );
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END EvAdsFacade class
}
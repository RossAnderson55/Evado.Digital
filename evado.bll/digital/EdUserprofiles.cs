/***************************************************************************************
 * <copyright file="dal\EvFormFields.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
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
using System.Text;

//Evado. namespace references.
using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Bll.Digital
{
  /// <summary>
  /// A business Component used to manage trial roles
  /// The m_xfs.Model.User is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  public class EdUserprofiles : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdUserprofiles ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvUserProfiles.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdUserprofiles ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvUserProfiles.";

      this._Dal_UserProfiles = new Evado.Dal.Digital.EdUserProfiles ( Settings );
    }
    #endregion

    #region Class variables and properties
    // 
    // Instantiate the DAL Class\
    // 
    private Evado.Dal.Digital.EdUserProfiles _Dal_UserProfiles = new Evado.Dal.Digital.EdUserProfiles ( );

    #endregion

    #region Class List methods
    // ==================================================================================
    /// <summary>
    /// This class returns a list of userprofile objects based on OrgId and OrderBy
    /// </summary>
    /// <param name="Type">EdUserProfile.UserTypesList enumerated value</param>
    /// <param name="OrgId">string: an organization identifier</param>
    /// <returns>List of Evado.Model.Digital.EdUserProfile: a list of userprofile objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of userprofile objects
    /// 
    /// 2. Return the list of userprofile objects. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public List<Evado.Model.Digital.EdUserProfile> GetView (
     String Type,
      String OrgId )
    {
      this.LogMethod ( "GetView method." );
      this.LogDebug ( "OrgId: " + OrgId );
      this.LogDebug ( "Type: " + Type );

      List<Evado.Model.Digital.EdUserProfile> profiles = this._Dal_UserProfiles.GetView ( Type, OrgId );
      this.LogClass ( this._Dal_UserProfiles.Log );

      return profiles;

    }//END GetView method.

    // =====================================================================================
    /// <summary>
    /// This method returns a list of UserProfile object
    /// </summary>
    /// <param name="UserType">String: The selection organistion's identifier</param>
    /// <param name="PartialUserId">String: partial string of the user's user identifier</param>
    /// <param name="PartialCommonName">String: partial string of the user's common nbame identifier</param>
    /// <returns>List of Evado.Model.Digital.EdUserProfile: A list of UserProfile objects.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on datatable. 
    /// 
    /// 3. Loop through the table and extract data row to the UserProfile object. 
    /// 
    /// 4. Add the UserProfile object's values to the UserProfiles list. 
    /// 
    /// 5. Return the UserProfiles list. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<Evado.Model.Digital.EdUserProfile> GetView (
      String OrgId,
      String PartialUserId,
      String PartialCommonName )
    {
      this.LogMethod ( "GetView method." );
      this.LogDebug ( "OrgId: " + OrgId );

      List<Evado.Model.Digital.EdUserProfile> profiles = this._Dal_UserProfiles.GetView ( OrgId,
        PartialUserId,
        PartialCommonName);
      this.LogClass ( this._Dal_UserProfiles.Log );

      return profiles;

    }//END GetView method.

    // ==================================================================================
    /// <summary>
    /// This class returns a list of options for userprofile objects based on OrgId and useGuid condition
    /// </summary>
    /// <param name="OrgId">EvUserProfile.UserTypesList enumerated value</param>
    /// <param name="useGuid">Boolean: true, if the Guid is used</param>
    /// <returns>List of Evado.Model.Digital.EdUserProfile: a list of options for userprofile objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of options for userprofile objects
    /// 
    /// 2. Return the list of options for userprofile objects. 
    /// </remarks>
    //  -------------------------------------------------------------------------------------
    public List<EvOption> GetList (
     String Type,
     String OrgId,
      bool useGuid )
    {
      LogMethod ( "GetView" );
      this.LogDebug ( "OrgId: " + OrgId );

      List<EvOption> list = this._Dal_UserProfiles.GetList ( Type, OrgId, useGuid );
      this.LogDebugClass ( this._Dal_UserProfiles.Log );

      return list;

    }//END GetList method

    #endregion

    #region Class  get methods
    // =====================================================================================
    /// <summary>
    /// This method returns true if the user exists in the database.
    /// </summary>
    /// <param name="UserId">String: The user identifier.</param>
    /// <returns>Boolean: True: user exists in the database.</returns>
    // -------------------------------------------------------------------------------------
    public bool ExistingUserId ( string UserId )
    {
      this.LogMethod ( "ExistingUserId" );
      this.LogDebug ( "UserId: " + UserId );

      bool response = this._Dal_UserProfiles.ExistingUserId ( UserId );
      this.LogDebugClass ( this._Dal_UserProfiles.Log );

      this.LogMethodEnd ( "ExistingUserId" );
      return response;

    }//END ExistingUserId method

    // =====================================================================================
    /// <summary>
    /// This method the number of user in the organisatino.
    /// </summary>
    /// <param name="OrgId">String: The organisation identifier.</param>
    /// <returns>Integer</returns>
    // -------------------------------------------------------------------------------------
    public int UserCount ( String UserType )
    {
      this.LogMethod ( "UserCount" );
      this.LogDebug ( "UserType: " + UserType );

      int response = this._Dal_UserProfiles.UserCount ( UserType );
      this.LogDebugClass ( this._Dal_UserProfiles.Log );

      this.LogMethodEnd ( "UserCount" );
      return response;

    }//END UserCount method

    // =====================================================================================
    /// <summary>
    /// This class retrieves a user profile with a specific connection String.
    /// </summary>
    /// <param name="ConnectionStringKey">string: the database connection string config key</param>
    /// <param name="UserId">string: a user identifier</param>
    /// <returns>Evado.Model.Digital.EdUserProfile: a user profile object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method to retrieve a userProfile object based on UserId
    /// 
    /// 2. Return the UserProfile object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdUserProfile getItem ( string ConnectionStringKey, string UserId )
    {
      this.FlushLog ( );
      this.LogMethod ( "getItem" );
      Evado.Bll.EvStaticSetting.ConnectionStringKey = ConnectionStringKey;

      EdUserProfile userProfile = getItem ( UserId );

      Evado.Bll.EvStaticSetting.ResetConnectionString ( );
      return userProfile;
    }

    // =====================================================================================
    /// <summary>
    /// This class retrieves the Userprofile object based on UserId
    /// </summary>
    /// <param name="UserId">string: a user identifier</param>
    /// <returns>Evado.Model.Digital.EdUserProfile: a user profile object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method to retrieve a userProfile object based on UserId
    /// 
    /// 2. Return the UserProfile object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public Evado.Model.Digital.EdUserProfile getItem ( string UserId )
    {
      this.LogMethod ( "getItem" );
      this.LogDebug ( "UserId: " + UserId );
      //
      // Initialise the methods variables and objects.
      // 
      Evado.Model.Digital.EdUserProfile userProfile = new Evado.Model.Digital.EdUserProfile ( );

      // 
      // Retrieve the user object from the database.
      // 
      userProfile = this._Dal_UserProfiles.GetItem ( UserId );

      this.LogClass ( this._Dal_UserProfiles.Log );

      // 
      // If the normal userId fails try the Active Directory logon.
      // 
      if ( userProfile.UserId == String.Empty )
      {
        this.LogDebug ( "Common Name empty to try AdsID. " );

        userProfile = this._Dal_UserProfiles.GetItemByAdsId ( UserId );

        this.LogDebugClass ( this._Dal_UserProfiles.Log );
      }

      // 
      // Return the user profile.
      // 
      return userProfile;

    }//END getItem method

    // =====================================================================================
    /// <summary>
    /// This class retrieves the Userprofile object based on Guid
    /// </summary>
    /// <param name="UserProfileGuid">Guid: a userProfile Global unique identifier</param>
    /// <returns>Evado.Model.Digital.EdUserProfile: a user profile object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method to retrieve a userProfile object based on Guid
    /// 
    /// 2. Return the UserProfile object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public Evado.Model.Digital.EdUserProfile getItem ( Guid UserProfileGuid )
    {
      this.FlushLog ( );
      this.LogMethod ( "getItem" );
      this.LogDebug ( "UserProfileGuid: " + UserProfileGuid );
      //
      // Initialise the methods variables and objects.
      // 
      Evado.Model.Digital.EdUserProfile userProfile = new Evado.Model.Digital.EdUserProfile ( );

      // 
      // Retrieve the user object from the database.
      // 
      userProfile = this._Dal_UserProfiles.GetItem ( UserProfileGuid );
      this.LogDebugClass ( this._Dal_UserProfiles.Log );

      // 
      // Return the user profile.
      // 
      return userProfile;

    }//END getItem method

    #endregion

    #region Class  update  methods
    // =====================================================================================
    /// <summary>
    /// This class retrieves a user profile with a specific connection String.
    /// </summary>
    /// <param name="ConnectionStringKey">string: the database connection string config key</param>
    /// <param name="UserProfile">Evado.Model.Digital.EdUserProfile object</param>
    /// <returns>EvEventCodes enumeration</returns>
    // -------------------------------------------------------------------------------------
    public EvEventCodes saveItem (
      string ConnectionStringKey,
      Evado.Model.Digital.EdUserProfile UserProfile )
    {
      this.FlushLog ( );
      this.LogMethod ( "saveItem" );
      Evado.Bll.EvStaticSetting.ConnectionStringKey = ConnectionStringKey;

      EvEventCodes userProfile = this.saveItem ( UserProfile );


      Evado.Bll.EvStaticSetting.ResetConnectionString ( );
      return userProfile;
    }

    // =====================================================================================
    /// <summary>
    /// This class updates the user profile in the database. 
    /// The update and add process are the same as in each execution the currentMonth objects are 
    /// set to superseded and then a new object is inserted to the database.
    /// </summary>
    /// <param name="UserProfile">Evado.Model.Digital.EdUserProfile: a userProfile object</param>
    /// <returns>EvEventCodes: an event code for saving items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the UserId or UserCommonName is empty
    /// 
    /// 2. Execute the method for deleting items if the commone name is empty. 
    /// 
    /// 3. Execute the method for adding items if the Uid is not defined 
    /// 
    /// 4. Else, execute the method to update items. 
    /// 
    /// 5. Return an event code of method execution.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes saveItem ( Evado.Model.Digital.EdUserProfile UserProfile )
    {
      this.FlushLog ( );
      this.LogMethod ( "saveItem" );
      this.LogValue ( "UserId: " + UserProfile.UserId );
      this.LogValue ( "CommonName: " + UserProfile.CommonName );
      // 
      // Initialise the methods variables and objects.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Check that the user id is valid
      // 
      if ( UserProfile.UserId == String.Empty )
      {
        this.LogValue ( "UserId is empty" );
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // If CommonName length is null delete the user.
      // 
      if ( UserProfile.CommonName == String.Empty )
      {
        this.LogValue ( "Delete User" );
        iReturn = this._Dal_UserProfiles.DeleteItem ( UserProfile );
        this.LogDebug ( this._Dal_UserProfiles.Log );

        return iReturn;
      }

      //
      // Add a new user if the user profile Guid is empty.
      //
      if ( UserProfile.Guid == Guid.Empty )
      {
        this.LogValue ( "Add User" );
        iReturn = this._Dal_UserProfiles.AddItem ( UserProfile );
        this.LogDebug ( this._Dal_UserProfiles.Log );

        return iReturn;
      }

      this.LogValue ( "update User" );
      iReturn = this._Dal_UserProfiles.UpdateItem ( UserProfile );
      this.LogDebug ( this._Dal_UserProfiles.Log );

      return iReturn;

    }//END saveItem method

    #endregion

    #region Class  update  methods

    // =====================================================================================
    /// <summary>
    /// This method updates user profile for token authentication process.
    /// </summary>
    /// <param name="TokenUser">Evado.Model.EusTokenUserProfile object</param>
    /// <param name="UserTypeList">EdSelectionList object containing the user category options.</param>
    /// <returns>EvEventCodes: an event code for saving items</returns>
    // -------------------------------------------------------------------------------------
    public EvEventCodes updateTokenUser ( 
      Evado.Model.EusTokenUserProfile TokenUser,
      EdSelectionList UserTypeList )
    {
      this.FlushLog ( );
      this.LogMethod ( "updateTokenUser" );
      this.LogDebug ( "TokenUser.UserId {0}", TokenUser.UserId );
      this.LogDebug ( "UserTypeList.Items Title: {0}, items {1}",
        UserTypeList.Title, UserTypeList.Items.Count );
      // 
      // Initialise the methods variables and objects.
      // 
      EvEventCodes result = EvEventCodes.Ok;
      Evado.Model.Digital.EdUserProfile userProfile = new EdUserProfile ( );
      Evado.Model.Digital.EdUserProfile existingUserProfile = new EdUserProfile ( );

      //
      // Import the user token data.
      //
      userProfile.ImportTokenProfile ( TokenUser );

      // 
      // Check that the user id is valid
      // 
      if ( userProfile.UserId == String.Empty )
      {
        this.LogEvent ( "UserId is empty" );
        return EvEventCodes.Token_User_Profile_UserId_Empty;
      }

      //
      // validate that the user does not already exist.
      //
      existingUserProfile = this._Dal_UserProfiles.GetItem ( userProfile.Guid );

      this.LogDebug ( " userProfile.Guid: '{0}', UserId '{1}'", userProfile.Guid, userProfile.UserId );
      this.LogDebug ( " existingUserProfile.Guid: '{0}', UserId '{1}'", existingUserProfile.Guid, existingUserProfile.UserId );

      if ( existingUserProfile.UserId != userProfile.UserId
        && existingUserProfile.Guid != Guid.Empty )
      {
        this.LogDebug ( "UserId does NOT match the Token value." );
        this.LogMethodEnd ( "updateTokenUser" );
        return EvEventCodes.Token_User_Profile_UserId_Error;
      }

      //
      // If a new user exists the update their details as a subscribed user.
      //
      if ( existingUserProfile.UserId == userProfile.UserId
        && TokenUser.UserStatus == EusTokenUserProfile.UserStatusCodes.New_User )
      {
        this.LogDebug ( "Existing User to update details as a subscribed user." );
        TokenUser.UserStatus = EusTokenUserProfile.UserStatusCodes.Subscribed_User;
      }

      //
      // if the user does not have an organisation create one for them.
      //
      if ( userProfile.OrgId == String.Empty )
      {
        this.LogDebug ( "No Organisation so create one for the user." );
        this.AddTokenOrganisation ( userProfile );
      }

      //
      // match the user category with the user type.
      //
      if ( UserTypeList.Items.Count > 0 )
      {
        this.LogDebug ( "Updating the use category" );
        this.LogDebug ( "UserType {0}.", userProfile.UserType );
        foreach ( EdSelectionList.Item item in UserTypeList.Items )
        {
          this.LogDebug ( "Item C: {0}, V: {1}, D: {2}", item.Category, item.Value, item.Description);
          if ( userProfile.UserType == item.Value )
          {
            userProfile.UserCategory = item.Category;
            break;
          }
        }
      }
      this.LogDebug ( "UserCatetory {0}.", userProfile.UserCategory );

      //
      // if the role is empty set to the category as a default.
      //
      if ( userProfile.Roles == String.Empty )
      {
        userProfile.Roles = userProfile.UserCategory;
      }

      //
      // based on the user status select the method of user update.
      //
      this.LogDebug ( "UserStatus {0}.", TokenUser.UserStatus );
      switch ( TokenUser.UserStatus )
      {
        case EusTokenUserProfile.UserStatusCodes.New_User:
          {
            this.LogValue ( "Add User" );
            result = this._Dal_UserProfiles.AddItem ( userProfile );
            this.LogDebug ( this._Dal_UserProfiles.Log );

            this.LogMethodEnd ( "updateTokenUser" );
            return result;
          }

        case EusTokenUserProfile.UserStatusCodes.Subscribed_User:
          {
            this.LogValue ( "update User" );
            result = this._Dal_UserProfiles.UpdateItem ( userProfile );
            this.LogDebug ( this._Dal_UserProfiles.Log );

            this.LogMethodEnd ( "updateTokenUser" );
            return result;
          }

        default:
          {
            this.LogValue ( "Disable user access" );

            userProfile.Roles = String.Empty;
            userProfile.UserCategory = String.Empty;
            userProfile.UserType = String.Empty;

            result = this._Dal_UserProfiles.UpdateItem ( userProfile );
            this.LogDebug ( this._Dal_UserProfiles.Log );

            this.LogMethodEnd ( "updateTokenUser" );
            return result;
          }
      }
    }//END updateTokenUser method
    
    // =====================================================================================
    /// <summary>
    /// This method add an organisation for token generated users.
    /// </summary>
    /// <param name="UserProfile">Evado.Model.Digital.EdUserProfile object</param>
    /// <returns>EvEventCodes: an event code for saving items</returns>
    // -------------------------------------------------------------------------------------
    public EvEventCodes AddTokenOrganisation (
      EdUserProfile UserProfile )
    {
      this.LogMethod ( "AddTokenOrganisation" );
      //
      // Initialise the methods variables and objects.
      //
      EvEventCodes result = EvEventCodes.Ok;
      Evado.Model.Digital.EdOrganisation organisation = new EdOrganisation ( );
      Evado.Dal.Digital.EdOrganisations dal_Organisations =
        new Evado.Dal.Digital.EdOrganisations ( this.ClassParameter );

      //
      // Create the org identifier.
      //
      int orgCount = dal_Organisations.getOrganisationCount ( String.Empty );

      String orgId = "T" + orgCount.ToString ( "0000" );
      organisation.OrgId = orgId;
      UserProfile.OrgId = orgId;

      //
      // Add organisation object
      //
      result = dal_Organisations.addItem ( organisation );

      this.LogDebug ( dal_Organisations.Log );

      this.LogMethodEnd ( "AddTokenOrganisation" );
      return result;
    }
    #endregion


    // ++++++++++++++++++++++++++++++++++  END OF SOURCE CODE +++++++++++++++++++++++++++++++++++++
  }//END EvUserProfiles class

}//END namespace Evado.Bll.Digital

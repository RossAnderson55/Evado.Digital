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
  public class EvUserProfiles : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvUserProfiles ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvUserProfiles.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvUserProfiles ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvUserProfiles.";

      this.LogMethod ( "EvUserProfiles initialisation method." );
      this.LogDebug ( "ApplicationGuid: " + this.ClassParameter.AdapterGuid );

      this.LogMethodEnd ( "EvUserProfiles" );

      this._Dal_UserProfiles = new Evado.Dal.Digital.EvUserProfiles ( Settings );
    }
    #endregion

    #region Class variables and properties
    // 
    // Instantiate the DAL Class\
    // 
    private Evado.Dal.Digital.EvUserProfiles _Dal_UserProfiles = new Evado.Dal.Digital.EvUserProfiles ( );

    #endregion

    #region Class methods
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
     EdUserProfile.UserTypesList Type,
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
     EdUserProfile.UserTypesList Type,
     String OrgId,
      bool useGuid )
    {
      LogMethod ( "GetView method." );
      this.LogDebug ( "OrgId: " + OrgId );

      List<EvOption> list = this._Dal_UserProfiles.GetList ( Type, OrgId, useGuid );
      this.LogDebugClass ( this._Dal_UserProfiles.Log );

      return list;

    }//END GetList method

    // =====================================================================================
    /// <summary>
    /// This method returns true if the user exists in the database.
    /// </summary>
    /// <param name="UserId">String: The user identifier.</param>
    /// <returns>Boolean: True: user exists in the database.</returns>
    // -------------------------------------------------------------------------------------
    public bool ExistingUserId ( string UserId )
    {
      this.LogMethod ( "ExistingUserId method." );
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
    public int UserCount ( EdUserProfile.UserTypesList UserType )
    {
      this.LogMethod ( "UserCount method." );
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
      this.LogMethod ( "getItem method" );
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
      this.LogMethod ( "getItem method." );
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
      this.LogMethod ( "getItem method." );
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
      this.LogMethod ( "saveItem method." );
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
      this.LogMethod ( "saveItem method." );
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

    // ++++++++++++++++++++++++++++++++++  END OF SOURCE CODE +++++++++++++++++++++++++++++++++++++
  }//END EvUserProfiles class

}//END namespace Evado.Bll.Digital

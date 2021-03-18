/* <copyright file="Dal\eclinical\EvUserProfiles.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;

//References to Evado specific libraries

using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Digital
{
  /// <summary>
  /// A business Component used to manage Ethics roles
  /// The Evado.Evado.User is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  public class EdUserProfiles : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdUserProfiles ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EvUserProfiles.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdUserProfiles ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Digital.EvUserProfiles.";

      this.LogMethod ( "EvUserProfiles initialisation method." );
      this.LogDebug ( "ApplicationGuid: " + this.ClassParameters.AdapterGuid );

      this.LogMethodEnd ( "EvUserProfiles" );
    }

    #endregion

    #region Initialise Class variables and objects
    //
    // Static constants
    //
    /// <summary>
    /// This constant selects all rows from EvUserProfile_View view.
    /// </summary>

    private const string SQL_SELECT_QUERY = "Select * FROM ED_USER_PROFILES ";

    /// <summary>
    /// This constant defines a store procedure for adding items to UserProfile table.
    /// </summary>
    private const string STORED_PROCEDURE_AddItem = "USR_USER_PROFILE_ADD";

    /// <summary>
    /// This constant defines a store procedure for updating items to UserProfile table.
    /// </summary>
    private const string STORED_PROCEDURE_UpdateItem = "USR_USER_PROFILE_UPDATE";

    /// <summary>
    /// This constant defines a store procedure for deleting items from UserProfile table.
    /// </summary>
    private const string STORED_PROCEDURE_DeleteItem = "USR_USER_PROFILE_DELETE";

    public const string DB_USER_GUID = "UP_Guid";
    public const string DB_ORG_ID = "ORG_ID";
    public const string DB_USER_ID = "USER_ID";
    public const string DB_AD_NAME = "UP_ACTIVE_DIRECTORY_NAME";
    public const string DB_COMMON_NAME = "UP_COMMON_NAME";
    public const string DB_TITLE = "UP_TITLE";
    public const string DB_PREFIX = "UP_PREFIX";
    public const string DB_GIVEN_NAME = "UP_GIVEN_NAME";
    public const string DB_FAMILY_NAME = "UP_FAMILY_NAME";
    public const string DB_SUFFIX = "UP_SUFFIX";
    public const string DB_ADDRESS_1 = "UP_ADDRESS_1";
    public const string DB_ADDRESS_2 = "UP_ADDRESS_2";
    public const string DB_ADDRESS_CITY = "UP_ADDRESS_CITY";
    public const string DB_ADDRESS_POST_CODE = "UP_ADDRESS_POST_CODE";
    public const string DB_ADDRESS_STATE = "UP_ADDRESS_STATE";
    public const string DB_ADDRESS_COUNTRY = "UP_ADDRESS_COUNTRY";
    public const string DB_MOBILE_PHONE = "UP_MOBILE_PHONE";
    public const string DB_TELEPHONE = "UP_TELEPHONE";
    public const string DB_EMAIL_ADDRESS = "UP_EMAIL_ADDRESS";
    public const string DB_ROLES = "UP_ROLES";
    public const string DB_TYPE = "UP_TYPE";
    public const string DB_IMAGE_FILENAME = "UP_IMAGE_FILENAME";
    public const string DB_UPDATED_BY_USER_ID = "UP_UPDATED_BY_USER_ID";
    public const string DB_UPDATE_BY = "UP_UPDATED_BY";
    public const string DB_UPDATE_DATE = "UP_UPDATED_DATE";
    public const string DB_DELETED = "UP_DELETED";
    public const string DB_EXPIRY_DATE = "UP_EXPIRY_DATE";


    /// <summary>
    /// This constant defines a string parameter as Guid for the UserProfile object
    /// </summary>
    private const string PARM_Guid = "@Guid";
    private const string PARM_ORG_ID = "@OrgId";
    private const string PARM_UserId = "@UserId";
    private const string PARM_ACTIVE_DIRECTORY_NAME = "@ActiveDirectName";
    private const string PARM_PREFIX = "@PREFIX";
    private const string PARM_GIVEN_NAME = "@GIVEN_NAME";
    private const string PARM_FAMILY_NAME = "FAMILY_NAME";
    private const string PARM_SUFFIX = "@SUFFIX";
    private const string PARM_ADDRESS_1 = "@ADDRESS_1";
    private const string PARM_ADDRESS_2 = "@ADDRESS_2";
    private const string PARM_ADDRESS_CITY = "@ADDRESS_CITY";
    private const string PARM_ADDRESS_STATE = "@ADDRESS_STATE";
    private const string PARM_ADDRESS_POSTCODE = "@ADDRESS_POST_CODE";
    private const string PARM_ADDRESS_COUNTRY = "@ADDRESS_COUNTRY";
    private const string PARM_TELEPHONE = "@TELEPHONE";
    private const string PARM_MOBILE_PHONE = "@MOBILE_PHONE";
    private const string PARM_CommonName = "@CommonName";
    private const string PARM_EmailAddress = "@EmailAddress";
    private const string PARM_RoleId = "@RoleId";
    private const string PARM_TYPE = "@TYPE";
    private const string PARM_Title = "@Title";
    private const string PARM_IMAGE_FILENAME = "@IMAGE_FILENAME";
    private const string PARM_UpdatedByUserId = "@UpdatedByUserId";
    private const string PARM_UpdatedBy = "@UpdatedBy";
    private const string PARM_UpdateDate = "@UpdateDate";
    private const string PARM_EXPIRY_DATE = "@EXPIRY_DATE";
    private const string PARM_PARTIAL_USER_ID = "@P_USER_ID";

    private const string PARM_PARTIAL_COMMON_NAME = "@P_COMMON_NAME";

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region SQL Parameter methods

    // =====================================================================================
    /// <summary>
    /// This method returns an array of sql query parameters. 
    /// </summary>
    /// <returns>SqlParameter: An array of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create an array of sql query parameters. 
    /// 
    /// 2. Return an array of sql query parameters.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private static SqlParameter [ ] getItemsParameters ( )
    {
      SqlParameter [ ] parms = new SqlParameter [ ]
      {
        new SqlParameter( EdUserProfiles.PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter( EdUserProfiles.PARM_ORG_ID, SqlDbType.NVarChar, 20),
        new SqlParameter( EdUserProfiles.PARM_UserId, SqlDbType.NVarChar, 100),
        new SqlParameter( EdUserProfiles.PARM_ACTIVE_DIRECTORY_NAME, SqlDbType.NVarChar, 100),
        new SqlParameter( EdUserProfiles.PARM_PREFIX, SqlDbType.NVarChar, 10),
        new SqlParameter( EdUserProfiles.PARM_GIVEN_NAME, SqlDbType.NVarChar, 50),
        new SqlParameter( EdUserProfiles.PARM_FAMILY_NAME, SqlDbType.NVarChar, 50),
        new SqlParameter( EdUserProfiles.PARM_SUFFIX, SqlDbType.NVarChar, 50),
        new SqlParameter( EdUserProfiles.PARM_ADDRESS_1, SqlDbType.NVarChar, 50),
        new SqlParameter( EdUserProfiles.PARM_ADDRESS_2, SqlDbType.NVarChar, 50),
        new SqlParameter( EdUserProfiles.PARM_ADDRESS_CITY, SqlDbType.NVarChar, 50),
        new SqlParameter( EdUserProfiles.PARM_ADDRESS_STATE, SqlDbType.NVarChar, 50),
        new SqlParameter( EdUserProfiles.PARM_ADDRESS_POSTCODE, SqlDbType.NVarChar, 50),
        new SqlParameter( EdUserProfiles.PARM_ADDRESS_COUNTRY, SqlDbType.NVarChar, 50),
        new SqlParameter( EdUserProfiles.PARM_TELEPHONE, SqlDbType.NVarChar, 50),
        new SqlParameter( EdUserProfiles.PARM_MOBILE_PHONE, SqlDbType.NVarChar, 50),
        new SqlParameter( EdUserProfiles.PARM_CommonName, SqlDbType.NVarChar, 100),
        new SqlParameter( EdUserProfiles.PARM_EmailAddress, SqlDbType.NVarChar, 100),
        new SqlParameter( EdUserProfiles.PARM_RoleId, SqlDbType.NVarChar, 100),
        new SqlParameter( EdUserProfiles.PARM_TYPE, SqlDbType.NVarChar, 100),
        new SqlParameter( EdUserProfiles.PARM_Title, SqlDbType.NVarChar, 100),
        new SqlParameter( EdUserProfiles.PARM_EXPIRY_DATE, SqlDbType.DateTime),
        new SqlParameter( EdUserProfiles.PARM_IMAGE_FILENAME, SqlDbType.NVarChar, 100),
        new SqlParameter( EdUserProfiles.PARM_UpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( EdUserProfiles.PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( EdUserProfiles.PARM_UpdateDate, SqlDbType.DateTime),
      };

      return parms;
    }//END getItemsParameters class

    // =====================================================================================
    /// <summary>
    /// This method assigns UserProfile object's values to the array of sql query parameters.
    /// </summary>
    /// <param name="parms">SqlParameter: An Array of sql parameters</param>
    /// <param name="UserProfile">Evado.Model.Digital.EvUserProfile: A user profile object</param>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Bind the User Profile object's values to the array of sql query parameters.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void setUsersParameters (
      SqlParameter [ ] parms,
      Evado.Model.Digital.EdUserProfile UserProfile )
    {
      parms [ 0 ].Value = UserProfile.Guid;
      parms [ 1 ].Value = UserProfile.OrgId;
      parms [ 2 ].Value = UserProfile.UserId;
      parms [ 3 ].Value = UserProfile.ActiveDirectoryUserId;
      parms [ 4 ].Value = UserProfile.Prefix;
      parms [ 5 ].Value = UserProfile.GivenName;
      parms [ 6 ].Value = UserProfile.FamilyName;
      parms [ 7 ].Value = UserProfile.Suffix;
      parms [ 8 ].Value = UserProfile.Address_1;
      parms [ 9 ].Value = UserProfile.Address_2;
      parms [ 10 ].Value = UserProfile.AddressCity;
      parms [ 11 ].Value = UserProfile.AddressState;
      parms [ 12 ].Value = UserProfile.AddressPostCode;
      parms [ 13 ].Value = UserProfile.AddressCountry;
      parms [ 14 ].Value = UserProfile.Telephone;
      parms [ 15 ].Value = UserProfile.MobilePhone;
      parms [ 16 ].Value = UserProfile.CommonName;
      parms [ 17 ].Value = UserProfile.EmailAddress.ToLower ( );
      parms [ 18 ].Value = UserProfile.Roles;
      parms [ 19 ].Value = UserProfile.TypeId;
      parms [ 20 ].Value = UserProfile.Title;
      parms [ 21 ].Value = UserProfile.ExpiryDate;
      parms [ 22 ].Value = UserProfile.ImageFileName;
      parms [ 23 ].Value = this.ClassParameters.UserProfile.UserId;
      parms [ 24 ].Value = this.ClassParameters.UserProfile.CommonName;
      parms [ 25 ].Value = DateTime.Now;


    }//END setUsersParameters class.

    #endregion

    #region Data Reader methods

    // =====================================================================================
    /// <summary>
    /// This method extracts data row values to the UserProfile object.
    /// </summary>
    /// <param name="Row">DataRow: A data row object</param>
    /// <returns>Evado.Model.Digital.EvUserProfile: A userprofile object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Extract the compatible data row values to the userprofile object. 
    /// 
    /// 2. Return a userprofile object.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public Evado.Model.Digital.EdUserProfile readRow (
      DataRow Row )
    {
      // 
      // Initialise the local variable.
      // 
      Evado.Model.Digital.EdUserProfile profile = new Evado.Model.Digital.EdUserProfile ( );

      // 
      // Update the object properties.
      // 
      profile.Guid = EvSqlMethods.getGuid ( Row, DB_USER_GUID );
      profile.OrgId = EvSqlMethods.getString ( Row, DB_ORG_ID );

      profile.UserId = EvSqlMethods.getString ( Row, DB_USER_ID );
      profile.ActiveDirectoryUserId = EvSqlMethods.getString ( Row, DB_AD_NAME );
      profile.CommonName = EvSqlMethods.getString ( Row, DB_COMMON_NAME );
      profile.Roles = EvSqlMethods.getString ( Row, DB_ROLES );
      profile.TypeId = EvSqlMethods.getString ( Row, EdUserProfiles.DB_TYPE );

      profile.Prefix = EvSqlMethods.getString ( Row, EdUserProfiles.DB_PREFIX );
      profile.GivenName = EvSqlMethods.getString ( Row, EdUserProfiles.DB_GIVEN_NAME );
      profile.FamilyName = EvSqlMethods.getString ( Row, EdUserProfiles.DB_FAMILY_NAME );
      profile.Suffix = EvSqlMethods.getString ( Row, EdUserProfiles.DB_SUFFIX );
      profile.Address_1 = EvSqlMethods.getString ( Row, EdUserProfiles.DB_ADDRESS_1 );
      profile.Address_2 = EvSqlMethods.getString ( Row, EdUserProfiles.DB_ADDRESS_2 );
      profile.AddressCity = EvSqlMethods.getString ( Row, EdUserProfiles.DB_ADDRESS_CITY );
      profile.AddressState = EvSqlMethods.getString ( Row, EdUserProfiles.DB_ADDRESS_STATE );
      profile.AddressPostCode = EvSqlMethods.getString ( Row, EdUserProfiles.DB_ADDRESS_POST_CODE );
      profile.AddressCountry = EvSqlMethods.getString ( Row, EdUserProfiles.DB_ADDRESS_COUNTRY );
      profile.Telephone = EvSqlMethods.getString ( Row, EdUserProfiles.DB_TELEPHONE );
      profile.MobilePhone = EvSqlMethods.getString ( Row, EdUserProfiles.DB_MOBILE_PHONE );
      profile.EmailAddress = EvSqlMethods.getString ( Row, EdUserProfiles.DB_EMAIL_ADDRESS ).ToLower ( );
      profile.Title = EvSqlMethods.getString ( Row, EdUserProfiles.DB_TITLE );
      profile.ExpiryDate = EvSqlMethods.getDateTime ( Row, EdUserProfiles.DB_EXPIRY_DATE );
      profile.ImageFileName = EvSqlMethods.getString ( Row, EdUserProfiles.DB_IMAGE_FILENAME );

      profile.UpdatedByUserId = EvSqlMethods.getString ( Row, EdUserProfiles.DB_UPDATED_BY_USER_ID );
      profile.UpdatedBy = EvSqlMethods.getString ( Row, EdUserProfiles.DB_UPDATE_BY );
      profile.UpdatedDate = EvSqlMethods.getDateTime ( Row, EdUserProfiles.DB_UPDATE_DATE );

      // 
      // Return the profile Object.
      // 
      return profile;

    }//END readRow method.

    #endregion

    #region UserProfile Query methods

    // =====================================================================================
    /// <summary>
    /// This method returns a list of UserProfile object.
    /// </summary>
    /// <param name="UserType">EdUserProfile.UserTypesList enumeration</param>
    /// <param name="OrgId">String: The selection user type</param>
    /// <returns>List of Evado.Model.Digital.EvUserProfile: A list of UserProfile objects.</returns>
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
     String UserType,
      String OrgId )
    {

      this.LogMethod ( "GetView method." );
      this.LogDebug ( "Type: " + UserType );
      this.LogDebug ( "OrgId: " + OrgId );

      // 
      // Define the local variables
      // 
      String sqlQueryString;
      List<Evado.Model.Digital.EdUserProfile> view = new List<Evado.Model.Digital.EdUserProfile> ( );

      if ( UserType =="Evado" )
      {
        OrgId = "EVADO";
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter ( EdUserProfiles.PARM_TYPE, SqlDbType.Char, 30 ),
        new SqlParameter ( EdUserProfiles.PARM_ORG_ID, SqlDbType.Char, 10 )
      };
      cmdParms [ 0 ].Value = UserType;
      cmdParms [ 1 ].Value = OrgId;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = SQL_SELECT_QUERY + " WHERE (" + EdUserProfiles.DB_DELETED + " = 0) \r\n"; ;

      if ( UserType != String.Empty )
      {
        sqlQueryString += " AND (" + EdUserProfiles.DB_TYPE + " = " + EdUserProfiles.PARM_TYPE + ") \r\n";
      }

      if ( OrgId != String.Empty )
      {
        sqlQueryString += " AND (" + EdUserProfiles.DB_ORG_ID + " = " + EdUserProfiles.PARM_ORG_ID + ") \r\n";
      }

      sqlQueryString += " ORDER BY " + EdUserProfiles.DB_ORG_ID + ", " + EdUserProfiles.DB_USER_ID + ";";

      this.LogDebug ( sqlQueryString );

      this.LogDebug ( "EvSqlMethods Log: " + EvSqlMethods.Log );
      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row.
          // 
          DataRow row = table.Rows [ count ];

          Evado.Model.Digital.EdUserProfile profile = this.readRow ( row );

          view.Add ( profile );
        }
      }
      this.LogValue ( " View count: " + view.Count );

      // 
      // Return the ArrayList containing the User data object.
      // 
      return view;

    }//END GetView method.

    // =====================================================================================
    /// <summary>
    /// This method returns a list of UserProfile object
    /// </summary>
    /// <param name="OrgId">String: The selection organistion's identifier</param>
    /// <param name="PartialUserId">String: partial string of the user's user identifier</param>
    /// <param name="PartialCommonName">String: partial string of the user's common nbame identifier</param>
    /// <returns>List of Evado.Model.Digital.EvUserProfile: A list of UserProfile objects.</returns>
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
      this.LogDebug ( "PartialUserId: " + PartialUserId );
      this.LogDebug ( "PartialCommonName: " + PartialCommonName );

      // 
      // Define the local variables
      // 
      String sqlQueryString;
      List<Evado.Model.Digital.EdUserProfile> view = new List<Evado.Model.Digital.EdUserProfile> ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter ( EdUserProfiles.PARM_ORG_ID, SqlDbType.Char, 20 ),
      };
      cmdParms [ 0 ].Value = OrgId;

      //
      // if the user is not an Evado user then set the Evado identifier (ApplicationGuid) to empty
      // to ensure that Evado organisations are not displayed in the org list.
      //
      if ( this.ClassParameters.UserProfile.hasEvadoAccess == false
        && OrgId.ToLower ( ) == "evado" )
      {
        cmdParms [ 1 ].Value = Guid.Empty;
      }

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = SQL_SELECT_QUERY + " WHERE  ( UP_SUPERSEDED = 0) ";

      if ( OrgId != String.Empty )
      {
        PartialUserId = PartialUserId.Replace ( "'", "" );

        sqlQueryString += " AND (" + EdUserProfiles.DB_ORG_ID + " = " + EdUserProfiles.PARM_ORG_ID + " ) \r\n";
      }

      if ( PartialUserId != String.Empty )
      {
        PartialUserId = PartialUserId.Replace ( "'", "" );

        sqlQueryString += " AND (" + EdUserProfiles.DB_USER_ID + " LIKE '%" + PartialUserId + "%' ) \r\n";
      }

      if ( PartialCommonName != String.Empty )
      {
        PartialCommonName = PartialUserId.Replace ( "'", "" );

        sqlQueryString += " AND (" + EdUserProfiles.DB_COMMON_NAME + " LIKE '%" + PartialCommonName + "%' ) \r\n";
      }

      sqlQueryString += " ORDER BY OrgId, UserId;";

      this.LogDebug ( sqlQueryString );

      this.LogDebug ( "EvSqlMethods Log: " + EvSqlMethods.Log );
      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row.
          // 
          DataRow row = table.Rows [ count ];

          Evado.Model.Digital.EdUserProfile profile = this.readRow ( row );

          view.Add ( profile );
        }
      }
      this.LogValue ( " View count: " + view.Count );

      // 
      // Return the ArrayList containing the User data object.
      // 
      return view;

    }//END GetView method.

    // =====================================================================================
    /// <summary>
    /// This method returns a list of UserProfile object based on OrgId and useGuid condition
    /// </summary>
    /// <param name="OrgId">string: an organization identifier</param>
    /// <param name="useGuid">Boolean: true, if Guid is used</param>
    /// <returns>List of Evado.Model.Digital.EvUserProfile: A list of UserProfile objects.</returns>
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
    public List<Evado.Model.EvOption> GetList (
     String Type,
      String OrgId,
      bool useGuid )
    {

      this.LogMethod ( "GetList method. " );
      this.LogValue ( "OrgId: " + OrgId );
      // 
      // Define local variables
      // 
      List<Evado.Model.EvOption> list = new List<Evado.Model.EvOption> ( );
      Evado.Model.EvOption option = new Evado.Model.EvOption ( );
      if ( useGuid == true )
      {
        option = new Evado.Model.EvOption ( Guid.Empty.ToString ( ), String.Empty );
      }
      list.Add ( option );

      //
      // get the list of users for this customer.
      //
      var userProfileList = this.GetView ( Type, OrgId );

      //
      // iterate through the list to create the option list of users.
      //
      foreach ( EdUserProfile user in userProfileList )
      {

        if ( useGuid == true )
        {
          option = new Evado.Model.EvOption ( user.Guid.ToString ( ),
           String.Format ( "{0} - {1}", user.UserId, user.CommonName ) );
        }
        else
        {
          option = new Evado.Model.EvOption ( user.UserId,
           String.Format ( "{0} - {1}", user.UserId, user.CommonName ) );
        }

        list.Add ( option );
      }

      this.LogValue ( "View count: " + list.Count );

      // 
      // Return the ArrayList of EvOptions data objects.
      // 
      return list;

    }//END GetList method

    // =====================================================================================
    /// <summary>
    /// This method returns true if the user exists in the database.
    /// </summary>
    /// <param name="UserId">String: The user identifier.</param>
    /// <returns>Boolean: True: user exists in the database.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on datatable. 
    /// 
    /// 3. Return true of more than on record is returned.
    /// 
    /// 4. Return false if no records are returned.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public bool ExistingUserId ( String UserId )
    {

      this.LogMethod ( "ExistngUserId method." );
      this.LogDebug ( "UserId: " + UserId );

      // 
      // Define the local variables
      // 
      string sqlQueryString;

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
         new SqlParameter ( PARM_UserId, SqlDbType.Char, 50 )
      };
      cmdParms [ 0 ].Value = UserId;


      // 
      // Generate the SQL query string
      // 
      sqlQueryString = SQL_SELECT_QUERY
        + "WHERE (" + EdUserProfiles.DB_USER_ID + " = " + EdUserProfiles.PARM_UserId + ")\r\n";

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        if ( table.Rows.Count > 0 )
        {
          return true;
        }
      }

      // 
      // Return the ArrayList containing the User data object.
      // 
      return false;

    }//END GetView method.


    // =====================================================================================
    /// <summary>
    /// This method returns true if the user exists in the database.
    /// </summary>
    /// <param name="OrgId">String: The user identifier.</param>
    /// <returns>Boolean: True: user exists in the database.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on datatable. 
    /// 
    /// 3. Return true of more than on record is returned.
    /// 
    /// 4. Return false if no records are returned.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public int UserCount ( String UserType )
    {

      this.LogMethod ( "UserCount method." );
      this.LogDebug ( "UserType: " + UserType );

      // 
      // Define the local variables
      // 
      string sqlQueryString;
      int count = 0;

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
         new SqlParameter ( PARM_ORG_ID, SqlDbType.Char, 10 )
      };
      cmdParms [ 0 ].Value = UserType;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = SQL_SELECT_QUERY;

      if ( UserType != String.Empty )
      {
        sqlQueryString = SQL_SELECT_QUERY
          + "WHERE (" + EdUserProfiles.DB_TYPE + " = " + EdUserProfiles.PARM_TYPE + ") ;";
      }

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        count = table.Rows.Count;
      }

      // 
      // Return the ArrayList containing the User data object.
      // 
      this.LogMethodEnd ( "UserCount" );
      return count;

    }//END GetView method.
    #endregion

    #region Retrieve Query methods

    // =====================================================================================
    /// <summary>
    /// This method retrieves the UserProfile table based on UserId
    /// </summary>
    /// <param name="UserId">string: A User identifier</param>
    /// <returns>Evado.Model.Digital.EvUserProfile: A UserProfile object</returns>
    /// <remarks>
    /// This method consists of follwoing steps. 
    /// 
    /// 1. Return an empty userprofile object, if the UserId is empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the result on table. 
    /// 
    /// 4. Return an empty userprofile object, if the table has no value. 
    /// 
    /// 5. Else, add role values to Userprofile object. 
    /// 
    /// 6. Return the UserProfile object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public Evado.Model.Digital.EdUserProfile GetItem ( string UserId )
    {
      this.LogMethod ( "GetItem method " );
      this.LogDebug ( "UserId: " + UserId );
      // 
      // Define local variables
      // 
      string sqlQueryString;
      Evado.Model.Digital.EdUserProfile userProfile = new Evado.Model.Digital.EdUserProfile ( );

      // 
      // If a UserId is equal to an empty string, add a string "User Id null" to a StringBuilder 
      // object _DebugLog and return userProfile object.  
      // 
      if ( UserId == String.Empty )
      {
        this.LogValue ( "User Id null" );
        return userProfile;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter ( PARM_UserId, SqlDbType.Char, 100 )
      };
      cmdParms [ 0 ].Value = UserId;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = SQL_SELECT_QUERY
        + "WHERE (" + EdUserProfiles.DB_USER_ID + " = " + EdUserProfiles.PARM_UserId + ")\r\n";

      this.LogDebug ( sqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // If no rows return, add a string "Query result empty" to a StringBuilder
        // object and return a userPrfile object. 
        // 
        if ( table.Rows.Count == 0 )
        {
          this.LogEvent ( "EvUserProfiles query return empty result." );
          this.LogClass ( EvSqlMethods.Log );
          return userProfile;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        userProfile = this.readRow ( row );

        this.LogDebug ( "UserProfile.UserId: " + userProfile.UserId );
        this.LogDebug ( "UserProfile.CommonName: " + userProfile.CommonName );

      }//END Using

      //
      // load the user parmeter list.
      //
      userProfile.Parameters = this.LoadObjectParameters ( userProfile.Guid );

      // 
      // Return the userProfile data object.
      // 
      this.LogMethodEnd ( "GetItem" );
      return userProfile;

    }//END GetItem method

    // =====================================================================================
    /// <summary>
    /// This method retrieves the userprofile data table based on the AdsUserId
    /// </summary>
    /// <param name="AdsUserId">string: An adverse user identifier</param>
    /// <returns>Evado.Model.Digital.EvUserProfile: a user profile object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Return an empty userprofile object, if the AdsUserId is empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the result on table. 
    /// 
    /// 4. Return an empty userprofile object, if the table has no value. 
    /// 
    /// 5. Else, add role values to Userprofile object. 
    /// 
    /// 6. Return the UserProfile object. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public Evado.Model.Digital.EdUserProfile GetItemByAdsId ( string AdsUserId )
    {
      this.LogMethod ( "getItemByAdsId method " );
      this.LogValue ( "AdsUserId: " + AdsUserId );
      // 
      // Define local variables
      // 
      string sqlQueryString;
      Evado.Model.Digital.EdUserProfile userProfile = new Evado.Model.Digital.EdUserProfile ( );

      // 
      // If AdsUserId is equal to an empty string, return userProfile object. 
      // 
      if ( AdsUserId == String.Empty )
      {
        return userProfile;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter ( EdUserProfiles.PARM_ACTIVE_DIRECTORY_NAME, SqlDbType.Char, 100 )
      };
      cmdParms [ 0 ].Value = AdsUserId;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = SQL_SELECT_QUERY
        + "WHERE (" + EdUserProfiles.DB_AD_NAME + " = " + EdUserProfiles.PARM_ACTIVE_DIRECTORY_NAME + ")\r\n";

      this.LogValue ( sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // If no rows found, return a userProfile object
        // 
        if ( table.Rows.Count == 0 )
        {
          return userProfile;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        userProfile = this.readRow ( row );

      }//END Using 

      //
      // load the user parmeter list.
      //
      userProfile.Parameters = this.LoadObjectParameters ( userProfile.Guid );

      // 
      // Return the userProfile data object.
      // 
      this.LogMethodEnd ( "GetItem" );
      return userProfile;

    }// Close GetItemByAdsId method

    // =====================================================================================
    /// <summary>
    /// This method gets the information for a user.
    /// </summary>
    /// <param name="UserProfileGuid">GUID: A UserProfileGuid</param>
    /// <returns>EvUserProfile Data object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define local variables.
    /// 
    /// 2. If a UserProfileid is equal to an empty GUID, return userProfile object. 
    /// 
    /// 3. Define the SQL query parameters and load the query values.
    /// 
    /// 4. Generate the SQL query string.
    /// 
    /// 5. Execute the query against the database.
    /// 
    /// 6. If no rows found, return userProfile object.
    /// 
    /// 7. Extract the table row.
    /// 
    /// 8. Fill the role object.
    /// 
    /// 9. Get the users TrialUser Profiles.
    /// 
    /// 10.Return the EvUserProfile data object.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public Evado.Model.Digital.EdUserProfile GetItem ( Guid UserProfileGuid )
    {
      this.LogMethod ( "getItem method " );
      this.LogValue ( "UserProfileGuid: " + UserProfileGuid );
      // 
      // Define local variables
      // 
      string sqlQueryString;
      Evado.Model.Digital.EdUserProfile userProfile = new Evado.Model.Digital.EdUserProfile ( );

      // 
      // If a UserProfileid is equal to an empty GUID, return userProfile object. 
      // 
      if ( UserProfileGuid == Guid.Empty )
      {
        return userProfile;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter ( EdUserProfiles.PARM_Guid, SqlDbType.UniqueIdentifier )
      };
      cmdParms [ 0 ].Value = UserProfileGuid;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = SQL_SELECT_QUERY
        + "WHERE (" + EdUserProfiles.DB_USER_GUID + " = " + EdUserProfiles.PARM_Guid + ")";

      this.LogValue ( sqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // If no rows found, return userProfile object. 
        // 
        if ( table.Rows.Count == 0 )
        {
          return userProfile;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        userProfile = this.readRow ( row );

      }//END Using 

      //
      // add the user parmeter list.
      //
      userProfile.Parameters = this.LoadObjectParameters ( userProfile.Guid );

      // 
      // Return the EvUserProfile data object.
      // 
      this.LogMethodEnd ( "GetItem" );
      return userProfile;

    }//END GetItem method


    #endregion

    #region Update Data methods

    // =====================================================================================
    /// <summary>
    /// This method retrieves the UserProfile table based on UserId
    /// </summary>
    /// <param name="UserId">string: A User identifier</param>
    /// <returns>Evado.Model.Digital.EvUserProfile: A UserProfile object</returns>
    /// <remarks>
    /// This method consists of follwoing steps. 
    /// 
    /// 1. Return an empty userprofile object, if the UserId is empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the result on table. 
    /// 
    /// 4. Return an empty userprofile object, if the table has no value. 
    /// 
    /// 5. Else, add role values to Userprofile object. 
    /// 
    /// 6. Return the UserProfile object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private Evado.Model.Digital.EdUserProfile getUser ( string UserId )
    {
      this.LogMethod ( "getUser method " );
      this.LogValue ( "UserId: " + UserId );
      // 
      // Define local variables
      // 
      string sqlQueryString;
      Evado.Model.Digital.EdUserProfile userProfile = new Evado.Model.Digital.EdUserProfile ( );

      // 
      // If a UserId is equal to an empty string, add a string "User Id null" to a StringBuilder 
      // object _DebugLog and return userProfile object.  
      // 
      if ( UserId == String.Empty )
      {
        this.LogValue ( "User Id null" );
        return userProfile;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_UserId, SqlDbType.Char, 100 );
      cmdParms.Value = UserId;
      // 
      // Generate the SQL query string
      // 
      sqlQueryString = SQL_SELECT_QUERY + " WHERE (" + DB_USER_ID + " = " + PARM_UserId + " );";

      this.LogDebug ( sqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // If no rows return, add a string "Query result empty" to a StringBuilder
        // object and return a userPrfile object. 
        // 
        if ( table.Rows.Count == 0 )
        {
          this.LogValue ( " Query result empty." );
          return userProfile;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        userProfile = this.readRow ( row );

        this.LogDebug ( "UserProfile.UserId: " + userProfile.UserId );
        this.LogDebug ( "UserProfile.CommonName: " + userProfile.CommonName );

      }//END Using 

      this.LogMethodEnd ( "getUser" );
      // 
      // Return the userProfile data object.
      // 
      return userProfile;

    }//END GetItem method

    // =====================================================================================
    /// <summary>
    /// This method updates items to the UserProfile table. 
    /// </summary>
    /// <param name="UserProfile">Evado.Model.Digital.EvUserProfile: A UserProfile object</param>
    /// <returns>Evado.Model.EvEventCodes: an event code for updating items</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Exit, if the Old user profile's Uid is not defined. 
    /// 
    /// 2. Add items to datachange object if they do not exist on the old userProfile object. 
    /// 
    /// 3. Define the sql query parameters and execute the storeprocedure for updating items. 
    /// 
    /// 4. Exit, if the storeprocedure runs fail. 
    /// 
    /// 5. Add datachange object's values to the backup datachanges object. 
    /// 
    /// 6. Return an event code for updating items. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public Evado.Model.EvEventCodes UpdateItem (
      Evado.Model.Digital.EdUserProfile UserProfile )
    {
      this.FlushLog ( );
      this.LogMethod ( "UpdateItem method. " );
      this.LogValue ( "UserId: " + UserProfile.UserId );
      this.LogDebug ( "ProjectDashboardComponents: " + UserProfile.DefaultDisplayParameters );

      // 
      // Define the local variables.
      // 
      Evado.Model.Digital.EdUserProfile oldUser = new Evado.Model.Digital.EdUserProfile ( );

      // 
      // Get the old user id for to verify that the user exists and instrument differential
      // comparision.
      // 
      oldUser = this.getUser ( UserProfile.UserId );
      if ( oldUser.Guid == Guid.Empty )
      {
        return Evado.Model.EvEventCodes.Data_InvalidId_Error;
      }

      // 
      // Compare the objects.
      // 
      EvDataChanges dataChanges = new EvDataChanges ( );
      Evado.Model.EvDataChange dataChange = new Evado.Model.EvDataChange ( );
      dataChange.TableName = Evado.Model.EvDataChange.DataChangeTableNames.EvUserProfiles;
      dataChange.RecordGuid = UserProfile.Guid;
      dataChange.UserId = UserProfile.UpdatedByUserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add items to the datachange object, if they do not exist on Old User Profile object. 
      //
      if ( UserProfile.TypeId != oldUser.TypeId )
      {
        dataChange.AddItem ( "UserTypeId", oldUser.TypeId, UserProfile.TypeId );
      }
      if ( UserProfile.UserId != oldUser.UserId )
      {
        dataChange.AddItem ( "UserId", oldUser.UserId, UserProfile.UserId );
      }
      if ( UserProfile.ActiveDirectoryUserId != oldUser.ActiveDirectoryUserId )
      {
        dataChange.AddItem ( "ActiveDirectoryUserId", oldUser.ActiveDirectoryUserId, UserProfile.ActiveDirectoryUserId );
      }
      if ( UserProfile.Prefix != oldUser.Prefix )
      {
        dataChange.AddItem ( "Prefix", oldUser.Prefix, UserProfile.Prefix );
      }
      if ( UserProfile.GivenName != oldUser.GivenName )
      {
        dataChange.AddItem ( "GivenName", oldUser.CommonName, UserProfile.GivenName );
      }
      if ( UserProfile.FamilyName != oldUser.FamilyName )
      {
        dataChange.AddItem ( "FamilyName", oldUser.CommonName, UserProfile.FamilyName );
      }
      if ( UserProfile.Suffix != oldUser.Suffix )
      {
        dataChange.AddItem ( "Suffix", oldUser.Suffix, UserProfile.Suffix );
      }
      if ( UserProfile.CommonName != oldUser.CommonName )
      {
        dataChange.AddItem ( "CommonName", oldUser.CommonName, UserProfile.CommonName );
      }
      if ( UserProfile.EmailAddress != oldUser.EmailAddress )
      {
        dataChange.AddItem ( "EmailAddress", oldUser.EmailAddress, UserProfile.EmailAddress );
      }
      if ( UserProfile.Roles != oldUser.Roles )
      {
        dataChange.AddItem ( "RoleId", oldUser.Roles.ToString ( ), UserProfile.Roles.ToString ( ) );
      }
      if ( UserProfile.Title != oldUser.Title )
      {
        dataChange.AddItem ( "Title", oldUser.Title, UserProfile.Title );
      }
      if ( UserProfile.Telephone != oldUser.Telephone )
      {
        dataChange.AddItem ( "Telephone", oldUser.Telephone, UserProfile.Telephone );
      }
      if ( UserProfile.Address_1 != oldUser.Address_1 )
      {
        dataChange.AddItem ( "Address_1", oldUser.Address_1, UserProfile.Address_1 );
      }
      if ( UserProfile.Address_2 != oldUser.Address_2 )
      {
        dataChange.AddItem ( "Address_2", oldUser.Address_2, UserProfile.Address_2 );
      }
      if ( UserProfile.AddressCity != oldUser.AddressCity )
      {
        dataChange.AddItem ( "AddressCity", oldUser.AddressCity, UserProfile.AddressCity );
      }
      if ( UserProfile.AddressPostCode != oldUser.AddressPostCode )
      {
        dataChange.AddItem ( "AddressPostCode", oldUser.AddressPostCode, UserProfile.AddressPostCode );
      }
      if ( UserProfile.AddressState != oldUser.AddressState )
      {
        dataChange.AddItem ( "AddressState", oldUser.AddressState, UserProfile.AddressState );
      }
      if ( UserProfile.AddressCountry != oldUser.AddressCountry )
      {
        dataChange.AddItem ( "AddressCountry", oldUser.AddressCountry, UserProfile.AddressCountry );
      }

      if ( UserProfile.Guid == Guid.Empty )
      {
        UserProfile.Guid = Guid.NewGuid ( );
      }


      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = getItemsParameters ( );
      setUsersParameters ( cmdParms, UserProfile );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_UpdateItem, cmdParms ) == 0 )
      {
        return Evado.Model.EvEventCodes.Database_Record_Update_Error;
      }

      this.UpdateObjectParameters ( UserProfile.Parameters, UserProfile.Guid );
      // 
      // Post the data changes
      // 
      dataChanges.AddItem ( dataChange );

      this.LogMethodEnd ( "UpdateItem" );
      return Evado.Model.EvEventCodes.Ok;

    }//END UpdateItem method

    // =====================================================================================
    /// <summary>
    /// This method adds items to the UserProfile data table. 
    /// </summary>
    /// <param name="UserProfile">Evado.Model.Digital.EvUserProfile: A User Profile object</param>
    /// <returns>Evado.Model.EvEventCodes object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Exit, if the Guid exists. 
    /// 
    /// 2. Create new DB row Guid, if it is empty. 
    /// 
    /// 3. Define the sql parameters and execute the storeprocedure for adding items. 
    /// 
    /// 4. Exit, if the storeprocedure runs fail. 
    /// 
    /// 5. Else, return an event code for adding items.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public Evado.Model.EvEventCodes AddItem (
      Evado.Model.Digital.EdUserProfile UserProfile )
    {
      this.FlushLog ( );
      this.LogMethod ( "addItem method. " );
      this.LogValue ( "UserId: " + UserProfile.UserId );
      // 
      // Define the local variables.
      // 
      Evado.Model.Digital.EdUserProfile user = this.getUser ( UserProfile.UserId );

      //
      // If UserId is not equal to an empty GUID, return a Data_Duplicate_Id_Error. 
      //
      if ( user.Guid != Guid.Empty )
      {
        this.LogDebug ( "UserGuid: " + user.Guid );
        this.LogDebug ( "UserId: " + user.UserId );
        return Evado.Model.EvEventCodes.Data_Duplicate_Id_Error;
      }

      // 
      // Define the guid for the user of one is not allocated.
      // 
      if ( UserProfile.Guid == Guid.Empty )
      {
        UserProfile.Guid = Guid.NewGuid ( );
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = getItemsParameters ( );
      setUsersParameters ( cmdParms, UserProfile );

      /*
      */
      if ( this.ClassParameters.LoggingLevel > 4 )
      {
        // 
        // Extract the parameters
        //
        if ( cmdParms != null )
        {
          this.LogDebug ( "Parameters:" );
          foreach ( SqlParameter prm in cmdParms )
          {
            this.LogDebug ( "Name: " + prm.ParameterName
              + ", Value: " + prm.Value
              + ", Type: " + prm.DbType );
          }
        }
      }

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_AddItem, cmdParms ) == 0 )
      {
        return Evado.Model.EvEventCodes.Database_Record_Update_Error;
      }

      this.LogMethodEnd ( "UpdateItem" );
      return Evado.Model.EvEventCodes.Ok;

    } //END AddItem Method 

    // =====================================================================================
    /// <summary>
    /// This class deletes items from UserProfile table. 
    /// </summary>
    /// <param name="Profile">Evado.Model.Digital.EvUserProfile: A User Profile object</param>
    /// <returns>Evado.Model.EvEventCodes: an event code for deleting items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Guid is empty. 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for deleting items.
    /// 
    /// 3. Exit, if the storeprocedure runs fail. 
    /// 
    /// 4. Else, return an event code for deleting items.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public Evado.Model.EvEventCodes DeleteItem ( Evado.Model.Digital.EdUserProfile Profile )
    {
      this.LogMethod ( "DeleteItem method. "
      + " UserId: " + Profile.UserId );

      // 
      // Check that the UserId is valid.
      // 
      if ( Profile.Guid == Guid.Empty )
      {
        return Evado.Model.EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_UpdatedBy, SqlDbType.NVarChar,100),
        new SqlParameter( PARM_UpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UpdateDate, SqlDbType.DateTime)
      };
      cmdParms [ 0 ].Value = Profile.Guid;
      cmdParms [ 1 ].Value = this.ClassParameters.UserProfile.UserId;
      cmdParms [ 2 ].Value = this.ClassParameters.UserProfile.CommonName;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_DeleteItem, cmdParms ) == 0 )
      {
        return Evado.Model.EvEventCodes.Database_Record_Update_Error;
      }

      return Evado.Model.EvEventCodes.Ok;

    }//END DeleteItem method

    #endregion


  }//END EvUserProfiles class

}//END namespace Evado.Dal.Digital

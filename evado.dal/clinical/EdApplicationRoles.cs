/***************************************************************************************
 * <copyright file="dal\EvActivityForms.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//References to Evado specific libraries

using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Clinical
{
  /// <summary>
  /// A business Component used to manage Ethics roles
  /// The Evado.Model.TrialVisitForm is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  public class EdApplicationRoles : EvDalBase
  {
    #region Class Initialization

    /// <summary>
    /// This is the class initialisation method.
    /// </summary>
    public EdApplicationRoles ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EdApplicationRoles.";
    }

    /// <summary>
    /// This is the class initialisation method with settings configured.
    /// </summary>
    public EdApplicationRoles ( EvClassParameters ClassParameters )
    {
      this.ClassParameters = ClassParameters;

      this.ClassNameSpace = "Evado.Dal.Clinical.EdApplicationRoles.";
    }

    #endregion

    #region Class Constant

    /// <summary>
    /// This constant defines a sql query for a view
    /// </summary>
    private const string _sqlQuery_View = "Select * FROM ED_APPLICATION_ROLES ";

    //
    // The field and parameter values for the SQl customer filter 
    //
    private const string DB_CUSTOMER_GUID = "CU_GUID";
    private const string PARM_CUSTOMER_GUID = "@CUSTOMER_GUID";

    private const string DB_APPLICATION_SETTING_GUID = "AS_GUID";
    private const string DB_ROLE_ID = "AR_ROLE_ID";
    private const string DB_ROLES_DESCRIPTION = "AR_DESCRIPTION";

    private const string PARM_APPLICATION_GUID = "@APPLICATION_GUID";
    #endregion

    #region Data Reader methods

    // ==================================================================================
    /// <summary>
    /// This method reads the content of the data row object containing a query result
    /// into an Activity Record object.
    /// </summary>
    /// <param name="Row">DataRow: a data row record object</param>
    /// <returns>EvActivityForm: a data row object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Extract the data object values from the data row object and add to the activity form object.
    /// 
    /// 2. Return the activity form object.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private EdRole readDataRow ( DataRow Row )
    {
      // 
      // Initialise activity form object
      // 
      EdRole role = new EdRole ( );

      // 
      // Extract the data object values from the data row object and add to the activity form object.
      // 
      role.RoleId = EvSqlMethods.getString ( Row, EdApplicationRoles.DB_ROLE_ID );
      role.Description = EvSqlMethods.getString ( Row, EdApplicationRoles.DB_ROLES_DESCRIPTION );

      // 
      // Return the activity form object.
      // 
      return role;

    }// End readDataRow method.

    #endregion

    #region List and queries methods

    // ==================================================================================
    /// <summary>
    /// This method returns a list of application roles
    /// </summary>
    /// <param name="Application"EdApplication object</param>
    // ----------------------------------------------------------------------------------
    public List<EdRole> getRoleList ( EdApplication Application )
    {
      this.LogMethod ( "getRoleList method. " );
      this.LogValue ( "ApplicationId: " + Application.ApplicationId );
      this.LogValue ( "ApplicationGuid: " + Application.Guid );
      //
      // Initialize the method status string, an sql query string and a return list of activity records
      //
      string sqlQueryString;
      List<EdRole> roleList = new List<EdRole> ( );
      EdRole role = new EdRole();

      //
      // Add static roles.
      //
      foreach ( EdRole rol in EvUserProfile.StaticRoles )
      {
        if ( role.Description.Contains ( "Evado" ) == true )
        {
          if ( this.ClassParameters.UserProfile.CustomerGuid == this.ClassParameters.PlatformGuid )
          {
            roleList.Add ( rol );
          }
        }
        else
        {
          roleList.Add ( rol );
        }
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdApplicationRoles.PARM_APPLICATION_GUID, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = Application.Guid;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE " + EdApplicationRoles.DB_APPLICATION_SETTING_GUID + " = " + EdApplicationRoles.PARM_APPLICATION_GUID + " "
        + " ORDER BY +" + EdApplicationRoles.DB_ROLE_ID + " ";

      this.LogDebug ( sqlQueryString );

      //
      // Execute the query against the database      
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

           role = this.readDataRow ( row );


          this.LogValue ( "Activity: " + role.RoleId + ", Description: " + role.Description );

          roleList.Add ( role );

        } //END interation loop.

      }//END using method

      this.LogValue ( "view count: " + roleList.Count );

      // 
      // Return the list containing the User data object.
      // 
      return roleList;

    }//END getView method.

    #endregion

    #region update methods

    // ==================================================================================
    /// <summary>
    /// This class updates the activity record data object. 
    /// </summary>
    /// <param name="Application">EvActivity: an activity object</param>
    /// <returns>EvEventCodes: an event code for update data object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Exit, if the RoleId or Activity's Guid or the Old activity object's Guid is empty.
    /// 
    /// 2. Generate the DB row Guid, if it does not exist. 
    /// 
    /// 3. Define the SQL query parameters and execute the storeprocedure for updating items.
    /// 
    /// 4. Return an event code for updating items. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvEventCodes updateItems ( EdApplication Application )
    {
      this.LogMethod ( "updateItems " );
      this.LogValue ( "ActivityGuid: " + Application.Guid );
      this.LogValue ( "RoleList count: " + Application.RoleList.Count );
      //
      // Initialize the Sql update query string. 
      //
      System.Text.StringBuilder SqlUpdateQuery = new System.Text.StringBuilder ( );
      bool savedItems = false;

      if ( Application.RoleList.Count == 0 )
      {
        this.LogValue ( "No forms in the list" );
        this.LogMethodEnd ( "updateItems " );
        return EvEventCodes.Ok;
      }

      //
      // Delete the milestone activities for this milestone.
      //
      SqlUpdateQuery.AppendLine ( "/** DELETE ALL OF ACTIVITy FORMS FOR THE ED_APPLICATION_ROLES **/" );
      SqlUpdateQuery.AppendLine ( " DELETE FROM ED_APPLICATION_ROLES " );
      SqlUpdateQuery.AppendLine ( " WHERE  ("+EdApplicationRoles.DB_APPLICATION_SETTING_GUID+" = '" + Application.Guid + "') ; \r\n" );
      //SqlUpdateQuery.AppendLine ( "GO " );

      foreach ( EdRole role in Application.RoleList )
      {
        //
        // skip empty role identifiers.
        //
        if ( role.RoleId == String.Empty )
        {
          continue;
        }

        //
        // skip static roles.
        //
        if ( role.RoleId == EvUserProfile.CONST_ADMINISTRATOR_ROLE
          || role.RoleId == EvUserProfile.CONST_MANAGER_ROLE
          || role.RoleId == EvUserProfile.CONST_DESIGNER_ROLE
          || role.RoleId == EvUserProfile.CONST_STAFF_ROLE )
        {
          continue;
        }

        this.LogDebug ( "RoleId: " + role.RoleId + " >> ADDED " );

        if ( role.Description == String.Empty )
        {
          role.Description = role.RoleId;
        }

        savedItems = true;

        string roleId = role.RoleId;
        string description = role.Description;

        if ( roleId.Length > 10 )
        {
          roleId = roleId.Substring ( 0, 10 );
        }

        if ( description.Length > 50 )
        {
          description = description.Substring ( 0, 50 );
        }

        SqlUpdateQuery.AppendLine ( "Insert Into ED_APPLICATION_ROLES " );
        SqlUpdateQuery.AppendLine ( "(" + EdApplicationRoles.DB_APPLICATION_SETTING_GUID + "," );
        SqlUpdateQuery.AppendLine ( " " + EdApplicationRoles.DB_ROLE_ID + "," );
        SqlUpdateQuery.AppendLine ( " " + EdApplicationRoles.DB_ROLES_DESCRIPTION + " )  " );
        SqlUpdateQuery.AppendLine ( "values  " );
        SqlUpdateQuery.AppendLine ( "('" + Application.Guid + "', " );
        SqlUpdateQuery.AppendLine ( "'" + roleId + "', " );
        SqlUpdateQuery.AppendLine ( "'" + description + "' ); \r\n" );

      }//END form list iteration loop.

      if ( savedItems == false )
      {
        this.LogMethodEnd ( "updateItems " );
        return EvEventCodes.Ok;
      }
      this.LogDebug ( "Sql Query: " + SqlUpdateQuery.ToString ( ) );

      if ( EvSqlMethods.QueryUpdate ( SqlUpdateQuery.ToString ( ), null ) == 0 )
      {
        this.LogError ( EvEventCodes.Database_Record_Update_Error, "ED_APPLICATION_ROLES databasse update error." );
        this.LogMethodEnd ( "updateItems " );
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Return code
      //       
      this.LogMethodEnd ( "updateItems " );
      return EvEventCodes.Ok;

    }//END updateItem class

    #endregion

  }//END EvActivityForms class

}//END namespace Evado.Dal.Clinical
